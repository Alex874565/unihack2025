using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance => _instance;
    private static ServiceLocator _instance;

    public ModulesManager ModulesManager => _modulesManager;
    public UpgradesManager UpgradesManager => _upgradesManager;
    public ShopManager ShopManager => _shopManager;
    public BoostersManager BoostersManager => _boostersManager;

    private ModulesManager _modulesManager;
    private UpgradesManager _upgradesManager;
    private ShopManager _shopManager;
    private BoostersManager _boostersManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        _modulesManager = GetComponent<ModulesManager>();
        _upgradesManager = GetComponent<UpgradesManager>();
        _shopManager = GetComponent<ShopManager>();
        _boostersManager = GetComponent<BoostersManager>();
    }
}
