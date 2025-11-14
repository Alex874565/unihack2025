using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;

    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;

        closeButton.onClick.AddListener( () =>
        {
            Hide();
        });
    }

    private void Start()
    {
        PauseManager.Instance.OnGameUnpaused += PauseManager_OnGameUnpaused;

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();

        Hide();
    }

    private void PauseManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
