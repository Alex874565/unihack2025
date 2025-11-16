using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject grassTiles;
    [SerializeField] private GameObject clouds;
    [SerializeField] private GameObject waterTiles;
    private List<Animator> _grassAnimators;
    private Animator _cloudsAnimator;
    private List<Animator> _waterAnimators;

    private void Start()
    {
        // Assuming grassTiles is a GameObject
        _grassAnimators = new List<Animator>();
        _waterAnimators = new List<Animator>();

        // Get all Animator components in children (including inactive ones if you want)
        Animator[] animators = grassTiles.GetComponentsInChildren<Animator>();

        foreach (var animator in animators)
        {
            _grassAnimators.Add(animator);
        }

        _cloudsAnimator = clouds.GetComponent<Animator>();

        Animator[] waterAnimators = waterTiles.GetComponentsInChildren<Animator>();
        foreach (var animator in waterAnimators)
        {
            _waterAnimators.Add(animator);
        }

        Debug.Log("Clouds Animator Controller: " + _cloudsAnimator.runtimeAnimatorController);

        foreach (var a in _grassAnimators)
            Debug.Log("Grass Controller: " + a.runtimeAnimatorController);

        foreach (var a in _waterAnimators)
            Debug.Log("Water Controller: " + a.runtimeAnimatorController);

    }

private void Update()
    {
        foreach (Animator grassAnimator in _grassAnimators)
        {
            if (grassAnimator != null)
                grassAnimator.SetFloat("SoilPollution", ServiceLocator.Instance.PollutionManager.SoilPollutionLevel);
        }


        _cloudsAnimator.SetFloat("AirPollution", ServiceLocator.Instance.PollutionManager.AirPollutionLevel);

        foreach (Animator waterAnimator in _waterAnimators)
        {
            if (waterAnimator != null)
                waterAnimator.SetFloat("WaterPollution", ServiceLocator.Instance.PollutionManager.WaterPollutionLevel);
        }
    }
}
