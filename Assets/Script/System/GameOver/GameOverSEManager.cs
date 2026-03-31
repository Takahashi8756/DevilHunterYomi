using UnityEngine;

public class GameOverSEManager : MonoBehaviour
{
    [Header("【SE】")]
    [SerializeField] private AudioClip _gameOverAudio = default;

    [Header("【AudioSource】")]
    [SerializeField] private AudioSource _audioSource = default;

    public void PlayGameOverAudio()
    {
        _audioSource.PlayOneShot(_gameOverAudio);
    }
}
