using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ModulesDatabase", menuName = "ScriptableObjects/ModulesDatabase", order = 3)]
public class ModulesDatabase : ScriptableObject
{
    public List<ModuleData> Modules => _modules;
    [SerializeField] private List<ModuleData> _modules;
}
