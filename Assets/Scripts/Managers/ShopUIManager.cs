using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUIManager : MonoBehaviour
{
    // Implementation of ShopUIManager
    [SerializeField] private Sprite _upgradeBackgroundSprite;
    [SerializeField] private Sprite _moduleBackgroundSprite;
    [SerializeField] private Sprite _boosterBackgroundSprite;

    [SerializeField] private GameObject _upgradesCanvas;

    [SerializeField] private List<Image> _backgroundRenderers;
    [SerializeField] private List<EventTrigger> _backgroundEventTriggers;
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

    private List<ShopItem> items;

    public void ShowShop()
    {
        this.items = ServiceLocator.Instance.ShopManager.GetSelectedItems();
        SetAllUIElements();
        _upgradesCanvas.SetActive(true);
    }

    public void HideShop()
    {
        _upgradesCanvas.SetActive(false);
    }

    public void SetBackground(ShopItem item)
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

    public void SetBackGroundEventTrigger(ShopItem item)
    {
            EventTrigger backgroundEventTrigger = _backgroundEventTriggers[items.IndexOf(item)];
            var trigger = new EventTrigger.Entry();
            trigger.eventID = EventTriggerType.PointerClick;
            trigger.callback.AddListener((data) =>
            {
                Debug.Log("Clicked on item: " + item.ShopItemType);
                ServiceLocator.Instance.ShopManager.ChooseItem(item);
            });
            backgroundEventTrigger.triggers.Add(trigger);
        
    }

    public void SetIcon(ShopItem item)
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

    public void SetName(ShopItem item)
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

    public void SetTier(ShopItem item)
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

    public void SetDescription(ShopItem item)
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

    public void SetIncomeModifier(ShopItem item)
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

    public void SetProductionSpeed(ShopItem item)
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

    public void SetWaterPollution(ShopItem item)
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

    public void SetSoilPollution(ShopItem item)
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

    public void SetAirPollution(ShopItem item)
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

    public void SetCost(ShopItem item)
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

    public void SetAllUIElements()
    {
        foreach (ShopItem item in items)
        {
            SetBackground(item);
            SetBackGroundEventTrigger(item);
            SetIcon(item);
            SetName(item);
            SetTier(item);
            SetDescription(item);
            SetIncomeModifier(item);
            SetProductionSpeed(item);
            SetAirPollution(item);
            SetSoilPollution(item);
            SetWaterPollution(item);
            SetCost(item);
        }
    }
}