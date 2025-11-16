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

    // Store all active booster icons
    private List<BoosterIconUI> activeBoosterIcons = new List<BoosterIconUI>();

    private void Start()
    {
        _moneyManager = ServiceLocator.Instance.MoneyManager;
        _pollutionManager = ServiceLocator.Instance.PollutionManager;
        _shopManager = ServiceLocator.Instance.ShopManager;
        _shopUIManager = ServiceLocator.Instance.ShopUIManager;

        shopButton.onClick.AddListener(OpenShop);

        boostTemplate.gameObject.SetActive(false);
        ServiceLocator.Instance.BoostersManager.OnBoostActivated += BoostersManager_OnBoostChanged;
        ServiceLocator.Instance.BoostersManager.OnBoostDeactivated += BoostersManager_OnBoostChanged;

        UpdateBoostsVisual();
    }

    private void BoostersManager_OnBoostChanged(object sender, System.EventArgs e)
    {
        UpdateBoostsVisual();
    }

    private void UpdateBoostsVisual()
{
    var manager = ServiceLocator.Instance.BoostersManager;
    var activeBoosters = manager.ActiveBoosters;
    var durations = manager.BoosterDurations;

    // Count stacks for each modifier
    Dictionary<GlobalModifierData, int> stackCounts = new Dictionary<GlobalModifierData, int>();
    foreach (var mod in activeBoosters)
    {
        if (stackCounts.ContainsKey(mod))
            stackCounts[mod]++;
        else
            stackCounts[mod] = 1;
    }

    // Remove icons no longer active
    for (int i = activeBoosterIcons.Count - 1; i >= 0; i--)
    {
        if (!activeBoosters.Contains(activeBoosterIcons[i].Modifier))
        {
            Destroy(activeBoosterIcons[i].gameObject);
            activeBoosterIcons.RemoveAt(i);
        }
    }

    // Add new icons or update stack
    for (int i = 0; i < activeBoosters.Count; i++)
    {
        var mod = activeBoosters[i];
        bool exists = activeBoosterIcons.Exists(icon => icon.Modifier == mod);
        if (!exists)
        {
            Transform iconObj = Instantiate(boostTemplate, boostsContainer);
            iconObj.gameObject.SetActive(true);

            BoosterIconUI iconUI = iconObj.GetComponent<BoosterIconUI>();
            if (iconUI != null)
            {
                iconUI.Initialize(mod, durations[i], stackCounts[mod]);
                activeBoosterIcons.Add(iconUI);
            }
        }
        else
        {
            // Update stack if already exists
            var iconUI = activeBoosterIcons.Find(icon => icon.Modifier == mod);
            iconUI?.SetStack(stackCounts[mod]);
        }
    }
}






    private void UpdateBoosterTimers()
{
    var manager = ServiceLocator.Instance.BoostersManager;

    for (int i = 0; i < manager.ActiveBoosters.Count; i++)
    {
        float remaining = manager.BoosterDurations[i];
        if (i < activeBoosterIcons.Count)
        {
            activeBoosterIcons[i].UpdateTimer(remaining);
        }
    }
}


    private void OpenShop()
    {
        _shopUIManager.ShowShop();
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

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = Mathf.RoundToInt(_moneyManager.CurrentMoney).ToString();
    }

    private void UpdatePollutionBars()
    {
        if (airSlider != null)
            airSlider.value = 1 - _pollutionManager.AirPollutionLevel / 100f;

        if (soilSlider != null)
            soilSlider.value = 1 - _pollutionManager.SoilPollutionLevel / 100f;

        if (waterSlider != null)
            waterSlider.value = 1 - _pollutionManager.WaterPollutionLevel / 100f;
    }
}
