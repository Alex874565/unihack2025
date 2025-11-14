using System.Collections.Generic;
using UnityEngine;

public class ModulePlacer : MonoBehaviour
{
    [SerializeField] private ModulesGrid _modulesGrid;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Color _occupiedColor;
    [SerializeField] private Color _freeColor;
    [SerializeField] private Color _hoveringColor;
    

    private float _cellWidth;
    private float _cellHeight;
    private float _totalWidth;
    private float _totalHeight;

    private List<List<GameObject>> _gridCells;
    private int _lastMouseRow;
    private int _lastMouseCol;

    private GameObject _modulePrefab;
    private ModuleData _moduleData;

    private void Start()
    {
        _lastMouseCol = -1;
        _lastMouseRow = -1;
        _gridCells = new List<List<GameObject>>();
        _totalWidth = _maxX - _minX;
        _totalHeight = _maxY - _minY;
        _cellWidth = _totalWidth / _modulesGrid.Columns;
        _cellHeight = _totalHeight / _modulesGrid.Rows;
    }

    public void StartPlacingModule(ModuleData moduleData)
    {
        _moduleData = moduleData;
        _modulePrefab = Instantiate(_moduleData.ModulePrefab);
        _modulePrefab.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        DrawGrid();
    }
    private void Update()
    {
        if (_modulePrefab != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0f;

            // Clamp to grid bounds
            worldMousePosition.x = Mathf.Clamp(worldMousePosition.x, _minX, _maxX - 0.001f);
            worldMousePosition.y = Mathf.Clamp(worldMousePosition.y, _minY, _maxY - 0.001f);

            // Convert to column/row
            int mouseColumn = (int)((worldMousePosition.x - _minX) / _cellWidth);
            int mouseRow = (int)((worldMousePosition.y - _minY) / _cellHeight);
            if(_lastMouseCol != mouseColumn || _lastMouseRow != mouseRow)
            {
                if (_lastMouseCol != -1 && _lastMouseRow != -1)
                {
                    if (_modulesGrid.IsSpaceFree(_lastMouseRow, _lastMouseCol))
                    {
                        _gridCells[_lastMouseRow][_lastMouseCol].GetComponent<SpriteRenderer>().color = _freeColor;
                    }
                    else
                    {
                        _gridCells[_lastMouseRow][_lastMouseCol].GetComponent<SpriteRenderer>().color = _occupiedColor;
                    }
                }
                _lastMouseCol = mouseColumn;
                _lastMouseRow = mouseRow;
            }
            float moduleX = _minX + mouseColumn * _cellWidth + _cellWidth / 2;
            float moduleY = _minY + mouseRow * _cellHeight + +_cellHeight / 2;
            _modulePrefab.transform.position = new Vector3(moduleX, moduleY, 0);
            if (_modulesGrid.IsSpaceFree(mouseRow, mouseColumn))
            {
                _gridCells[mouseRow][mouseColumn].GetComponent<SpriteRenderer>().color = _hoveringColor;
            }
            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceCurrentModule(mouseRow, mouseColumn);
            }
        }
    }
    private void TryPlaceCurrentModule(int x, int y)
    {
        if (_modulesGrid.IsSpaceFree(x, y))
        {
            foreach (var row in _gridCells)
            {
                foreach (var cell in row)
                {
                    cell.SetActive(false);
                }
            }
            _modulesGrid.AddModuleToGrid(_moduleData, x, y);
            _modulePrefab.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            _modulePrefab = null;
            _moduleData = null;
            
        }
    }

    private void DrawGrid()
    {
        if (_gridCells.Count == 0)
        {
            for (int i = 0; i < _modulesGrid.Rows; i++)
            {
                List<GameObject> row = new List<GameObject>();
                for (int j = 0; j < _modulesGrid.Columns; j++)
                {
                    Vector3 cellPosition = new Vector3(_minX + j * _cellWidth + _cellWidth / 2, _minY + i * _cellHeight + _cellHeight / 2, 0);
                    GameObject cell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);
                    SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                    Vector2 spriteSize = sr.sprite.bounds.size; // in world units
                    float scaleX = _cellWidth / spriteSize.x;
                    float scaleY = _cellHeight / spriteSize.y;
                    cell.transform.localScale = new Vector3(scaleX - .1f, scaleY - .1f, 1f);

                    if (_modulesGrid.IsSpaceFree(i, j))
                    {
                        cell.GetComponent<SpriteRenderer>().color = _freeColor;
                    }
                    else
                    {
                        cell.GetComponent<SpriteRenderer>().color = _occupiedColor;
                    }
                    row.Add(cell);
                }
                _gridCells.Add(row);
            }
        }
        else
        {
            for (int i = 0; i < _modulesGrid.Rows; i++)
            {
                for (int j = 0; j < _modulesGrid.Columns; j++)
                {
                    _gridCells[i][j].SetActive(true);
                    if (_modulesGrid.IsSpaceFree(i, j))
                    {
                        _gridCells[i][j].GetComponent<SpriteRenderer>().color = _freeColor;
                    }
                    else
                    {
                        _gridCells[i][j].GetComponent<SpriteRenderer>().color = _occupiedColor;
                    }
                }
            }
        }
    }
}
