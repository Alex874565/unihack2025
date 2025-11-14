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
    public GameObject ModulePrefab => _modulePrefab;

    [SerializeField] private string _moduleName;
    [SerializeField] private ModuleTypes _moduleType;
    [SerializeField] private string _moduleDescription;
    [SerializeField] private string _income;
    [SerializeField] private string _pollution;
    [SerializeField] private string _productionSpeed;
    [SerializeField] private string _cost;
    [SerializeField] private Sprite _icon;
    [SerializeField] private GameObject _modulePrefab;
    [SerializeField] private List<UpgradeData> _upgrades;
}
