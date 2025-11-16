using UnityEngine;
using UnityEngine.UI;

public class UIButtonClick : MonoBehaviour
{
    AudioManager audioManager;

    private void Start()
    {
        audioManager = ServiceLocator.Instance.AudioManager;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        audioManager.PlaySFX(audioManager.click);
    }
}
