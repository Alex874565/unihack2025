using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModulesGrid : MonoBehaviour
{
    public int Rows => _rows;
    public int Columns => _columns;
    public List<List<ModuleData>> Grid => _grid;

    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    
    private List<List<ModuleData>> _grid;

    private void Awake()
    {
        _grid = new List<List<ModuleData>>();

        for (int row = 0; row < _rows; row++)
        {
            var newRow = new List<ModuleData>();
            for (int col = 0; col < _columns; col++)
            {
                newRow.Add(null);
            }
            _grid.Add(newRow);
        }
    }

    public void AddModuleToGrid(ModuleData module, int x, int y)
    {
        _grid[x][y] = module;
    }

    public bool AreSpacesLeft()
    {
        return _grid.Any(t => t.Any());
    }

    public bool IsSpaceFree(int x, int y)
    {
        return _grid[x][y] == null;
    }
}
