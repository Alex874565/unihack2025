using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ----------")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource SFXAudioSource;

    [Header("----------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip hover;
    public AudioClip click;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
    }


    private void Start()
    {
        musicAudioSource.clip = background;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip);
    }

    public void ChangeBackgroundMusic(AudioClip newClip, float fadeTime = 1f)
    {
        if (newClip == null)
        {
            Debug.LogWarning("AudioManager: ChangeBackgroundMusic called with null clip.");
            return;
        }

        // If same clip, do nothing
        if (musicAudioSource.clip == newClip)
            return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeMusicCoroutine(newClip, fadeTime));
    }

    private IEnumerator FadeMusicCoroutine(AudioClip newClip, float fadeTime)
    {
        float startVolume = musicAudioSource.volume;

        // Fade Out
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; // WORKS EVEN WHEN PAUSED
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();

        // Swap clip
        musicAudioSource.clip = newClip;
        musicAudioSource.Play();

        // Fade In
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; // STILL WORKS
            musicAudioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }

        musicAudioSource.volume = startVolume;
        _fadeCoroutine = null;
    }


}
