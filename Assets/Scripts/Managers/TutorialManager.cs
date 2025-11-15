using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialDatabase tutorialDatabase;

    private int _tutorialStep;

    void Start()
    {
        _tutorialStep = 0;
    }
}
