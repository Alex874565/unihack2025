using UnityEngine;
using UnityEngine.UI;

public class SliderSound : MonoBehaviour
{
    [SerializeField] private AudioClip slideSound;

    private Slider slider;
    private AudioManager audioManager;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        audioManager = FindObjectOfType<AudioManager>();

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (audioManager != null && slideSound != null)
            audioManager.PlaySFX(slideSound);
    }
}
