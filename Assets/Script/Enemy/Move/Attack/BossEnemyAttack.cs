using UnityEngine;

/// <summary>
/// ボスの攻撃を管理するクラス
/// </summary>
public class BossEnemyAttack : EnemyAttack
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    private enum AttackState
    {
        Ready,
        CoolDown,
    }

    [Header("【攻撃用変数】")]
    [SerializeField] private float _attackCooldown = 1.0f;
    [SerializeField] private int _numberOfShotBullet = 3;

    [Header("【取得用変数】")]
    [SerializeField] private EnemyState _enemyState = default;
    [SerializeField] private BossShotAttack _bossShotAttack = default;
    [SerializeField] private BossTeleport _bossTeleport = default;

    [Header("【仮変数】")]
    [SerializeField] private PlayerState _playerState = default;

    //作業用変数
    private float _timer = 0.0f;
    private EnemyBulletPool _enemyBulletPool = default;
    private AttackState _nowState = AttackState.CoolDown;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行するメソッド
    /// </summary>
    /// <param name="pool"></param>
    public override void ToStart(EnemyBulletPool pool)
    {
        _enemyBulletPool = pool;
        _timer = 0.0f;

        _bossShotAttack.ToStart(_enemyBulletPool, _enemyState);
        _bossTeleport.ToStart(_playerState);
    }

    /// <summary>
    /// 攻撃を実行するメソッド
    /// </summary>
    /// <param name="playerCheck">プレイヤーを検知したか</param>
    public override void Attack(bool playerCheck)
    {
        if (!playerCheck)
        {
            return;
        }

        ShotUpdate();
    }

    /// <summary>
    /// 射撃のステート管理用メソッド
    /// </summary>
    private void ShotUpdate()
    {

        switch (_nowState)
        {
            case AttackState.Ready:

                _bossShotAttack.Shot(_numberOfShotBullet);
                _nowState = AttackState.CoolDown;

                break;

            case AttackState.CoolDown:

                CoolDown();

                break;
        }

    }

    /// <summary>
    /// クールダウンを管理するメソッド
    /// </summary>
    private void CoolDown()
    {
        if (_timer > _attackCooldown)
        {
            _timer = 0.0f;
            _nowState = AttackState.Ready;
            return;
        }

        _timer += Time.deltaTime;
    }
}
