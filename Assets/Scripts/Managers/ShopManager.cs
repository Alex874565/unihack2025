using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int _itemsCount;
    [SerializeField] private float _moduleProbability;
    [SerializeField] private float _boosterProbability;

    private List<ShopItem> _selectedItems;
    private UpgradesManager _upgradesManager;
    private ModulesManager _modulesManager;

    public void Start()
    {
        _upgradesManager = ServiceLocator.Instance.UpgradesManager;
        _modulesManager = ServiceLocator.Instance.ModulesManager;
    }

    public void SelectNextItems()
    {
        Debug.Log("Selecting Upgrades...");

        if (!_modulesManager.ModulesGrid.AreSpacesLeft())
        {
            _boosterProbability += _moduleProbability;
            _moduleProbability = 0f;
        }

        _selectedItems = new List<ShopItem>();

        for (int i = 0; i < _itemsCount; i++)
        {
            if (_modulesManager.OwnedModules.Count == 0)
            {
                Debug.Log("No owned modules.");
                SelectModule();
            }
            else if(_upgradesManager.PossibleUpgrades.Count > 0)
            {
                float random = Random.Range(0f, 99f);
                if (random < _moduleProbability)
                {
                    SelectModule();
                }
                else if (random > 99 - _boosterProbability)
                {
                    SelectBooster();
                }
                else
                {
                    SelectUpgrade();   
                }
            }
            else
            {
                SelectModuleOrBooster();
            }
        }
    }

    private void SelectModule()
    {
        ShopItem item = new ShopItem();
        Debug.Log("Selected Module as item.");
        ModuleData module = _modulesManager.GetRandomModule();
        item.ModuleData = module;
        Debug.Log("Selected Module: " + module.ModuleName);
        _selectedItems.Add(item);
    }

    private void SelectBooster()
    {
        var item = new ShopItem();
        Debug.Log("Selected Booster as item.");
        GlobalModifierData booster = ServiceLocator.Instance.BoostersManager.GetWeightedBooster();
        item.BoosterData = booster;
        Debug.Log("Selected Booster: " + booster.Name);
        _selectedItems.Add(item);
    }

    private void SelectUpgrade()
    {
        ShopItem item = new ShopItem();
        UpgradeData upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
        Debug.Log("Attempting to select upgrade: " + (upgrade != null ? upgrade.Name : "null"));
        int index = 0;
        while (_selectedItems.Any(t => t.UpgradeData && t.UpgradeData.Name == upgrade.Name) || upgrade == null)
        {
            if (index > 5)
            {
                Debug.Log("Too many attempts to select a unique upgrade, choosing others.");
                SelectModuleOrBooster();
                break;
            }
            Debug.Log(upgrade == null ? "Upgrade is null, reselecting." : "Duplicate upgrade selected, reselecting.");
            upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
            index++;
        }
        item.UpgradeData = upgrade;
        _selectedItems.Add(item);
        Debug.Log("Selected Upgrade: " + upgrade.Name);
    }

    public void SelectModuleOrBooster()
    {
        if (!_modulesManager.ModulesGrid.AreSpacesLeft())
        {
            SelectBooster();
        }
        else
        {
            var random = Random.Range(0f, 99f);
            if (random < 50)
            {
                SelectModule();
            }
            else
            {
                SelectBooster();
            }
        }
    }

    public void ChooseItem(ShopItem item)
    {
        Debug.Log("Item chosen from shop.");
        if(item.UpgradeData)
        {
            _upgradesManager.MakeUpgrade(_upgradesManager.GetUpgradeIndexByName(item.UpgradeData));
        }else if (item.ModuleData)
        {
            _modulesManager.BuyModule(item.ModuleData);
        }else if (item.BoosterData)
        {
            ServiceLocator.Instance.BoostersManager.AddBooster(item.BoosterData);
        }
    }
}
