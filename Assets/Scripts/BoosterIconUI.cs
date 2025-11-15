using UnityEngine;
using UnityEngine.UI;

public class BoosterIconUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImage;      // normal icon
    [SerializeField] private Image timerImage;     // radial overlay

    private float duration;

    public void Initialize(Sprite icon, float duration)
    {
        this.duration = duration;

        // Normal icon
        iconImage.sprite = icon;

        // Timer overlay uses same sprite
        timerImage.sprite = icon;

        // Timer starts full
        timerImage.fillAmount = 1f;
    }

    public void UpdateTimer(float remaining)
    {
        if (duration <= 0f) return;

        float fill = Mathf.Clamp01(remaining / duration);
        timerImage.fillAmount = fill;
    }
}
