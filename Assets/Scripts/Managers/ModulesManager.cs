using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulesManager : MonoBehaviour
{
    public bool IsPlacingModule => _modulePlacer != null && _modulePlacer.IsPlacingModule;
    public ModulesGrid ModulesGrid => _modulesGrid;
    public List<GameObject> OwnedModules => _ownedModules;
    public Dictionary<ModuleTypes, Modifiers> ModuleTypeProductions => _moduleTypeProductions;
    public ModulesDatabase ModulesDatabase => _modulesDatabase;
    [SerializeField] private ModulesDatabase _modulesDatabase;
    [SerializeField] private ModulesGrid _modulesGrid;
    [SerializeField] private List<GameObject> _ownedModules;
    [SerializeField] private ModulePlacer _modulePlacer;
    [SerializeField] private Dictionary<ModuleTypes, Modifiers> _moduleTypeProductions;

    [SerializeField] private ModuleData _generator;
    [SerializeField] private ModuleData _barn;
    [SerializeField] private ModuleData _vehicles;

    public ModuleData test;

    private void Awake()
    {
        _ownedModules = new List<GameObject>();
        _moduleTypeProductions = new Dictionary<ModuleTypes, Modifiers>();
    }

    private void Start()
    {
    }

    public void BuyInitialModules()
    {
        ServiceLocator.Instance.TutorialManager.StartPlacingModules();
        BuyModule(_barn);
        BuyModule(_vehicles);
        BuyModule(_generator);
    }

    public void BuyModule(ModuleData module)
    {
        if (!module)
        {
            module = test;
        }
        foreach (var upgrade in module.Upgrades)
        {
            ServiceLocator.Instance.UpgradesManager.AddPossibleUpgrade(upgrade);
        }
       
        SetModuleTypeProduction(module.ModuleType, module.BaseProduction);
        
        _modulePlacer.StartPlacingModule(module);
    }

    public ModuleData GetRandomModule()
    {
        List<ModuleData> buyableModules = _modulesDatabase.Modules.Where(m => m.ModuleType != ModuleTypes.Generator && m.ModuleType != ModuleTypes.Vehicles && m.ModuleType != ModuleTypes.Barn).ToList();
        int index = Random.Range(0, buyableModules.Count);
        Debug.Log("GetRandomModule - Selected index: " + index);
        ModuleData module = buyableModules[index];
        Debug.Log(module.ModuleName);
        return module;
    }

    public void CalculateModuleTypeProduction(ModuleTypes moduleType)
    {
        Debug.LogWarning("CalculateModuleTypeProduction - moduleType is Barn, returning.");
        Modifiers barnProduction = _modulesDatabase.Modules.Where(m => m.ModuleType == ModuleTypes.Barn).First().BaseProduction;

        if (ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers.ContainsKey(ModuleTypes.Barn))
        {
            barnProduction += ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers[ModuleTypes.Barn];
        }

        if (moduleType == ModuleTypes.Barn)
        {
            SetModuleTypeProduction(ModuleTypes.Barn, barnProduction);

            return;
        }
        Debug.Log("CalculateModuleTypeProduction - Calculating production for module type: " + moduleType);
        Modifiers totalProduction = _modulesDatabase.Modules.Where(m => m.ModuleType == moduleType).First().BaseProduction;

        if(ServiceLocator.Instance.UpgradesManager.ModuleBaseModifiers.ContainsKey(moduleType))
        {
            totalProduction = ServiceLocator.Instance.UpgradesManager.ModuleBaseModifiers[moduleType];
            Debug.Log("CalculateModuleTypeProduction - Found upgrade modifiers for module type: " + moduleType + ", value: " + ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers[moduleType]);
        }

        // add flat upgrade modifiers
        if (ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers.ContainsKey(moduleType)) {
            totalProduction += ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers[moduleType];
        }

        Debug.Log("Barn Modifier for " + moduleType + ": " + barnProduction);
        Debug.Log("Pre-Barn Total production for " + moduleType + ": " + totalProduction);
        totalProduction += totalProduction.Abs() * barnProduction / 100;
        Debug.Log("Post-Barn Total production for " + moduleType + ": " + totalProduction);

        // add percent booster modifiers
        if (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers.ContainsKey(moduleType)) {
            totalProduction += totalProduction.Abs() * (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers[moduleType]/100);
        }

        SetModuleTypeProduction(moduleType, totalProduction);

        Debug.Log("CalculateModuleTypeProduction - Total production for " + moduleType + ": " + totalProduction);
    }

    public void SetModuleTypeProduction(ModuleTypes moduleType, Modifiers production)
    {
        _moduleTypeProductions[moduleType] = production;

        Debug.Log("CalculateModuleTypeProduction - New production for " + moduleType + ": " + production);
    }

    public void AddOwnedModule(GameObject module)
    {
        _ownedModules.Add(module);
    }

    public void UpdateModuleTypeVisuals(ModuleTypes moduleType, UpgradePhases phase)
    {
        Debug.Log("UpdateModuleTypeVisuals - Updating visuals for module type: " + moduleType + " to phase: " + phase);
        foreach (var module in _ownedModules)
        {
            Debug.Log("UpdateModuleTypeVisuals - Checking module: " + module.name);
            ModuleBehaviour moduleBehaviour = module.GetComponent<ModuleBehaviour>();
            if (moduleBehaviour.ModuleData.ModuleType == moduleType)
            {
                Debug.Log("UpdateModuleTypeVisuals - Updating module: " + module.name);
                moduleBehaviour.UpdateVisuals(phase);
            }
        }
    }
}
