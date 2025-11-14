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
    public Modifiers Modifiers => _modifiers;
    public List<UpgradeData> PossibleUpgrades => _possibleUpgrades;
    public UpgradePhases Phase => _phase;
    public ModuleTypes ModuleType => _moduleType;

    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private Modifiers _modifiers;
    [SerializeField] private UpgradePhases _phase;
    [SerializeField] private List<UpgradeData> _possibleUpgrades;
    [SerializeField] private ModuleTypes _moduleType;
}
