using UnityEngine;

/// <summary>
/// 敵の弾スクリプトを管理するクラス
/// </summary>
public class EnemyBulletManager : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【取得用変数】")]
    [SerializeField] private BulletMove _bulletMove = default;
    [SerializeField] private EnemyBulletGiveDamage _enemyBulletGiveDamage = default;
    [SerializeField] private BulletDelete _bulletDelete = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行されるメソッド
    /// </summary>
    private void Start()
    {
        ToReset();
    }

    private void FixedUpdate()
    {
        FixedUpdateMethod();
    }

    public void ToReset()
    {
        _bulletMove.ToReset();
        _enemyBulletGiveDamage.ToReset();
        _bulletDelete.ToReset();
    }

    public override void FixedUpdateMethod()
    {
        _bulletMove.MoveBullet(_enemyBulletGiveDamage.IsHit);
        _enemyBulletGiveDamage.Detection();
        _bulletDelete.ToDelete();
    }
}
