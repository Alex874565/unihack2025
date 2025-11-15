using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialDatabase tutorialDatabase;
    [SerializeField] private TMP_Text _dialogueText;

    private int _tutorialStep;
    private int _totalTutorialSteps;

    void Start()
    {
        _tutorialStep = 0;
        _totalTutorialSteps = tutorialDatabase.TutorialSteps.Count;
    }

    private void ShowTutorialStep()
    {
        _dialogueText.text = tutorialDatabase.TutorialSteps[_tutorialStep].TextToShow;
        foreach (var obj in tutorialDatabase.TutorialSteps[_tutorialStep].ObjectsToShow)
        {
            obj.SetActive(true);
        }
        foreach (var obj in tutorialDatabase.TutorialSteps[_tutorialStep].ObjectsToHide)
        {
            obj.SetActive(false);
        }
    }

    private void ChangeTutorialStep()
    {
        if(_tutorialStep < _totalTutorialSteps - 1)
        {
            _tutorialStep++;
            ShowTutorialStep();
        }
    }
}
