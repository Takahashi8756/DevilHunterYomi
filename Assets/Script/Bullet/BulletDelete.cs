using UnityEngine;

/// <summary>
/// 弾を消す処理を管理するクラス
/// </summary>
public class BulletDelete : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private MeshRenderer _meshRenderer = default;
    [SerializeField] private ParticleSystem _perticleSystem = default;
    [SerializeField] private ParticleSystem _firePerticle = default;
    [SerializeField] private EnemyBulletSound _enemyBulletSound = default;  

    [Header("【その他変数】")]
    [SerializeField] private float _deleteDuration = 1.0f;

    private float _timer = 0.0f;
    private bool _isDelete = false;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 弾の情報をリセットするメソッド
    /// </summary>
    public void ToReset()
    {
        _meshRenderer.enabled = true;

        if (_firePerticle != null)
        {
            _firePerticle.Play();
        }

        if(_enemyBulletSound != null)
        {
            _enemyBulletSound.PlayShotAudio();
        }
    }

    /// <summary>
    /// 弾の消滅を開始するメソッド
    /// </summary>
    public void DeleteStart()
    {
        _timer = 0.0f;
        _meshRenderer.enabled = false;

        _perticleSystem.Play();

        if(_firePerticle  != null)
        {
            _firePerticle.Stop();
        }

        if (_enemyBulletSound != null)
        {
            _enemyBulletSound.PlayExplosionAudio();
        }

        _isDelete = true;
    }

    /// <summary>
    /// 消滅処理
    /// </summary>
    public void ToDelete()
    {
        if (!_isDelete)
        {
            return;
        }

        _timer += Time.deltaTime;
        
        if(_timer  > _deleteDuration)
        {
            _isDelete = false;
            gameObject.SetActive(false);
        }
    }
}
