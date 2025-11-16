using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TMPro;

public class ModuleBehaviour : MonoBehaviour
{
    public ModuleData ModuleData => _moduleData;
    [SerializeField] private ModuleData _moduleData;
    [SerializeField] private Image _collectionBackgroundIcon;
    [SerializeField] private Image _collectionFillIcon;
    [SerializeField] private RectTransform _maskTransform;
    [SerializeField] private List<UpgradeObjectEntry> _upgradeObjects;

    [SerializeField] private AudioClip _moduleAudio;
    [SerializeField] private AudioClip _collectAudio;

    [SerializeField] private TMP_Text _productionText;
    [SerializeField] private TMP_Text _airPollutionText;
    [SerializeField] private TMP_Text _soilPollutionText;
    [SerializeField] private TMP_Text _waterPollutionText;

    [SerializeField] private GameObject _stats;

    private float _collectionTimeRemaining;

    private float _moneyCollected;

    private ModulesManager _modulesManager;
    private Modifiers _production;

    private bool _collecting;
    private bool _isPlaced;

    private Coroutine _flashCoroutine;

    private Color _positiveColor;
    private Color _negativeColor;

    private void Start()
    {
        _modulesManager = ServiceLocator.Instance.ModulesManager;
        _collectionFillIcon.fillAmount = 0f;
        _collecting = false;
        _stats.SetActive(false);
        _isPlaced = false;
        _positiveColor = ServiceLocator.Instance.ShopUIManager.PositiveModifierColor;
        _negativeColor = ServiceLocator.Instance.ShopUIManager.NegativeModifierColor;
    }

    private void Update()
    {
        if (ServiceLocator.Instance.TutorialManager.InTutorial)
            return;

        if (!_isPlaced)
            return;

        // always get current production (to reflect upgrades)
        _production = _modulesManager.ModuleTypeProductions[_moduleData.ModuleType];

        // compute income for this frame
        Modifiers production = _production * Time.deltaTime;

        UpdateHUDStats(_production);

        ApplyPollution(production);

        if (!_collecting)
            return;

        //Debug.Log("ModuleBehaviour - Update: Collecting, time remaining: " + _collectionTimeRemaining);

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

        _flashCoroutine = StartCoroutine(Flash(_collectionFillIcon.rectTransform, _maskTransform));

        if (!ServiceLocator.Instance.TutorialManager.TutorialContinued)
        {
            ServiceLocator.Instance.TutorialManager.TutorialContinued = true;
            ServiceLocator.Instance.TutorialManager.ContinueTutorial();
        }
    }

    public void ApplyIncome(Modifiers production)
    {
        float actualIncome = production.IncomeModifier * _moduleData.BaseProduction.SpeedModifier / _production.SpeedModifier;
        _moneyCollected += actualIncome;
        //Debug.Log("ModuleBehaviour - ApplyIncome: Applying income: " + actualIncome + ", total collected: " + _moneyCollected);
        SetProductionIconFill(_moneyCollected, production.IncomeModifier * _moduleData.BaseProduction.SpeedModifier / Time.deltaTime);
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
            _collectionTimeRemaining = _production.SpeedModifier;
            _moneyCollected = 0f;
            if(ServiceLocator.Instance.TutorialManager.InTutorial)
            {
                ServiceLocator.Instance.TutorialManager.EndTutorial();
            }
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                _collectionFillIcon.rectTransform.localScale = Vector3.one;
            }
        }
    }

    public void SetProductionIconFill(float value, float totalProduction)
    {
        //Debug.Log("ModuleBehaviour - SetProductionIconFill: Setting production icon fill, value: " + value + ", totalProduction: " + totalProduction);
        if (_collectionFillIcon)
        {
            //Debug.Log("ModuleBehaviour - SetProductionIconFill: time left:" + (_production.SpeedModifier - _collectionTimeRemaining) + ", totalProduction: " + totalProduction + "value/total production:" + (value * 100) / totalProduction);
            float fillAmount = value / totalProduction - .1f;
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
        //Debug.Log("ModuleBehaviour - Place: Module placed.");
        if (_moduleData.BaseProduction.SpeedModifier != 0)
        {
            if (!_modulesManager.ModuleTypeProductions.ContainsKey(_moduleData.ModuleType))
            {
                ServiceLocator.Instance.ModulesManager.CalculateModuleTypeProduction(_moduleData.ModuleType);
            }
            _production = _modulesManager.ModuleTypeProductions[_moduleData.ModuleType];
            _collectionTimeRemaining = _production.SpeedModifier;
            _collecting = true;
        }

        _stats.SetActive(true);
        _isPlaced = true;
    }

    public void UpdateVisuals(UpgradePhases upgradePhase)
    {
        foreach (var uo in _upgradeObjects)
        {
            uo.Object.SetActive(false);
        }
        _upgradeObjects.Where(uo => uo.UpgradePhase == upgradePhase).First().Object.SetActive(true);
    }

    IEnumerator Flash(RectTransform spriteTransform, RectTransform maskTransform)
    {
        while (true)
        {
            //spriteTransform.localScale = Vector3.one * 1.1f;
            maskTransform.localScale = Vector3.one * 1.1f;
            yield return new WaitForSeconds(0.5f);
            //spriteTransform.localScale = Vector3.one;
            maskTransform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateHUDStats(Modifiers production)
    {
        if (_moduleData.ModuleType == ModuleTypes.Barn)
        {
            _productionText.text = $"{production.IncomeModifier:F2}%";
            _airPollutionText.text = $"{production.AirPollutionModifier / 10:F2}%";
            _soilPollutionText.text = $"{production.SoilPollutionModifier / 10:F2}%";
            _waterPollutionText.text = $"{production.WaterPollutionModifier / 10:F2}%";
        }
        else
        {
            _productionText.text = $"{production.IncomeModifier:F2}/s";
            _airPollutionText.text = $"{production.AirPollutionModifier / 10:F2}/s";
            _soilPollutionText.text = $"{production.SoilPollutionModifier / 10:F2}/s";
            _waterPollutionText.text = $"{production.WaterPollutionModifier / 10:F2}/s";
        }

        _airPollutionText.color = production.AirPollutionModifier >= 0 ? _negativeColor : _positiveColor;
        _soilPollutionText.color = production.SoilPollutionModifier >= 0 ? _negativeColor : _positiveColor;
        _waterPollutionText.color = production.WaterPollutionModifier >= 0 ? _negativeColor : _positiveColor;
    }
}