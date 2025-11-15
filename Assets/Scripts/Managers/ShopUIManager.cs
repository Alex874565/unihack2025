using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    // Implementation of ShopUIManager
    [SerializeField] private Sprite _upgradeBackgroundSprite;
    [SerializeField] private Sprite _moduleBackgroundSprite;
    [SerializeField] private Sprite _boosterBackgroundSprite;

    [SerializeField] private GameObject _upgradesCanvas;

    [SerializeField] private List<Image> _backgroundRenderers;
    [SerializeField] private List<Image> _iconRenderers;
    [SerializeField] private List<TMP_Text> _nameFields;
    [SerializeField] private List<TMP_Text> _tierFields;
    [SerializeField] private List<Color> _upgradeTierColors;
    [SerializeField] private List<Color> _boosterTierColors;
    [SerializeField] private List<TMP_Text> _descriptionFields;
    [SerializeField] private List<TMP_Text> _incomeFields;
    [SerializeField] private List<TMP_Text> _speedFields;
    [SerializeField] private List<TMP_Text> _airPollutionFields;
    [SerializeField] private List<TMP_Text> _soilPollutionFields;
    [SerializeField] private List<TMP_Text> _waterPollutionFields;
    [SerializeField] private List<TMP_Text> _costFields;

    [SerializeField] private List<ShopItem> items;

    private void Start()
    {
        ShowShop(items);
    }

    public void ShowShop(List<ShopItem> items)
    {
        SetAllUIElements(items);
        _upgradesCanvas.SetActive(true);
    }

    public void HideShop()
    {
        _upgradesCanvas.SetActive(false);
    }

    public void SetBackgrounds(List<ShopItem> items)
    {
        foreach (var item in items)
        {
            Image backgroundRenderer = _backgroundRenderers[items.IndexOf(item)];
            if (item.ShopItemType == ShopItemTypes.Upgrade)
            {
                backgroundRenderer.sprite = _upgradeBackgroundSprite;
            }
            else if (item.ShopItemType == ShopItemTypes.Module)
            {
                backgroundRenderer.sprite = _moduleBackgroundSprite;
            }
            else if (item.ShopItemType == ShopItemTypes.Booster)
            {
                backgroundRenderer.sprite = _boosterBackgroundSprite;
            }
        }
    }

    public void SetIcons(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            Image iconRenderer = _iconRenderers[items.IndexOf(item)];
            iconRenderer.sprite = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Icon,
                ShopItemTypes.Module => item.ModuleData.Icon,
                ShopItemTypes.Booster => item.BoosterData.Icon,
                _ => null
            };
        }
    }

    public void SetNames(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text nameField = _nameFields[items.IndexOf(item)];
            nameField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Name,
                ShopItemTypes.Module => item.ModuleData.ModuleType.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Name,
                _ => "Unknown Item"
            };
        }
    }

    public void SetTiers(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text tierField = _tierFields[items.IndexOf(item)];
            if (item.ShopItemType == ShopItemTypes.Booster)
            {
                var boosterTier = ServiceLocator.Instance.BoostersManager.GetBoosterTier(item.BoosterData.Name);
                tierField.text = boosterTier switch
                {
                    BoosterTiers.Tier1 => "Common",
                    BoosterTiers.Tier2 => "Uncommon",
                    BoosterTiers.Tier3 => "Rare",
                    BoosterTiers.Tier4 => "High Risk High Reward",
                    _ => ""
                };
                tierField.color = boosterTier switch
                {
                    BoosterTiers.Tier1 => _boosterTierColors[0],
                    BoosterTiers.Tier2 => _boosterTierColors[1],
                    BoosterTiers.Tier3 => _boosterTierColors[2],
                    BoosterTiers.Tier4 => _boosterTierColors[3],
                    _ => Color.white
                };
            }
            else if (item.ShopItemType == ShopItemTypes.Upgrade)
            {
                tierField.text = item.UpgradeData.Phase switch
                {
                    UpgradePhases.Phase1 => "Common",
                    UpgradePhases.Phase2 => "Uncommon",
                    UpgradePhases.Phase3 => "Rare",
                    UpgradePhases.Phase4 => "Awesome",
                    _ => ""
                };
                tierField.color = item.UpgradeData.Phase switch
                {
                    UpgradePhases.Phase1 => _upgradeTierColors[0],
                    UpgradePhases.Phase2 => _upgradeTierColors[1],
                    UpgradePhases.Phase3 => _upgradeTierColors[2],
                    UpgradePhases.Phase4 => _upgradeTierColors[3],
                    _ => Color.white
                };
            }
            else
            {
                tierField.text = "";
            }
        }
    }

    public void SetDescriptions(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text descriptionField = _descriptionFields[items.IndexOf(item)];
            descriptionField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Description,
                ShopItemTypes.Module => item.ModuleData.ModuleDescription,
                ShopItemTypes.Booster => item.BoosterData.Description,
                _ => ""
            };
        }
    }

    public void SetIncomeModifiers(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text incomeField = _incomeFields[items.IndexOf(item)];
            incomeField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Modifiers.IncomeModifier.ToString(),
                ShopItemTypes.Module => item.ModuleData.BaseProduction.IncomeModifier.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Modifiers.IncomeModifier.ToString("P0"),
                _ => ""
            };
        }
    }

    public void SetProductionSpeeds(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text speedField = _speedFields[items.IndexOf(item)];
            speedField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Modifiers.SpeedModifier.ToString(),
                ShopItemTypes.Module => item.ModuleData.BaseProduction.SpeedModifier.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Modifiers.SpeedModifier.ToString("P0"),
                _ => ""
            };
        }
    }

    public void SetWaterPollutions(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text waterPollutionField = _waterPollutionFields[items.IndexOf(item)];
            waterPollutionField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Modifiers.WaterPollutionModifier.ToString(),
                ShopItemTypes.Module => item.ModuleData.BaseProduction.WaterPollutionModifier.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Modifiers.WaterPollutionModifier.ToString("P0"),
                _ => ""
            };
        }
    }

    public void SetSoilPollutions(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text soilPollutionField = _soilPollutionFields[items.IndexOf(item)];
            soilPollutionField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Modifiers.SoilPollutionModifier.ToString(),
                ShopItemTypes.Module => item.ModuleData.BaseProduction.SoilPollutionModifier.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Modifiers.SoilPollutionModifier.ToString("P0"),
                _ => ""
            };
        }
    }

    public void SetAirPollutions(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text airPollutionField = _airPollutionFields[items.IndexOf(item)];
            airPollutionField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Modifiers.AirPollutionModifier.ToString(),
                ShopItemTypes.Module => item.ModuleData.BaseProduction.AirPollutionModifier.ToString(),
                ShopItemTypes.Booster => item.BoosterData.Modifiers.AirPollutionModifier.ToString("P0"),
                _ => ""
            };
        }
    }

    public void SetCosts(List<ShopItem> items)
    {
        foreach (ShopItem item in items)
        {
            TMP_Text costField = _costFields[items.IndexOf(item)];
            costField.text = item.ShopItemType switch
            {
                ShopItemTypes.Upgrade => item.UpgradeData.Cost.ToString(),
                ShopItemTypes.Module => item.ModuleData.Cost.ToString(),
                ShopItemTypes.Booster => ServiceLocator.Instance.BoostersManager.GetBoosterCost(item.BoosterData.Name).ToString(),
                _ => ""
            };
        }
    }

    public void SetAllUIElements(List<ShopItem> items)
    {
        SetBackgrounds(items);
        SetIcons(items);
        SetNames(items);
        SetTiers(items);
        SetDescriptions(items);
        SetIncomeModifiers(items);
        SetProductionSpeeds(items);
        SetAirPollutions(items);
        SetSoilPollutions(items);
        SetWaterPollutions(items);
        SetCosts(items);
    }
}