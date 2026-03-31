using UnityEngine;

/// <summary>
/// 弾のスクリプトを管理するクラス
/// </summary>
public class BulletManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【取得用変数】")]
    [SerializeField] private BulletMove _bulletMove = default;
    [SerializeField] private BulletGiveDamage _bulletGiveDamage = default;
    [SerializeField] private BulletDelete _bulletDelete = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行するメソッド
    /// </summary>
    private void Start()
    {
        _bulletGiveDamage.ToStart();
        ToReset();
    }

    /// <summary>
    /// 一定間隔で実行されるメソッド
    /// </summary>
    private void FixedUpdate()
    {
        FixedUpdateMethod();
    }

    /// <summary>
    /// リセット用メソッド
    /// </summary>
    public void ToReset()
    {
        _bulletMove.ToReset();
        _bulletGiveDamage.ToReset();
        _bulletDelete.ToReset();
    }

    public void FixedUpdateMethod()
    {
        _bulletMove.MoveBullet(_bulletGiveDamage.IsHit);
        _bulletGiveDamage.Detection();
        _bulletDelete.ToDelete();
    }
}
