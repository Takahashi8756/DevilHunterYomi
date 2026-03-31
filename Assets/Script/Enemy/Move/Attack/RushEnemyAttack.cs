using UnityEngine;

/// <summary>
/// 突進する敵の処理を管理するクラス
/// </summary>
public class RushEnemyAttack : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【攻撃判定用変数】")]
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(0.1f, 0.1f, 0.5f);
    [SerializeField] private Vector3 _boxCenterOffset = new Vector3(0, -0.5f, 0);
    [SerializeField] private float _maxDistance = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private EnemyState _enemyState = default;

    [Header("【攻撃用変数】")]
    [SerializeField] private float _rushSmashForce = 5.0f;

    private bool _isHit = false;

    //---【定数】---//
    private const string PLAYER_TAG_NAME = "Player";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 攻撃を管理するメソッド
    /// </summary>
    public void Attack()
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
        }
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

        playerState.Damage(_enemyState.EnemyStrength, _rushSmashForce, transform.position);
        _isHit = true;
    }

    public void AttackReset()
    {
        _isHit = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // プレイヤーの中心位置を計算
        Vector3 center = transform.position + Vector3.up * _boxHalfExtents.y + _boxCenterOffset;

        // ワイヤーフレームでBoxを描画
        Gizmos.DrawWireCube(center, _boxHalfExtents * 2);
    }
#endif
}
