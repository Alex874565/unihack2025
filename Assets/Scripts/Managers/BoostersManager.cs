using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class BoostersManager : MonoBehaviour
{

    public event EventHandler OnBoostActivated;
    public event EventHandler OnBoostDeactivated;

    public List<GlobalModifierData> ActiveBoosters => _activeBoosters;
    public Dictionary<ModuleTypes, Modifiers> ModuleBoosterModifiers => _moduleBoosterModifiers;
    public List<float> BoosterDurations => _boosterDurations;

    public BoostersDatabase BoostersDatabase => _boostersDatabase;

    [SerializeField] private BoostersDatabase _boostersDatabase;

    [SerializeField] private AnimationCurve _rarityCurve;

    private List<GlobalModifierData> _activeBoosters;
    private List<float> _boosterDurations;

    private Dictionary<ModuleTypes, Modifiers> _moduleBoosterModifiers = new Dictionary<ModuleTypes, Modifiers>();

    private void Awake()
    {
        
        _activeBoosters = new List<GlobalModifierData>();
        
        _boosterDurations = new List<float>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        for (int i = 0; i < _boosterDurations.Count; i++)
        {
            float duration = _boosterDurations[i];
            if(duration <= 0)
            {
                RemoveBooster(i);
            }
            else
            {
                _boosterDurations[i] -= Time.deltaTime;
            }
        }
    }

    public GlobalModifierData GetWeightedBooster()
    {
        //Debug.Log("GetWeightedBooster - Selecting a weighted booster...");
        float totalWeight = 0f;
        foreach (var booster in _boostersDatabase.Boosters)
        {
            totalWeight += GetBoosterWeight(booster);
        }
        Debug.Log("GetWeightedBooster - Total Weight: " + totalWeight);
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (var booster in _boostersDatabase.Boosters)
        {
            cumulativeWeight += GetBoosterWeight(booster);
            if (randomValue <= cumulativeWeight)
            {
                //Debug.Log("GetWeightedBooster - Selected Booster: " + booster.Booster.name);
                return booster.Booster;
            }
        }
        //Debug.LogWarning("GetWeightedBooster - No booster selected, returning null.");
        return null;
    }

    public float GetBoosterWeight(BoosterData booster)
    {
        float weight = _rarityCurve.Evaluate((float)booster.Tier);
        return weight;
    }

    public void AddBooster(GlobalModifierData booster)
    {
        AddBoosterModifiers(booster);
        _activeBoosters.Add(booster);
        _boosterDurations.Add(booster.Duration);


        Debug.Log("AddBooster - Booster added!");
        OnBoostActivated?.Invoke(this, EventArgs.Empty);
    }


    public void AddBoosterModifiers(GlobalModifierData booster)
    {
        //Debug.Log("ApplyBoosterModifiers - Booster modifiers applied!");
        if (booster.IsPerModule)
        {
            foreach (var moduleType in booster.AffectedModules)
            {
                if (_moduleBoosterModifiers.ContainsKey(moduleType))
                {
                    SetBoosterModifiers(moduleType, _moduleBoosterModifiers[moduleType] + booster.Modifiers);
                }
                else
                {
                    SetBoosterModifiers(moduleType, booster.Modifiers);
                }
            }
        }
        else
        {
            foreach (var moduleType in _moduleBoosterModifiers.Keys)
            {
                if(moduleType == ModuleTypes.Barn)
                {
                    continue;
                }
                SetBoosterModifiers(moduleType, _moduleBoosterModifiers[moduleType] + booster.Modifiers);
            }
        }
    }

    public void RemoveBoosterModifiers(GlobalModifierData booster)
    {
        //Debug.Log("RemoveBoosterModifiers - Booster modifiers removed!");
        if (booster.IsPerModule)
        {
            foreach (var moduleType in booster.AffectedModules)
            {
                if (_moduleBoosterModifiers.ContainsKey(moduleType))
                {
                    SetBoosterModifiers(moduleType, _moduleBoosterModifiers[moduleType] - booster.Modifiers);
                }
            }
        }
        else
        {
            foreach (var moduleType in _moduleBoosterModifiers.Keys)
            {
                if(moduleType == ModuleTypes.Barn)
                {
                    continue;
                }
                SetBoosterModifiers(moduleType, _moduleBoosterModifiers[moduleType] - booster.Modifiers);
            }
        }
    }

    public void RemoveBooster(int index)
    {
        RemoveBoosterModifiers(_activeBoosters[index]);

        _boosterDurations.RemoveAt(index);
        _activeBoosters.RemoveAt(index);

        Debug.Log("RemoveBooster - Booster expired and removed!");
        OnBoostDeactivated?.Invoke(this, EventArgs.Empty);
    }


    public void SetBoosterModifiers(ModuleTypes moduleType, Modifiers modifiers)
    {
        _moduleBoosterModifiers[moduleType] = modifiers;

        //Debug.Log("SetBoosterModifiers - set " + moduleType + " to:" + _moduleBoosterModifiers[moduleType].ToString());

        ServiceLocator.Instance.ModulesManager.CalculateModuleTypeProduction(moduleType);
    }

    public BoosterTiers GetBoosterTier(string boosterName)
    {
        var booster = _boostersDatabase.Boosters.Find(b => b.Booster.Name == boosterName);
        //Debug.Log("GetBoosterTier - Booster " + boosterName + " is of tier " + booster.Tier);
        return booster.Tier;
    }

    public int GetBoosterCost(string boosterName)
    {
        var booster = _boostersDatabase.Boosters.Find(b => b.Booster.Name == boosterName);
        //Debug.Log("GetBoosterCost - Booster " + boosterName + " costs " + booster.Cost);
        return booster.Cost;
    }
}
