using UnityEngine;

/// <summary>
/// 敵の弾の音を管理するクラス
/// </summary>
public class EnemyBulletSound : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【再生する用のソース】")]
    [SerializeField] private AudioSource _audioSource = default;

    [Header("【共通音声】")]
    [SerializeField] private AudioClip _shotClip = default;
    [SerializeField] private AudioClip _explosionClip = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 射撃音再生
    /// </summary>
    public void PlayShotAudio()
    {
        _audioSource.PlayOneShot(_shotClip);
    }

    /// <summary>
    /// 破裂音再生
    /// </summary>
    public void PlayExplosionAudio()
    {
        _audioSource.PlayOneShot(_explosionClip);
    }
}
