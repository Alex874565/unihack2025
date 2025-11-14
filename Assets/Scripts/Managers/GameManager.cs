using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    public ModulesManager ModulesManager => _modulesManager;
    public UpgradesManager UpgradesManager => _upgradesManager;
    public ShopManager ShopManager => _shopManager;

    private static GameManager _instance;

    private ModulesManager _modulesManager;
    private UpgradesManager _upgradesManager;
    private ShopManager _shopManager;

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
    }

}
