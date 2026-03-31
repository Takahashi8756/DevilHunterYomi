using UnityEngine;

/// <summary>
/// 弾の判定を作成し、当たったものに応じて処理するクラス
/// 【制作日時：2025/10/21】
/// </summary>
public class BulletGiveDamage : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【弾判定範囲用変数】")]
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(0.1f, 0.1f, 0.5f);
    [SerializeField] private Vector3 _boxCenterOffset = new Vector3(0, -0.5f, 0);
    [SerializeField] private float _maxDistance = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private BulletData _bulletData = default;
    [SerializeField] private BulletDelete _bulletDelete = default;

    //その他
    private BulletHitSignal _bulletHitSignal = default;
    private bool _isHit = false;

    //計算用変数
    private Vector3 _center = Vector3.zero;

    //セッター
    public BulletHitSignal BulletHitSignal
    {
        set { _bulletHitSignal = value; }
    }

    //プロパティ
    public bool IsHit => _isHit;

    //---【定数】---//

    private const string ENEMY_TAG_NAME = "Enemy";
    private const string PLAYER_TAG_NAME = "Player";

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        //弾のデータが取得出来ていないなら再度取得
        if(_bulletData == null)
        {
            _bulletData = GetComponent<BulletData>();
        }
    }

    public void ToReset()
    {
        _isHit = false;
    }

    /// <summary>
    /// 当たったものを検知するメソッド：BoxCastにて実装
    /// </summary>
    public void Detection()
    {
        if (_isHit)
        {
            return;
        }

        //中心地点、向いている方向、角度を取得
        Vector3 center = transform.position + transform.rotation * _boxCenterOffset;
        Vector3 direction = transform.forward;
        Quaternion rotation = transform.rotation;

        //ヒットしたオブジェクトを格納する。
        RaycastHit hitObject;
        if(Physics.BoxCast(center, _boxHalfExtents, direction, out hitObject, rotation, _maxDistance))
        {
            //当たったものが敵だったら、ダメージを与える。
            if (hitObject.collider.CompareTag(ENEMY_TAG_NAME))
            {
                GiveDamage(hitObject.collider.GetComponent<EnemyState>());
            }
            else if (!hitObject.collider.CompareTag(PLAYER_TAG_NAME))
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
    /// <param name="hitEnemyState">ヒットした敵のEnemyStateを代入</param>
    private void GiveDamage(EnemyState hitEnemyState)
    {
        //ぶつかった敵にEnemyStateが無かったらリターン
        if(hitEnemyState == null)
        {
            return;
        }

        //ヒットした時、標準の周りを赤く光らせる。
        _bulletHitSignal.GiveHitSignal();

        //ダメージ、クリティカル、クリティカル倍率を代入しダメージを与える
        hitEnemyState.Damage(_bulletData.BulletDamage, _bulletData.BulletCritical, _bulletData.CriticalMagnification);

        _bulletDelete.DeleteStart();
        _isHit = true;
    }

#if UNITY_EDITOR
    /// <summary>
    /// GizmosでBoxCastの判定を表示するメソッド
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 gizmoCenter = transform.position + transform.rotation * _boxCenterOffset;
        Gizmos.matrix = Matrix4x4.TRS(gizmoCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _boxHalfExtents * 2);
    }
#endif
}
