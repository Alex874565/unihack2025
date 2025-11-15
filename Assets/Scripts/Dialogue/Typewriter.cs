using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Typewriter : MonoBehaviour
{
    public bool IsTyping => _isTyping;
    [SerializeField] private float normalDelay;
    [SerializeField] private float spaceDelay;
    [SerializeField] private float lineDelay;
    private bool _isTyping;

    private Coroutine coroutine;

    public void StartTyping(TMP_Text textField)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }


        coroutine = StartCoroutine(TypeRoutine(textField));
    }

    public IEnumerator TypeRoutine(TMP_Text textField)
    {
        _isTyping = true;
        textField.maxVisibleCharacters = 0;

        // Wait for TMP to update layout before reading characterInfo
        yield return null;

        int totalChars = textField.textInfo.characterCount;

        for (int i = 0; i < totalChars; i++)
        {
            textField.maxVisibleCharacters = i + 1;

            char c = textField.text[i];

            // Determine delay
            if (c == ' ')
            {
                yield return new WaitForSecondsRealtime(spaceDelay);
            }
            else if (c == '\n')
            {
                yield return new WaitForSecondsRealtime(lineDelay);
            }
            else
            {
                yield return new WaitForSecondsRealtime(normalDelay);
            }
        }
        _isTyping = false;
    }

    public void StopCurrentCoroutine()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        _isTyping = false;
    }

}
