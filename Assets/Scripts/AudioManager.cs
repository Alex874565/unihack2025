using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ----------")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource SFXAudioSource;

    [Header("----------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip hover;
    public AudioClip click;

    private void Start()
    {
        musicAudioSource.clip = background;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip);
    }
}
