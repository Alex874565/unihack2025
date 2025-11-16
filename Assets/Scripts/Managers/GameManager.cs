using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _airLoseUI;
    [SerializeField] private GameObject _soilLoseUI;
    [SerializeField] private GameObject _waterLoseUI;

    [SerializeField] private List<Slider> _waterSliders;
    [SerializeField] private List<Slider> _soilSliders;
    [SerializeField] private List<Slider> _airSliders;

    [SerializeField] private AudioClip _loseMusic;

    private bool _gameOver;

    private void Start()
    {
        _gameOver = false;
    }

    private void Update()
    {
        if (!_gameOver) {
            if (ServiceLocator.Instance.PollutionManager.GetTotalPollution() < 0.1 && ServiceLocator.Instance.MoneyManager.CurrentMoney >= 50000)
            {
                _gameOver = true;
                //Debug.Log("You Win!");
                Time.timeScale = 0f;
                foreach (var slider in _waterSliders)
                {
                    slider.value = 1 - ServiceLocator.Instance.PollutionManager.WaterPollutionLevel / 100;
                }
                foreach (var slider in _soilSliders)
                {
                    slider.value = 1 - ServiceLocator.Instance.PollutionManager.SoilPollutionLevel / 100;
                }
                foreach (var slider in _airSliders)
                {
                    slider.value = 1 - ServiceLocator.Instance.PollutionManager.AirPollutionLevel / 100;
                }
                _winUI.SetActive(true);
            }
            else 
            {
                if (ServiceLocator.Instance.PollutionManager.AirPollutionLevel > 09)
                {
                    _gameOver = true;
                    ServiceLocator.Instance.AudioManager.ChangeBackgroundMusic(_loseMusic);
                    //Debug.Log("You Lose!");
                    Time.timeScale = 0f;
                    foreach (var slider in _airSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.AirPollutionLevel / 100;
                    }
                    foreach (var slider in _soilSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.SoilPollutionLevel / 100;
                    }
                    foreach (var slider in _waterSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.WaterPollutionLevel / 100;
                    }
                    _airLoseUI.SetActive(true);
                }
                else if (ServiceLocator.Instance.PollutionManager.SoilPollutionLevel > 99)
                {
                    _gameOver = true;
                    ServiceLocator.Instance.AudioManager.ChangeBackgroundMusic(_loseMusic);
                    Time.timeScale = 0f;
                    foreach (var slider in _waterSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.WaterPollutionLevel / 100;
                    }
                    foreach (var slider in _soilSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.SoilPollutionLevel / 100;
                    }
                    foreach (var slider in _airSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.AirPollutionLevel / 100;
                    }
                    _soilLoseUI.SetActive(true);
                }
                else if (ServiceLocator.Instance.PollutionManager.WaterPollutionLevel > 99)
                {
                    _gameOver = true;
                    ServiceLocator.Instance.AudioManager.ChangeBackgroundMusic(_loseMusic);
                    Time.timeScale = 0f;
                    foreach (var slider in _waterSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.WaterPollutionLevel / 100;
                    }
                    foreach (var slider in _soilSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.SoilPollutionLevel / 100;
                    }
                    foreach (var slider in _airSliders)
                    {
                        slider.value = 1 - ServiceLocator.Instance.PollutionManager.AirPollutionLevel / 100;
                    }
                    _waterLoseUI.SetActive(true);
                }
            }
        }
    }

}
