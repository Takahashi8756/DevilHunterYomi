using UnityEngine;

/// <summary>
/// 敵の弾を管理するクラス
/// </summary>
public class EnemyBulletGiveDamage : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【設定用変数】")]
    [SerializeField] private float _bulletKnockBackForce = 3.0f;

    [Header("【弾判定範囲用変数】")]
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(0.1f, 0.1f, 0.5f);
    [SerializeField] private Vector3 _boxCenterOffset = new Vector3(0, -0.5f, 0);
    [SerializeField] private float _maxDistance = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private BulletData _bulletData = default;
    [SerializeField] private BulletDelete _bulletDelete = default;

    //計算用変数
    private Vector3 _center = Vector3.zero;
    private bool _isHit = false;

    //プロパティ
    public bool IsHit => _isHit;

    //---【定数】---//

    private const string PLAYER_TAG_NAME = "Player";
    private const string ENEMY_TAG_NAME = "Enemy";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行するメソッド
    /// </summary>
    public void ToStart()
    {
        //弾のデータが取得出来ていないなら再度取得
        if (_bulletData == null)
        {
            _bulletData = GetComponent<BulletData>();
        }
    }

    /// <summary>
    /// 情報をリセットするメソッド
    /// </summary>
    public void ToReset()
    {
        _isHit = false;
    }

    /// <summary>
    /// ヒットしたかどうか検知するメソッド
    /// </summary>
    public void Detection()
    {
        if (_isHit)
        {
            return;
        }

        //中心地点、向いている方向、角度を取得
        Vector3 center = transform.position + Vector3.up * _boxHalfExtents.y + _boxCenterOffset;
        Vector3 direction = transform.forward;
        Quaternion rotation = transform.rotation;

        //ヒットしたオブジェクトを格納する。
        RaycastHit hitObject;
        if (Physics.BoxCast(center, _boxHalfExtents, direction, out hitObject, rotation, _maxDistance))
        {
            //当たったものが敵だったら、ダメージを与える。
            if (hitObject.collider.CompareTag(PLAYER_TAG_NAME))
            {
                GiveDamage(hitObject.collider.GetComponent<PlayerState>());
            }
            else if (!hitObject.collider.CompareTag(ENEMY_TAG_NAME))
            {
                _bulletDelete.DeleteStart();
                _isHit = true;
            }
        }

        //gizmos表示用
        _center = center;
    }

    /// <summary>
    /// 敵にダメージを与えるメソッド
    /// </summary>
    /// <param name="hitEnemyState">ヒットした敵のPlayerStateを代入</param>
    private void GiveDamage(PlayerState playerState)
    {
        if (playerState == null)
        {
            return;
        }

        playerState.Damage(_bulletData.BulletDamage, _bulletKnockBackForce, transform.position);
        _isHit = true;
        _bulletDelete.DeleteStart();
    }

#if UNITY_EDITOR
    /// <summary>
    /// GizmosでBoxCastの判定を表示するメソッド
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_center, _boxHalfExtents * 2);
    }
#endif
}
