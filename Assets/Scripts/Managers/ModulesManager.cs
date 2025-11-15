using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulesManager : MonoBehaviour
{
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
        BuyInitialModules();
    }

    public void BuyInitialModules()
    {
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
        if (module.ModuleType == ModuleTypes.Barn)
        {
            Modifiers barnProduction = new Modifiers();
            barnProduction.AirPollutionModifier = 0;
            barnProduction.SoilPollutionModifier = 0;
            barnProduction.WaterPollutionModifier = 0;
            SetModuleTypeProduction(ModuleTypes.Barn, barnProduction);
        }
        else
        {
            SetModuleTypeProduction(module.ModuleType, module.BaseProduction);
        }
        _modulePlacer.StartPlacingModule(module);
    }

    public ModuleData GetRandomModule()
    {
        List<ModuleData> buyableModules = _modulesDatabase.Modules.Where(m => m.ModuleType != ModuleTypes.Generator && m.ModuleType != ModuleTypes.Vehicles && m.ModuleType != ModuleTypes.Barn).ToList();
        int index = Random.Range(0, buyableModules.Count - 1);
        ModuleData module = buyableModules[index];
        Debug.Log(module.ModuleName);
        return module;
    }

    public void CalculateModuleTypeProduction(ModuleTypes moduleType)
    {
        if(moduleType == ModuleTypes.Barn)
        {
            Debug.LogWarning("CalculateModuleTypeProduction - moduleType is Barn, returning.");

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

        // add barn modifiers 
        Modifiers barnModifiers = _barn.BaseProduction;
        if(ServiceLocator.Instance.UpgradesManager.ModuleBaseModifiers.ContainsKey(ModuleTypes.Barn))
        {
            barnModifiers = ServiceLocator.Instance.UpgradesManager.ModuleBaseModifiers[ModuleTypes.Barn];
        }

        if (ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers.ContainsKey(ModuleTypes.Barn))
        {
            barnModifiers += ServiceLocator.Instance.UpgradesManager.ModuleUpgradeModifiers[ModuleTypes.Barn];
        }
        totalProduction += totalProduction * barnModifiers / 100;

        // add percent booster modifiers
        if (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers.ContainsKey(moduleType)) {
            totalProduction += totalProduction * (ServiceLocator.Instance.BoostersManager.ModuleBoosterModifiers[moduleType]/100);
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
        foreach (var module in _ownedModules)
        {
            ModuleBehaviour moduleBehaviour = module.GetComponent<ModuleBehaviour>();
            if (moduleBehaviour.ModuleData.ModuleType == moduleType)
            {
                moduleBehaviour.UpdateVisuals(phase);
            }
        }
    }
}
