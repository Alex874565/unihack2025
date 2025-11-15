using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulesManager : MonoBehaviour
{
    public ModulesGrid ModulesGrid => _modulesGrid;
    public List<ModuleData> OwnedModules => _ownedModules;
    public Dictionary<ModuleTypes, Modifiers> ModuleTypeProductions => _moduleTypeProductions;
    [SerializeField] private ModulesDatabase _modulesDatabase;
    [SerializeField] private ModulesGrid _modulesGrid;
    [SerializeField] private List<ModuleData> _ownedModules;
    [SerializeField] private ModulePlacer _modulePlacer;
    [SerializeField] private Dictionary<ModuleTypes, Modifiers> _moduleTypeProductions;

    public ModuleData test;

    private void Awake()
    {
        _ownedModules = new List<ModuleData>();
        _moduleTypeProductions = new Dictionary<ModuleTypes, Modifiers>();
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
        SetModuleTypeProduction(module.ModuleType, module.BaseProduction);
        _ownedModules.Add(module);
        _modulePlacer.StartPlacingModule(module);
    }

    public ModuleData GetRandomModule()
    {
        int index = Random.Range(0, _modulesDatabase.Modules.Count);
        ModuleData module = _modulesDatabase.Modules[index];
        Debug.Log(module.ModuleName);
        return module;
    }

    public void CalculateModuleTypeProduction(ModuleTypes moduleType)
    {
        Debug.Log("CalculateModuleTypeProduction - Calculating production for module type: " + moduleType);
        Modifiers totalProduction = _modulesDatabase.Modules.Where(m => m.ModuleType == moduleType).First().BaseProduction;
        
        // add flat upgrade modifiers
        if (ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers.ContainsKey(moduleType)) {
            totalProduction += ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers[moduleType];
        }

        // add percent booster modifiers
        if (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers.ContainsKey(moduleType)) {
            totalProduction += totalProduction * (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers[moduleType]/100);
        }
        
        // add flat event modifiers

        SetModuleTypeProduction(moduleType, totalProduction);
    }

    public void SetModuleTypeProduction(ModuleTypes moduleType, Modifiers production)
    {
        _moduleTypeProductions[moduleType] = production;

        Debug.Log("CalculateModuleTypeProduction - New production for " + moduleType + ": " + production);
    }
}
