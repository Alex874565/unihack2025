using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;

    private void Start()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float masterVolume = masterVolumeSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(masterVolume)*20);
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicVolumeSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(musicVolume)*20);
    }

    public void SetSFXVolume()
    {
        float SFXVolume = SFXVolumeSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(SFXVolume)*20);
    }
}
