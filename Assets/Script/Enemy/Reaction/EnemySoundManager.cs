using UnityEngine;

/// <summary>
/// 敵の効果音を管理するクラス
/// </summary>
public class EnemySoundManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【再生する用のソース】")]
    [SerializeField] private AudioSource _audioSource = default;

    [Header("【共通音声】")]
    [SerializeField] private AudioClip _damageClip = default;
    [SerializeField] private AudioClip _criticalClip = default;

    //========================================================================
    //メソッド
    //========================================================================

    //各種SE再生メソッド
    public void PlayDamageAudio()
    {
        _audioSource.PlayOneShot(_damageClip);
    }

    public void PlayCriticalAudio()
    {
        _audioSource.PlayOneShot(_criticalClip);
    }
}
