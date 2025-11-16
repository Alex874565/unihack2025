using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> TutorialSteps;
    public bool IsPlacingModules => _isPlacingModules;
    public bool InTutorial => _inTutorial;
    public bool TutorialContinued;

    [System.Serializable]
    public class TutorialStep
    {
        public string TextToShow;
        public List<GameObject> ObjectsToShow;
        public List<GameObject> ObjectsToHide;
    }

    public bool _isPlacingModules;

    private int _tutorialStep;
    private int _totalTutorialSteps;

    private bool _inTutorial;
    private bool _continuingTutorial;

    [SerializeField] private Button _shopItemButton;

    void Start()
    {
        _totalTutorialSteps = TutorialSteps.Count;;
        _inTutorial = true;
        _continuingTutorial = false;
        //_shopItemButton.enabled = false;
        TutorialContinued = false;
        ChangeTutorialStep(0);
    }

    public enum TutorialState
    {
        Normal,
        InterruptWarning,
        InterruptApproval
    }

    private TutorialState _state = TutorialState.Normal;

    private void Update()
    {
        if(_continuingTutorial)
        {
            return;
        }
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (!_inTutorial) return;
        if (_isPlacingModules) return;

        var typewriter = ServiceLocator.Instance.DialogueManager.Typewriter;

        // --- CASE 1: Player interrupts typing ---
        if (typewriter.IsTyping)
        {
            HandleInterrupt(typewriter);
            return;
        }

        // --- CASE 2: Player presses space AFTER text finished ---
        OnLineFinished();
    }

    private void HandleInterrupt(Typewriter typewriter)
    {
        switch (_state)
        {
            case TutorialState.Normal:
                // First interruption → ANGRY
                _state = TutorialState.InterruptWarning;
                typewriter.StopCurrentCoroutine();
                ServiceLocator.Instance.DialogueManager.StartTyping("DON'T INTERRUPT ME!");
                return;

            case TutorialState.InterruptWarning:
                // Second interruption → APPROVAL
                _state = TutorialState.InterruptApproval;
                typewriter.StopCurrentCoroutine();
                ServiceLocator.Instance.DialogueManager.StartTyping("Okay, fine, you may proceed, goldfish...");
                return;

            case TutorialState.InterruptApproval:
                // After approval → just skip typing
                typewriter.StopCurrentCoroutine();
                ChangeTutorialStep(_tutorialStep + 1);
                return;
        }
    }

    private void OnLineFinished()
    {
        if (_state == TutorialState.InterruptWarning)
        {
            // Reset interrupt state when line ends
            _state = TutorialState.Normal;
        }

        Debug.Log("Advancing tutorial step from " + _tutorialStep);
        ChangeTutorialStep(_tutorialStep + 1);
    }


    private void ShowTutorialStep(int tutorialStep)
    {
        ServiceLocator.Instance.DialogueManager.StartTyping(TutorialSteps[tutorialStep].TextToShow);
        foreach (var obj in TutorialSteps[tutorialStep].ObjectsToShow)
        {
            obj.SetActive(true);
        }
        foreach (var obj in TutorialSteps[tutorialStep].ObjectsToHide)
        {
            obj.SetActive(false);
        }
    }

    private void ChangeTutorialStep(int step)
    {
        _tutorialStep = step;

        Debug.Log("Changing tutorial step to: " + step);

        // End tutorial if past last step
        if (_tutorialStep > _totalTutorialSteps)
        {
            EndTutorial();
            return;
        }

        // Execute final special step if needed (your Shop logic)
        if (_tutorialStep == _totalTutorialSteps - 1)
        {
            ServiceLocator.Instance.ShopManager.SelectNextItems();
            ServiceLocator.Instance.ShopUIManager.ShowShop();
            EndTutorial();
            return;
        }

        // Normal step
        ShowTutorialStep(_tutorialStep);
    }


    public void EndTutorial()
    {
        _inTutorial = false;
        ServiceLocator.Instance.DialogueManager.HideDialogue();
        Debug.Log("TutorialManager - EndTutorial: Tutorial ended.");
        Time.timeScale = 1f;
        //_shopItemButton.enabled = true;
    }

    public void ContinueTutorial()
    {
        _continuingTutorial = true;
        Debug.Log("TutorialManager - ContinueTutorial: Continuing tutorial at step " + _tutorialStep + " of " + _totalTutorialSteps);
        _inTutorial = true;
        ServiceLocator.Instance.DialogueManager.ShowDialogue();
        ShowTutorialStep(_tutorialStep);
    }

    public void StartPlacingModules()
    {
        _isPlacingModules = true;
    }

    public void StopPlacingModules()
    {
        _isPlacingModules = false;
        ChangeTutorialStep(_tutorialStep + 1);
    }
}
