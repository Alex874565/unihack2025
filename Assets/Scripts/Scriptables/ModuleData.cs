using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ModuleData", menuName = "ScriptableObjects/ModuleData", order = 4)]
public class ModuleData : ScriptableObject
{
    public string ModuleName => _moduleName;
    public string ModuleDescription => _moduleDescription;
    public Modifiers BaseProduction => _baseProduction;
    public int Cost => _cost;

    public Sprite Icon => _icon;
    public List<UpgradeData> Upgrades => _upgrades;
    public GameObject ModulePrefab => _modulePrefab;
    public ModuleTypes ModuleType => _moduleType;

    [SerializeField] private string _moduleName;
    [SerializeField] private ModuleTypes _moduleType;
    [SerializeField] private string _moduleDescription;
    [SerializeField] private Modifiers _baseProduction;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private GameObject _modulePrefab;
    [SerializeField] private List<UpgradeData> _upgrades;
}
