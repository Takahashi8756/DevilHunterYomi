using UnityEngine;

public class PrologueAudioManager : MonoBehaviour
{
    [Header("【SE】")]
    [SerializeField] private AudioClip _typeAudio = default;
    [SerializeField] private AudioClip _skipAudio = default;

    [Header("【AudioSource】")]
    [SerializeField] private AudioSource _seAudioSource = default;

    public void PlayTypeAudio()
    {
        _seAudioSource.PlayOneShot(_typeAudio);
    }

    public void PlaySkipAudio()
    {
        _seAudioSource.PlayOneShot(_skipAudio);
    }
}
