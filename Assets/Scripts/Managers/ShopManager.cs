using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int _itemsCount = 3;

    private List<UpgradeData> _selectedUpgrades = new List<UpgradeData>();
    private UpgradesManager _upgradesManager;
    private ModulesManager _modulesManager;

    public void Start()
    {
        _upgradesManager = GameManager.Instance.UpgradesManager;
        _modulesManager = GameManager.Instance.ModulesManager;
    }

    public void SelectUpgrades()
    {
        _selectedUpgrades.Clear();
        for (int i = 0; i < _itemsCount; i++)
        {
            if(_modulesManager.OwnedModules.Count == 0)
            {
                Debug.Log(_modulesManager.GetRandomModule());
            }
            UpgradeData upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
            while (_selectedUpgrades.Contains(upgrade) || upgrade == null)
            {
                upgrade = _upgradesManager.GetPossibleWeightedUpgrade();
            }
            _selectedUpgrades.Add(upgrade);
            Debug.Log("Selected Upgrade: " + upgrade.Name);
        }
    }
}
