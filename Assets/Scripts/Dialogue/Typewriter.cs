using System.Collections;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedText;
    [SerializeField] private TMP_Text hiddenFullText;
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;

    public void StartTyping(string text)
    {
        hiddenFullText.text = text;     // store the full text (invisible)
        displayedText.text = "";        // clear visible text

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeRoutine());
    }

    private IEnumerator TypeRoutine()
    {
        string full = hiddenFullText.text;
        displayedText.maxVisibleCharacters = 0;
        displayedText.text = full;

        for (int i = 0; i <= full.Length; i++)
        {
            displayedText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
