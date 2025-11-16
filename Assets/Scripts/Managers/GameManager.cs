using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        if(ServiceLocator.Instance.PollutionManager.GetTotalPollution() < 0.1 && ServiceLocator.Instance.MoneyManager.CurrentMoney >= 1000000)
        {
            Debug.Log("You Win!");
        }else
        {
            if(ServiceLocator.Instance.PollutionManager.AirPollutionLevel > 99 || ServiceLocator.Instance.PollutionManager.SoilPollutionLevel > 99 || ServiceLocator.Instance.PollutionManager.WaterPollutionLevel > 99)
            {
                Debug.Log("You Lose!");
            }
        }
    }

}
