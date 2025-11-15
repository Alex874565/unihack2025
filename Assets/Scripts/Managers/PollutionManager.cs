using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    public float AirPollutionLevel => _airPollutionLevel;
    public float SoilPollutionLevel => _soilPollutionLevel;
    public float WaterPollutionLevel => _waterPollutionLevel;
    public float TotalPollutionLevel => _totalPollutionLevel;

    private float _airPollutionLevel;
    [SerializeField] private float _soilPollutionLevel;
    private float _waterPollutionLevel;
    private float _totalPollutionLevel;

    public void ModifyAirPollution(float amount)
    {
        _airPollutionLevel += amount;
        UpdateTotalPollution();
    }
    public void ModifySoilPollution(float amount)
    {
        _soilPollutionLevel += amount;
        UpdateTotalPollution();
    }
    public void ModifyWaterPollution(float amount)
    {
        _waterPollutionLevel += amount;
        UpdateTotalPollution();
    }

    private void UpdateTotalPollution()
    {
        _totalPollutionLevel = _airPollutionLevel + _soilPollutionLevel + _waterPollutionLevel;
    }

}
