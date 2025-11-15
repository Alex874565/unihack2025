using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TutorialDatabase", menuName = "ScriptableObjects/TutorialDatabase", order = 1)]
public class TutorialDatabase : ScriptableObject
{
    public List<TutorialStep> TutorialSteps;

    [System.Serializable]
    public class TutorialStep
    {
        public string TextToShow;
        public List<GameObject> ObjectsToShow;
        public List<GameObject> ObjectsToHide;
    }
}
