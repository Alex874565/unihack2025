using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance => _instance;
    private static ServiceLocator _instance;

    public ModulesManager ModulesManager => _modulesManager;
    public UpgradesManager UpgradesManager => _upgradesManager;
    public ShopManager ShopManager => _shopManager;
    public BoostersManager BoostersManager => _boostersManager;
    public PollutionManager PollutionManager => _pollutionManager;
    public MoneyManager MoneyManager => _moneyManager;
    public EnvironmentManager EnvironmentManager => _environmentManager;
    public ShopUIManager ShopUIManager => _shopUIManager;
    public DialogueManager DialogueManager => _dialogueManager;
    private TutorialManager _tutorialManager;

    private ModulesManager _modulesManager;
    private UpgradesManager _upgradesManager;
    private ShopManager _shopManager;
    private BoostersManager _boostersManager;
    private PollutionManager _pollutionManager;
    private MoneyManager _moneyManager;
    private EnvironmentManager _environmentManager;
    private ShopUIManager _shopUIManager;
    private DialogueManager _dialogueManager;
    public TutorialManager TutorialManager => _tutorialManager;

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
        _pollutionManager = GetComponent<PollutionManager>();
        _moneyManager = GetComponent<MoneyManager>();
        _environmentManager = GetComponent<EnvironmentManager>();
        _shopUIManager = GetComponent<ShopUIManager>();
        _dialogueManager = GetComponent<DialogueManager>();
        _tutorialManager = GetComponent<TutorialManager>();
    }
}
