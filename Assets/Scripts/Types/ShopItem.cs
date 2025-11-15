using UnityEngine;
using System.Linq;

[System.Serializable]
public class ShopItem
{
    public ShopItemTypes ShopItemType;
    public UpgradeData UpgradeData;
    public ModuleData ModuleData;
    public GlobalModifierData BoosterData;

    public int GetPrice()
    {
        switch (ShopItemType)
        {
            case ShopItemTypes.Upgrade:
                if (UpgradeData != null)
                {
                    return UpgradeData.Cost;
                }
                return 0;
                break;
            case ShopItemTypes.Module:
                if (ModuleData != null)
                {
                    return ModuleData.Cost;
                }
                return 0;
                break;
            case ShopItemTypes.Booster:
                if (BoosterData != null)
                {
                    return ServiceLocator.Instance.BoostersManager.BoostersDatabase.Boosters.Where(b => b.Booster.Name == BoosterData.Name).First().Cost;
                }
                return 0;
                break;
            default:
                return 0;
                break;
        }
    }
}
