using UnityEngine;
using UnityEngine.UI;

public class BoosterIconUI : MonoBehaviour
{
    public Image iconImage;      // the main icon
    public Image timerFill;      // radial fill image

    private float _duration;
    private float _remaining;

    public void Initialize(Sprite sprite, float duration)
    {
        iconImage.sprite = sprite;
        _duration = duration;
        _remaining = duration;
    }

    public void UpdateTimer(float remainingTime)
    {
        _remaining = remainingTime;

        if (_duration > 0)
            timerFill.fillAmount = remainingTime / _duration;
        else
            timerFill.fillAmount = 0f;
    }
}
