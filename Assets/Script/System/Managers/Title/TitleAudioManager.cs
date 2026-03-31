using UnityEngine;

public class TitleAudioManager : MonoBehaviour
{
    [Header("【SE】")]
    [SerializeField] private AudioClip _choiceAudio = default;
    [SerializeField] private AudioClip _submitAudio = default;
    [SerializeField] private AudioClip _startAudio = default;

    [Header("【AudioSource】")]
    [SerializeField] private AudioSource _audioSource = default;

    public void PlayChoiceAudio()
    {
        _audioSource.PlayOneShot(_choiceAudio);
    }

    public void PlaySubmitAudio()
    {
        _audioSource.PlayOneShot(_submitAudio);
    }

    public void PlayStartAudio()
    {
        _audioSource.PlayOneShot(_startAudio);
    }
} 
