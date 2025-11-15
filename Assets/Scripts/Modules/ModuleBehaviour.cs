using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ModuleBehaviour : MonoBehaviour
{
    public ModuleData ModuleData => _moduleData;
    [SerializeField] private ModuleData _moduleData;
    [SerializeField] private Image _collectionBackgroundIcon;
    [SerializeField] private Image _collectionFillIcon;
    [SerializeField] private List<UpgradeObjectEntry> _upgradeObjects;

    private float _collectionTimeRemaining;

    private float _moneyCollected;

    private ModulesManager _modulesManager;
    private Modifiers _production;

    private bool _collecting;
    private bool _isPlaced;

    private void Start()
    {
        _modulesManager = ServiceLocator.Instance.ModulesManager;
        _collectionFillIcon.fillAmount = 0f;
        _collecting = false;
        _collectionBackgroundIcon.enabled = false;
        _isPlaced = false;
    }

    private void Update()
    {
        if (!_isPlaced)
            return;
        
        // always get current production (to reflect upgrades)
        _production = _modulesManager.ModuleTypeProductions[_moduleData.ModuleType];

        // compute income for this frame
        Modifiers production = _production * Time.deltaTime;

        ApplyPollution(production);

        if (!_collecting)
            return;
        
        if (_collectionTimeRemaining > 0f)
        {

            ApplyIncome(production);

            // subtract real time passed from remaining collection time
            _collectionTimeRemaining -= Time.deltaTime;       }
        else
        {
            FinishProduction();
        }
    }


    public void FinishProduction()
    {
        //Debug.Log("ModuleBehaviour - FinishProduction: Production cycle finished, produced: " + _moneyCollected);
        FillProductionIcon();
        _collecting = false;
    }

    public void ApplyIncome(Modifiers production)
    {
        float actualIncome = production.IncomeModifier * _moduleData.BaseProduction.SpeedModifier / _production.SpeedModifier;
        _moneyCollected += actualIncome;
        //Debug.Log("ModuleBehaviour - ApplyIncome: Applying income: " + actualIncome + ", total collected: " + _moneyCollected);

        SetProductionIconFill(_moneyCollected, production.IncomeModifier * _moduleData.BaseProduction.SpeedModifier);
    }

    public void ApplyPollution(Modifiers production)
    {
        //Debug.Log("ModuleBehaviour - ApplyPollution: Applying pollution - Air: " + production.AirPollutionModifier + ", Soil: " + production.SoilPollutionModifier + ", Water: " + production.WaterPollutionModifier);
        ServiceLocator.Instance.PollutionManager.ModifyAirPollution(production.AirPollutionModifier);
        ServiceLocator.Instance.PollutionManager.ModifySoilPollution(production.SoilPollutionModifier);
        ServiceLocator.Instance.PollutionManager.ModifyWaterPollution(production.WaterPollutionModifier);
    }

    public void TryCollect()
    {
        //Debug.Log("ModuleBehaviour - TryCollect: Trying to collect , collecting is: " + _collecting);
        if (!_collecting)
        {
            ServiceLocator.Instance.MoneyManager.GainMoney(_moneyCollected);
            _collecting = true;
            _production = _modulesManager.ModuleTypeProductions[_moduleData.ModuleType];
            _collectionTimeRemaining = _production.SpeedModifier;
            _moneyCollected = 0f;
        }
    }

    public void SetProductionIconFill(float value, float totalProduction)
    {
        if (_collectionFillIcon)
        {
            //Debug.Log("ModuleBehaviour - SetProductionIconFill: time left:" + (_production.SpeedModifier - _collectionTimeRemaining) + ", totalProduction: " + totalProduction + "value/total production:" + (value * 100) / totalProduction);
            float fillAmount = value / totalProduction;
            _collectionFillIcon.fillAmount = fillAmount;
        }
    }

    public void FillProductionIcon()
    {
        if (_collectionFillIcon)
        {
            _collectionFillIcon.fillAmount = 1f;
        }
    }

    public void Place()
    {
        Debug.Log("ModuleBehaviour - Place: Module placed.");
        _collectionBackgroundIcon.enabled = true;
        _production = _modulesManager.ModuleTypeProductions[_moduleData.ModuleType];
        _collectionTimeRemaining = _production.SpeedModifier;
        _collecting = true;
        _isPlaced = true;
    }

    public void ShowUpgrade(UpgradeData upgrade)
    {
        _upgradeObjects.Where(uo => uo.Upgrade.Name == upgrade.Name).First().Object.SetActive(true);
    }
}