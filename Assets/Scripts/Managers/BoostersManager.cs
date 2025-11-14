using UnityEngine;
using System.Collections.Generic;

public class BoostersManager : MonoBehaviour
{
    public List<GlobalModifierData> ActiveBoosters => _activeBoosters;
    public Dictionary<ModuleTypes, Modifiers> ModuleBoosterModifiers => _moduleBoosterModifiers;

    [SerializeField] private BoostersDatabase _boostersDatabase;

    [SerializeField] private AnimationCurve _rarityCurve;

    private List<GlobalModifierData> _activeBoosters;
    private List<float> _boosterDurations;

    private Dictionary<ModuleTypes, Modifiers> _moduleBoosterModifiers = new Dictionary<ModuleTypes, Modifiers>();

    private void Start()
    {
        _activeBoosters = new List<GlobalModifierData>();
        _boosterDurations = new List<float>();
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
        Debug.Log("GetWeightedBooster - Selecting a weighted booster...");
        float totalWeight = 0f;
        foreach (var booster in _boostersDatabase.Boosters)
        {
            totalWeight += GetBoosterWeight(booster);
        }
        Debug.Log("GetWeightedBooster - Total Weight: " + totalWeight);
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (var booster in _boostersDatabase.Boosters)
        {
            cumulativeWeight += GetBoosterWeight(booster);
            if (randomValue <= cumulativeWeight)
            {
                Debug.Log("GetWeightedBooster - Selected Booster: " + booster.Booster.name);
                return booster.Booster;
            }
        }
        Debug.LogWarning("GetWeightedBooster - No booster selected, returning null.");
        return null;
    }

    public float GetBoosterWeight(BoosterData booster)
    {
        float weight = _rarityCurve.Evaluate((float)booster.Tier);
        return weight;
    }

    public void AddBooster(GlobalModifierData booster)
    {
        ApplyBoosterModifiers(booster);
        _activeBoosters.Add(booster);
        _boosterDurations.Add(booster.Duration);
        Debug.Log("AddBooster - Booster added!");
    }

    public void ApplyBoosterModifiers(GlobalModifierData booster)
    {
        Debug.Log("ApplyBoosterModifiers - Booster modifiers applied!");
        if (booster.IsPerModule)
        {
            foreach (var moduleType in booster.AffectedModules)
            {
                if (_moduleBoosterModifiers.ContainsKey(moduleType))
                {
                    _moduleBoosterModifiers[moduleType] += booster.Modifiers;
                }
                else
                {
                    _moduleBoosterModifiers[moduleType] = booster.Modifiers;
                }

                Debug.Log(moduleType + ":" + _moduleBoosterModifiers[moduleType].ToString());
            }
        }
        else
        {
            foreach (var key in _moduleBoosterModifiers.Keys)
            {
                _moduleBoosterModifiers[key] += booster.Modifiers;
                Debug.Log("ApplyBoosterModifiers - " + key + ": " + _moduleBoosterModifiers[key].ToString());
            }
        }
    }

    public void RemoveBoosterModifiers(GlobalModifierData booster)
    {
        Debug.Log("RemoveBoosterModifiers - Booster modifiers removed!");
        if (booster.IsPerModule)
        {
            foreach (var moduleType in booster.AffectedModules)
            {
                if (_moduleBoosterModifiers.ContainsKey(moduleType))
                {
                    _moduleBoosterModifiers[moduleType] -= booster.Modifiers;
                }
                Debug.Log("RemoveBoosterModifiers - " + moduleType + ": " + _moduleBoosterModifiers[moduleType]);
            }
        }
        else
        {
            foreach (var key in _moduleBoosterModifiers.Keys)
            {
                _moduleBoosterModifiers[key] += booster.Modifiers;
                Debug.Log("RemoveBoosterModifiers  - " + key + ": " +  (_moduleBoosterModifiers[key].ToString()));
            }
        }
    }

    public void RemoveBooster(int index)
    {
        RemoveBoosterModifiers(_activeBoosters[index]);
        _boosterDurations.RemoveAt(index);
        _activeBoosters.RemoveAt(index);

        Debug.Log("RemoveBooster - Booster expired and removed!");
    }
}
