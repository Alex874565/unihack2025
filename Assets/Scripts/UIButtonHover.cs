using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler
{
    AudioManager audioManager;

    private void Start()
    {
        audioManager = ServiceLocator.Instance.AudioManager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.hover);
    }
}
