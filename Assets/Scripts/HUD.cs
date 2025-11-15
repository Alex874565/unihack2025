using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Slider waterSlider;
    [SerializeField] private Slider soilSlider;
    [SerializeField] private Slider airSlider;
    [SerializeField] private Button shopButton;

    [SerializeField] private Transform boostsContainer;
    [SerializeField] private Transform boostTemplate;

    private MoneyManager _moneyManager;
    private PollutionManager _pollutionManager;
    private ShopManager _shopManager;
    private ShopUIManager _shopUIManager;

    private Dictionary<GlobalModifierData, BoosterIconUI> activeBoosterIcons 
    = new Dictionary<GlobalModifierData, BoosterIconUI>();


    private void Awake()
    {

    }

    private void Start()
    {
        _moneyManager = ServiceLocator.Instance.MoneyManager;
        _pollutionManager = ServiceLocator.Instance.PollutionManager;
        _shopManager = ServiceLocator.Instance.ShopManager;
        _shopUIManager = ServiceLocator.Instance.ShopUIManager;

        shopButton.onClick.AddListener(OpenShop);

        boostTemplate.gameObject.SetActive(false);
        ServiceLocator.Instance.BoostersManager.OnBoostActivated += BoostersManager_OnBoostActivated;
        ServiceLocator.Instance.BoostersManager.OnBoostDeactivated += BoostersManager_OnBoostDeactivated;

        UpdateBoostsVisual();
    }


    private void BoostersManager_OnBoostActivated(object sender, System.EventArgs e)
    {
        UpdateBoostsVisual();
    }

    private void BoostersManager_OnBoostDeactivated(object sender, System.EventArgs e)
    {
        UpdateBoostsVisual();
    }

    private void UpdateBoostsVisual()
    {
        // Delete previous icons safely
        List<Transform> toDestroy = new List<Transform>();
        foreach (Transform child in boostsContainer)
        {
            if (child != boostTemplate)
                toDestroy.Add(child);
        }
        foreach (Transform t in toDestroy)
            Destroy(t.gameObject);

        activeBoosterIcons.Clear();

        Debug.Log(ServiceLocator.Instance.BoostersManager.ActiveBoosters);
        // ← Replace the old foreach here with the for-loop below
        for (int i = 0; i < ServiceLocator.Instance.BoostersManager.ActiveBoosters.Count; i++)
        {
            var booster = ServiceLocator.Instance.BoostersManager.ActiveBoosters[i];
            float remaining = ServiceLocator.Instance.BoostersManager.BoosterDurations[i];

            Transform iconObj = Instantiate(boostTemplate, boostsContainer);
            iconObj.gameObject.SetActive(true);

            BoosterIconUI iconUI = iconObj.GetComponent<BoosterIconUI>();
            if (iconUI == null)
                Debug.LogError("BoosterIconUI MISSING on boostTemplate!", boostTemplate);

            if (booster == null)
                Debug.LogError("Active booster is NULL at index " + i);

            if (booster.Icon == null)
                Debug.LogError("Booster ICON is NULL for booster: " + booster.name);

            iconUI.Initialize(booster.Icon, booster.Duration);

            activeBoosterIcons.Add(booster, iconUI);
        }
    }





    private void OpenShop()
    {
        _shopUIManager.ShowShop();        // ShopUI retrieves items internally
    }


    private void Update()
    {
        UpdateMoneyUI();
        UpdatePollutionBars();
    }

    private void LateUpdate()
    {
        UpdateBoosterTimers();
    }

    private void UpdateBoosterTimers()
    {
        var manager =  ServiceLocator.Instance.BoostersManager;

        for (int i = 0; i < manager.ActiveBoosters.Count; i++)
        {
            GlobalModifierData booster = manager.ActiveBoosters[i];
            float remaining = manager.BoosterDurations[i];

            if (activeBoosterIcons.TryGetValue(booster, out BoosterIconUI iconUI))
            {
                iconUI.UpdateTimer(remaining);
            }
        }
    }


    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "" + Mathf.RoundToInt(_moneyManager.CurrentMoney);
    }

    private void UpdatePollutionBars()
    {
        if (airSlider != null)
            airSlider.value = _pollutionManager.AirPollutionLevel / 100;

        if (soilSlider != null)
            soilSlider.value = _pollutionManager.SoilPollutionLevel / 100;

        if (waterSlider != null)
            waterSlider.value = _pollutionManager.WaterPollutionLevel / 100;
    }
}
