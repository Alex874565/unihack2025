using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ----------")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource SFXAudioSource;

    [Header("----------- Audio Clip ----------")]
    public AudioClip background;

    private void Start()
    {
        musicAudioSource.clip = background;
        musicAudioSource.Play();
    }
}
