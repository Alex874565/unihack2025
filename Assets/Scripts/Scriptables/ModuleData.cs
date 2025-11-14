using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ModuleData", menuName = "ScriptableObjects/ModuleData", order = 4)]
public class ModuleData : ScriptableObject
{
    public string ModuleName => _moduleName;
    public string ModuleDescription => _moduleDescription;
    public string Income => _income;
    public string Pollution => _pollution;
    public string Cost => _cost;

    public Sprite ModuleIcon => _icon;
    public List<UpgradeData> Upgrades => _upgrades;

    [SerializeField] private string _moduleName;
    [SerializeField] private string _moduleDescription;
    [SerializeField] private string _income;
    [SerializeField] private string _pollution;
    [SerializeField] private string _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<UpgradeData> _upgrades;
}
