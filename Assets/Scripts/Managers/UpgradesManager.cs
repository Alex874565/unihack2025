using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradesManager : MonoBehaviour
{

    public List<UpgradeData> PossibleUpgrades => _possibleUpgrades;
    public List<UpgradeData> CurrentUpgrades => _currentUpgrades;
    public Dictionary<ModuleTypes, Modifiers> ModuleUpgradeModifiers => _moduleUpgradeModifiers;
    public Dictionary<ModuleTypes, Modifiers> ModuleBaseModifiers => _moduleBaseModifiers;

    [SerializeField] private List<UpgradeData> _possibleUpgrades;
    [SerializeField] private List<UpgradeData> _currentUpgrades;
    [SerializeField] private int totalUpgrades = 56;

    [SerializeField] private AnimationCurve rarityCurve;

    [SerializeField] private Dictionary<ModuleTypes, Modifiers> _moduleUpgradeModifiers;
    [SerializeField] private Dictionary<ModuleTypes, Modifiers> _moduleBaseModifiers;

    public void Awake()
    {
        _moduleUpgradeModifiers = new Dictionary<ModuleTypes, Modifiers>();
        _moduleBaseModifiers = new Dictionary<ModuleTypes, Modifiers>();
        _possibleUpgrades = new List<UpgradeData>();
        _currentUpgrades = new List<UpgradeData>();
    }

    public UpgradeData GetPossibleWeightedUpgrade()
    {
        float totalWeight = 0f;
        foreach (var upgrade in _possibleUpgrades)
        {
            totalWeight += GetUpgradeWeight(upgrade);
        }
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (var upgrade in _possibleUpgrades)
        {
            cumulativeWeight += GetUpgradeWeight(upgrade);
            if (randomValue <= cumulativeWeight)
            {
                return upgrade;
            }
        }
        Debug.LogWarning("GetPossibleWeightedUpgrade - No upgrade selected, returning null.");
        return null; // Fallback, should not reach here
    }

    public UpgradeData GetPossibleUpgrade(int index)
    {
        return _possibleUpgrades[index];
    }

    public void MakeUpgrade(int index)
    {
        UpgradeData upgrade = GetPossibleUpgrade(index);
        //Debug.Log("MakeUpgrade - Adding: " + upgrade.Name + "; removing: " + _possibleUpgrades[index].Name);
        if (upgrade.IsNewTier)
        {
            _possibleUpgrades = _possibleUpgrades.Where(u => u.ModuleType != upgrade.ModuleType).ToList();
            _currentUpgrades = _currentUpgrades.Where(u => u.ModuleType != upgrade.ModuleType).ToList();
            _moduleUpgradeModifiers.Remove(upgrade.ModuleType);
            _moduleBaseModifiers[upgrade.ModuleType] = upgrade.Modifiers;
        }
        else
        {
            _possibleUpgrades.RemoveAt(index);
            _currentUpgrades.Add(upgrade);
        }
        ApplyUpgradeModifiers(upgrade);
        foreach (var u in upgrade.PossibleUpgrades)
        {
            _possibleUpgrades.Add(u);
        }
    }

    public float GetUpgradeWeight(UpgradeData upgrade)
    {
        float progress = 1f - ((float)_possibleUpgrades.Count / (float)totalUpgrades);
        float globalRarityMultiplier = Mathf.Lerp(1f, 0f, progress);
        float weight = rarityCurve.Evaluate((float)upgrade.Phase) * globalRarityMultiplier;
        return weight;
    }

    public int GetUpgradeIndexByName(UpgradeData upgrade)
    {
        for (int i = 0; i < _possibleUpgrades.Count; i++)
        {
            if (_possibleUpgrades[i].Name == upgrade.Name)
            {
                return i;
            }
        }
        return -1; // Not found
    }

    public void AddPossibleUpgrade(UpgradeData upgradeData)
    {
        if(!_currentUpgrades.Contains(upgradeData) && !_possibleUpgrades.Contains(upgradeData))
        {
            _possibleUpgrades.Add(upgradeData);
        }
        //Debug.Log("Added possible upgrade: " + upgradeData.Name);
    }

    public void ApplyUpgradeModifiers(UpgradeData upgradeData)
    {
        //Debug.Log("ApplyUpgradeModifiers - Applying upgrade modifiers for: " + upgradeData.Name);
        if (!_moduleUpgradeModifiers.ContainsKey(upgradeData.ModuleType))
        {
            _moduleUpgradeModifiers[upgradeData.ModuleType] = upgradeData.Modifiers;
        }
        else
        {
            _moduleUpgradeModifiers[upgradeData.ModuleType] += upgradeData.Modifiers;
        }
        //Debug.Log("ApplyUpgradeModifiers - Upgrade modifiers applied to " + upgradeData.ModuleType + ": " + _moduleUpgradeModifiers[upgradeData.ModuleType]);
        ServiceLocator.Instance.ModulesManager.CalculateModuleTypeProduction(upgradeData.ModuleType);
    }
}
