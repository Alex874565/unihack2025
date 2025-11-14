using UnityEngine;

public class ModuleBehaviour : MonoBehaviour
{
    public ModuleData ModuleData => _moduleData;
    [SerializeField] private ModuleData _moduleData;
    
    private GameObject _gameObject;

    private void Awake()
    {
        _gameObject = this.gameObject;
    }

    private void Update()
    {
        
    }
}
