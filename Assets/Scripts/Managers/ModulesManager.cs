using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModulesManager : MonoBehaviour
{
    public List<ModulesDatabase> OwnedModules => _ownedModules;
    [SerializeField] private ModulesDatabase _modulesDatabase;
    [SerializeField] private ModulesGrid _modulesGrid;
    [SerializeField] private List<ModulesDatabase> _ownedModules;

    public void AddOwnedModule(ModulesDatabase module)
    {
        _ownedModules.Add(module);
    }

    public ModuleData GetRandomModule()
    {
        var index = Random.Range(0, _modulesDatabase.Modules.Count);
        return _modulesDatabase.Modules[index].ModuleData;
    }
}
