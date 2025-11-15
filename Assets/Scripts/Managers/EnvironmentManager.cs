using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject grassTiles;
    [SerializeField] private GameObject clouds;
    private List<Animator> _grassAnimators;
    private Animator _cloudsAnimator;

    private void Start()
    {
        // Assuming grassTiles is a GameObject
        _grassAnimators = new List<Animator>();

        // Get all Animator components in children (including inactive ones if you want)
        Animator[] animators = grassTiles.GetComponentsInChildren<Animator>();

        foreach (var animator in animators)
        {
            _grassAnimators.Add(animator);
        }

        _cloudsAnimator = clouds.GetComponent<Animator>();
    }

    private void Update()
    {
        foreach (Animator grassAnimator in _grassAnimators)
        {
            grassAnimator.SetFloat("SoilPollution", ServiceLocator.Instance.PollutionManager.SoilPollutionLevel);
            Debug.Log("EnvironmentManager - Update: Setting SoilPollution to " + ServiceLocator.Instance.PollutionManager.SoilPollutionLevel);
            _cloudsAnimator.SetFloat("AirPollution", ServiceLocator.Instance.PollutionManager.AirPollutionLevel);
        }
    }
}
