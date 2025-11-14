using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int _itemsCount = 3;

    private List<ShopItem> _selectedItems;
    private UpgradesManager _upgradesManager;
    private ModulesManager _modulesManager;

    public void Start()
    {
        _upgradesManager = ServiceLocator.Instance.UpgradesManager;
        _modulesManager = ServiceLocator.Instance.ModulesManager;
    }

    public void SelectUpgrades()
    {
        Debug.Log("Selecting Upgrades...");
        _selectedItems = new List<ShopItem>();
        for (int i = 0; i < _itemsCount; i++)
        {
            ShopItem item = new ShopItem();
            if (_modulesManager.OwnedModules.Count == 0)
            {
                Debug.Log("No owned modules, selecting module as upgrade.");
                _modulesManager.GetRandomModule();
            }
            else
            {
                int x = Random.Range(0, 99);
                if(x < 25)
                {
                    Debug.Log("Selected Module as Upgrade.");
                    _modulesManager.GetRandomModule();
                    continue;
                }

                UpgradeData upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
                while (_selectedItems.Any(t => t.UpgradeData.Name == upgrade.Name) || upgrade == null)
                {
                    Debug.Log(upgrade == null ? "Upgrade is null, reselecting." : "Duplicate upgrade selected, reselecting.");
                    upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
                }
                item.UpgradeData = upgrade;
                _selectedItems.Add(item);
                Debug.Log("Selected Upgrade: " + upgrade.Name);
            }
        }
    }

    public void ChooseItem()
    {
        Debug.Log("Item chosen from shop.");
    }
}
