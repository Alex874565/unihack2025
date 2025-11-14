using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModulesGrid : MonoBehaviour
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private float _moduleSizeX;
    [SerializeField] private float _moduleSizeY;

    private List<List<GameObject>> _grid;

    private void Awake()
    {
        _grid = new List<List<GameObject>>(_rows);
        for (int i = 0; i < _rows; i++)
        {
            _grid.Add(new List<GameObject>(_columns));
        }
    }

    public void PlaceModule(GameObject module, int x, int y)
    {
        _grid[y][x] = module;
    }

    public bool AreSpacesLeft()
    {
        return _grid.Any(t => t.Any());
    }

    public bool IsSpaceFree(int x, int y)
    {
        return _grid[y][x] == null;
    }

    public void CommandModules(IModuleCommand command)
    {
        foreach (var row in _grid)
        {
            foreach (var module in row)
            {
                if (module != null)
                {
                    command.Execute(module.GetComponent<ModuleController>());
                }
            }
        }
    }
}
