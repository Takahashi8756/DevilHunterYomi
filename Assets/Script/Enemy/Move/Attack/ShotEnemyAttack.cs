using UnityEngine;

/// <summary>
/// 射撃する敵の攻撃を管理するクラス
/// </summary>
public class ShotEnemyAttack : EnemyAttack
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

    [Header("【取得用変数】")]
    [SerializeField] private EnemyState _enemyState = default;
    [SerializeField] private Transform _shotPoint = default;

    [Header("【RayCast作成用変数】")]
    [SerializeField] private float _eyeSightDistance = 20.0f;
    [SerializeField] private Vector3 _eyeOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private LayerMask _obstacleLayerMask;

    //作業用変数
    private float _timer = 0.0f;
    private EnemyBulletPool _enemyBulletPool = default;
    private AttackState _nowState = AttackState.CoolDown;

    //定数
    private const string PLAYER_LAYERNAME = "Player";

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
    }

    /// <summary>
    /// 攻撃時実行するメソッド
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
    /// 射撃時のステート変更用のメソッド
    /// </summary>
    private void ShotUpdate()
    {
        int layerMask = LayerMask.GetMask(PLAYER_LAYERNAME);
        if (!CheckEyeSightRange(layerMask, _obstacleLayerMask))
        {
            return;
        }

        switch (_nowState)
        {
            case AttackState.Ready:

                Shot();

                break;

            case AttackState.CoolDown:

                CoolDown();

                break;
        }
    }

    /// <summary>
    /// 任意の方向に射撃するメソッド
    /// </summary>
    private void Shot()
    {
        GameObject bullet =  _enemyBulletPool.GetBullet();
        Vector3 shotDirection = _shotPoint.transform.forward;

        bullet.transform.position = _shotPoint.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(shotDirection);

        bullet.SetActive(true);

        bullet.GetComponent<TrailRenderer>().Clear();
        bullet.GetComponent<BulletData>().SetDamage(_enemyState.EnemyStrength);
        bullet.GetComponent<EnemyBulletManager>().ToReset();

        _nowState = AttackState.CoolDown;
    }

    /// <summary>
    /// 射撃後のクールダウンを管理するメソッド
    /// </summary>
    private void CoolDown()
    {
        if(_timer > _attackCooldown)
        {
            _timer = 0.0f;
            _nowState = AttackState.Ready;
            return;
        }

        _timer += Time.deltaTime;
    }

    private bool CheckEyeSightRange(int playerLayer, LayerMask obstacles)
    {
        Vector3 origin = transform.position + _eyeOffset;
        Vector3 direction = transform.forward;

        int searchLayer = playerLayer | obstacles;
        if(Physics.Raycast(origin, direction, out RaycastHit hit, _eyeSightDistance, searchLayer))
        {
            if(((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                return true;
            }
        }

        return false;
    }
}
