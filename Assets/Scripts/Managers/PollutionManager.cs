using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    public float AirPollutionLevel => _airPollutionLevel;
    public float SoilPollutionLevel => _soilPollutionLevel;
    public float WaterPollutionLevel => _waterPollutionLevel;

    private float _airPollutionLevel;
    [SerializeField] private float _soilPollutionLevel;
    private float _waterPollutionLevel;

    public void ModifyAirPollution(float amount)
    {
        _airPollutionLevel += amount / 5;
        //Debug.Log("AirPollutionLevel - " + _airPollutionLevel);
    }
    public void ModifySoilPollution(float amount)
    {
        _soilPollutionLevel += amount / 5;
        //Debug.Log("SoilPollutionLevel - " + _soilPollutionLevel);
    }
    public void ModifyWaterPollution(float amount)
    {
        _waterPollutionLevel += amount / 5;
        //Debug.Log("WaterPollutionLevel - " + _waterPollutionLevel);
    }

    public float GetTotalPollution()
    {
        return _airPollutionLevel + _soilPollutionLevel + _waterPollutionLevel;
    }

}
