using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Typewriter Typewriter => _typewriter;
    [SerializeField] private Typewriter _typewriter;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TMP_Text _dialogueText;

    public void StartTyping(string text)
    {
        Debug.Log("Starting dialogue typing effect");
        _dialogueText.maxVisibleCharacters = 0;
        _dialogueText.text = text;
        // Implementation for starting the dialogue typing effect
        _typewriter.StartTyping(_dialogueText);
    }

    public void ShowDialogue()
    {
        _dialogueUI.SetActive(true);
    }

    public void HideDialogue()
    {
        _dialogueUI.SetActive(false);
    }
}
