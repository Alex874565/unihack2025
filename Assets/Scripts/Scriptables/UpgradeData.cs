using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData", order = 2)]
public class UpgradeData: ScriptableObject
{
    public string Name => _name;
    public int Cost => _cost;
    public Sprite Icon => _icon;
    public string Description => _description;
    public int IncomeModifier => _incomeModifier;
    public int PollutionModifier => _pollutionModifier;
    public UpgradeType Type => _type;
    public List<UpgradeData> PossibleUpgrades => _possibleUpgrades;

    public UpgradePhase Phase => _phase;

    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private int _incomeModifier;
    [SerializeField] private int _pollutionModifier;
    [SerializeField] private UpgradeType _type;
    [SerializeField] private UpgradePhase _phase;
    [SerializeField] private List<UpgradeData> _possibleUpgrades;
}
