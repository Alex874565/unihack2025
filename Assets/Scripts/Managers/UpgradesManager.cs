using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradesManager : MonoBehaviour
{

    public List<UpgradeData> PossibleUpgrades => _possibleUpgrades;
    public List<UpgradeData> CurrentUpgrades => _currentUpgrades;

    [SerializeField] private List<UpgradeData> _possibleUpgrades;
    [SerializeField] private List<UpgradeData> _currentUpgrades;
    [SerializeField] private int totalUpgrades = 56;

    [SerializeField] private AnimationCurve rarityCurve;
    private Dictionary<UpgradeType, Modifiers> _typeModifiers;

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
        return null; // Fallback, should not reach here
    }

    public UpgradeData GetPossibleUpgrade(int index)
    {
        return _possibleUpgrades[index];
    }

    public void MakeUpgrade(int index)
    {
        UpgradeData upgrade = GetPossibleUpgrade(index);

        _possibleUpgrades.RemoveAt(index);
        _currentUpgrades.Add(upgrade);
        foreach (var u in upgrade.PossibleUpgrades)
        {
            _possibleUpgrades.Add(u);
        }

        _typeModifiers[upgrade.Type] = CalculateTypeModifiers(upgrade.Type);
    }

    public Modifiers CalculateTypeModifiers(UpgradeType type)
    {
        Modifiers modifiers = new Modifiers();
        foreach (var upgrade in _currentUpgrades.Where(u => u.Type == type))
        {
            modifiers.IncomeModifier += upgrade.IncomeModifier;
            modifiers.PollutionModifier += upgrade.PollutionModifier;
        }
        return modifiers;
    }

    public float GetUpgradeWeight(UpgradeData upgrade)
    {
        float progress = 1f - ((float)_possibleUpgrades.Count / (float)totalUpgrades);
        float globalRarityMultiplier = Mathf.Lerp(1f, 0f, progress);
        float weight = rarityCurve.Evaluate((float)upgrade.Phase) * globalRarityMultiplier;
        return weight;
    }
}
