using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShopUIManager : MonoBehaviour
{
    // Implementation of ShopUIManager
    [SerializeField] private List<UpgradePhaseBackground> _upgradeSprites;
    [SerializeField] private Sprite _moduleSprite;
    [SerializeField] private List<BoosterTierBackground> _boosterSprites;
    [SerializeField] private List<SpriteRenderer> _backgroundRenderers;


    public void SetBackgrounds(List<ShopItem> items)
    {
        foreach (var item in items)
        {
            SpriteRenderer backgroundRenderer = _backgroundRenderers[items.IndexOf(item)];
            if(item.ShopItemType == ShopItemTypes.Upgrade)
            {
                var upgradePhase = item.UpgradeData.Phase;
                var upgradeBackground = _upgradeSprites.FirstOrDefault(x => x.Phase == upgradePhase);
                if (upgradeBackground != null)
                {
                    backgroundRenderer.sprite = upgradeBackground.BackgroundSprite;
                }
            }
            else if (item.ShopItemType == ShopItemTypes.Module)
            {
                backgroundRenderer.sprite = _moduleSprite;
            }
            else if (item.ShopItemType == ShopItemTypes.Booster)
            {
                var boosterTier = ServiceLocator.Instance.BoostersManager.GetBoosterTier(item.BoosterData.Name);
                var boosterBackground = _boosterSprites.FirstOrDefault(x => x.BoosterTier == boosterTier);
                if (boosterBackground != null)
                {
                    backgroundRenderer.sprite = boosterBackground.BackgroundSprite;
                }
            }
        }
    }
}