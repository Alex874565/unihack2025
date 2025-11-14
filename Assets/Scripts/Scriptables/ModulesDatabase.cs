using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ModulesDatabase", menuName = "ScriptableObjects/ModulesDatabase", order = 3)]
public class ModulesDatabase : ScriptableObject
{
    public List<DbModule> Modules => _modules;
    [SerializeField] private List<DbModule> _modules;

    [System.Serializable]
    public class DbModule
    {
        public ModuleData ModuleData => _moduleData;
        public GameObject ModulePrefab => _modulePrefab;

        [SerializeField] private ModuleData _moduleData;
        [SerializeField] private GameObject _modulePrefab;
    }
}
