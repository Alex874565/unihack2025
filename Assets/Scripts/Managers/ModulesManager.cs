using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulesManager : MonoBehaviour
{
    public ModulesGrid ModulesGrid => _modulesGrid;
    public List<ModuleData> OwnedModules => _ownedModules;
    [SerializeField] private ModulesDatabase _modulesDatabase;
    [SerializeField] private ModulesGrid _modulesGrid;
    [SerializeField] private List<ModuleData> _ownedModules;
    [SerializeField] private ModulePlacer _modulePlacer;

    public ModuleData test;

    private void Awake()
    {
        _ownedModules = new List<ModuleData>();
    }

    public void BuyModule(ModuleData module)
    {
        if (!module)
        {
            module = test;
        }
        foreach(var upgrade in module.Upgrades)
        {
            ServiceLocator.Instance.UpgradesManager.AddPossibleUpgrade(upgrade);
        }
        _modulePlacer.StartPlacingModule(module);
        _ownedModules.Add(module);
    }

    public ModuleData GetRandomModule()
    {
        int index = Random.Range(0, _modulesDatabase.Modules.Count);
        ModuleData module = _modulesDatabase.Modules[index];
        Debug.Log(module.ModuleName);
        return module;
    }
}
