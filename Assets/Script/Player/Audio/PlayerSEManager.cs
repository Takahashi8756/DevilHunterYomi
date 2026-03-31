using UnityEngine;

/// <summary>
/// プレイヤーのSEを再生する用クラス
/// </summary>
public class PlayerSEManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【SE】")]
    [SerializeField] private AudioClip _walkClip = default;
    [SerializeField] private AudioClip _jumpClip = default;
    [SerializeField] private AudioClip _dodgeClip = default;    
    [SerializeField] private AudioClip _pickUpClip = default;
    [SerializeField] private AudioClip _shotClip = default;
    [SerializeField] private AudioClip _reloadClip = default;
    [SerializeField] private AudioClip _reloadMissClip = default;
    [SerializeField] private AudioClip _knifeAttackClip = default;
    [SerializeField] private AudioClip _damageClip = default;

    [Header("【AudioSource】")]
    [SerializeField] private AudioSource _moveAudioSource = default;
    [SerializeField] private AudioSource _shotAudioSource = default;
    [SerializeField] private AudioSource _pickUpAudioSource = default;

    [Header("【変数】")]
    [SerializeField] private float _playAudioCooltime = 0.05f;

    private float _timer = 0.0f;
    private bool _isPlaying = false;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// オーディオが重ならないようにするメソッド
    /// </summary>
    public void PlayAudioCoolDown()
    {
        if (!_isPlaying)
        {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer > _playAudioCooltime)
        {
            _isPlaying = false;
        }
    }

    //-----【各種SE】-----//

    public void PlayWalkAudio()
    {
        _moveAudioSource.PlayOneShot(_walkClip);
    }

    public void PlayJumpClip()
    {
        _moveAudioSource.PlayOneShot(_jumpClip);
    }

    public void PlayDodgeClip()
    {
        _moveAudioSource.PlayOneShot(_dodgeClip);
    }

    public void PlayPickUpClip()
    {
        if (_isPlaying)
        {
            return;
        }

        _pickUpAudioSource.PlayOneShot(_pickUpClip);
        _isPlaying = true;
    }

    public void PlayShotClip()
    {
        _shotAudioSource.PlayOneShot(_shotClip);
    }

    public void PlayReloadClip()
    {
        _shotAudioSource.PlayOneShot(_reloadClip);
    }

    public void PlayReloadMissClip()
    {
        _shotAudioSource.PlayOneShot(_reloadMissClip);
    }

    public void PlayKnifeAttackClip()
    {
        _shotAudioSource.PlayOneShot(_knifeAttackClip);
    }

    public void PlayDamageClip()
    {
        _shotAudioSource.PlayOneShot(_damageClip);
    }
}
