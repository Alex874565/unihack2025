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

    //attempt to get mobile phones input instead of space
    // --- New Helper Method ---
private bool IsInputPressed()
{
    // Check for PC/Editor input (Spacebar or Left Mouse Click)
    if (Application.platform == RuntimePlatform.WindowsPlayer || 
        Application.isEditor || 
        Application.platform == RuntimePlatform.OSXPlayer)
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }
    // Check for iOS/Android input (Any screen tap)
    else if (Application.platform == RuntimePlatform.IPhonePlayer ||
             Application.platform == RuntimePlatform.Android)
    {
        // Check for the start of a single touch on the screen
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
    
    // Default to false for unknown platforms
    return false;
}

// --- Modified Update Method ---
private void Update()
{
    if(_continuingTutorial)
    {
        return;
    }
    
    // *** THIS IS THE CRUCIAL CHANGE ***
    if (!IsInputPressed()) return; 
    
    // Original checks
    if (!_inTutorial) return;
    if (_isPlacingModules) return;

    var typewriter = ServiceLocator.Instance.DialogueManager.Typewriter;

    // --- CASE 1: Player interrupts typing ---
    if (typewriter.IsTyping)
    {
        HandleInterrupt(typewriter);
        return;
    }

    // --- CASE 2: Player presses space/taps AFTER text finished ---
    OnLineFinished();
}
    /*private void Update()
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
    */
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

    //Iphone try

    private void ShowTutorialStep(int tutorialStep)
{
    // 1. Get the original text defined in the Inspector.
    string currentText = TutorialSteps[tutorialStep].TextToShow;

    // --- CRITICAL PLATFORM CHECK ---
    // Only run the text replacement if the game is running on a phone.
    if (Application.platform == RuntimePlatform.IPhonePlayer || 
        Application.platform == RuntimePlatform.Android)
    {
        // 2. Look for the exact PC phrase and replace it with the mobile phrase.
        // We use Contains() for a quick check, but Replace() for the precise swap.
        if (currentText.Contains("press Space to continue")) 
        {
            currentText = currentText.Replace("press Space to continue", "Tap the screen to continue");
        }
    }
    
    // 3. Start Typing with the potentially modified text.
    ServiceLocator.Instance.DialogueManager.StartTyping(currentText); 

    // 4. Manage Objects (original logic)
    foreach (var obj in TutorialSteps[tutorialStep].ObjectsToShow)
    {
        obj.SetActive(true);
    }
    foreach (var obj in TutorialSteps[tutorialStep].ObjectsToHide)
    {
        obj.SetActive(false);
    }
}
    /*private void ShowTutorialStep(int tutorialStep)
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
    */
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
