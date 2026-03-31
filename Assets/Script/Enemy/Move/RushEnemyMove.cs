using UnityEngine;

/// <summary>
/// 突進する敵の動きを管理するクラス
/// </summary>
public class RushEnemyMove : EnemyMove
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    private enum MoveState
    {
        MoveForward,
        MoveBackward,
        Stay,
    }

    public enum RushState
    {
        Aiming,
        WindUp,
        Rush,
    }

    [Header("【特殊変数】")]
    [SerializeField] private float _moveDistance = 3.0f;
    [SerializeField] private float _distanceOffset = 0.5f;
    [SerializeField] private float backMoveSpeed = 1.0f;
    [SerializeField] private float _toAttackDuration = 3.0f;

    [Header("【後退用変数】")]
    [SerializeField] private float _windUpDuration = 0.5f;
    [SerializeField] private float _windUpSpeed = 1.5f;

    [Header("【突進用変数】")]
    [SerializeField] private float _rushSpeed = 2.0f;
    [SerializeField] private float _rushDuration = 2.0f;
    [SerializeField] private RushEnemyAttack _rushEnemyAttack = default;
    [SerializeField] private RushEnemyAnimation _rushEnemyAnimation  = default;

    //作業用変数
    private MoveState _moveState = MoveState.MoveForward;
    private RushState _rushState = RushState.Aiming;
    private float _attackTimer = 0.0f;
    private float _windUpTimer = 0.0f;
    private float _rushTimer = 0.0f;
    private Vector3 _rushDirection = default;

    //定数
    public RushState NowRushState => _rushState;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="playerCheck">プレイヤーを検知したか</param>
    /// <param name="playerPosition">プレイヤーの位置</param>
    public override void Move(bool playerCheck, Transform playerPosition)
    {
        switch (_rushState)
        {
            case RushState.Aiming:

                AimingMove(playerCheck, playerPosition);

                if(_attackTimer > _toAttackDuration)
                {
                    _rushDirection = (playerPosition.position - transform.position).normalized;
                    _rushEnemyAttack.AttackReset();

                    _windUpTimer = 0.0f;
                    _rushState = RushState.WindUp;
                }

                break;

            case RushState.WindUp:
                WindUpMove();
                break;

            case RushState.Rush:

                RushMove(_rushDirection);
                _rushEnemyAttack.Attack();
                break;
        }
    }

    /// <summary>
    /// 狙う時の移動処理
    /// </summary>
    /// <param name="playerCheck">プレイヤーを検知したか</param>
    /// <param name="playerPosition">プレイヤーの位置</param>
    private void AimingMove(bool playerCheck, Transform playerPosition)
    {
        if (_enemyKnockBack.NowState == EnemyKnockBack.KnockBackState.DefaultKnockBack)
        {
            _navMeshAgent.Move(_enemyKnockBack.KnockBack());
            return;
        }

        //未検知状態は徘徊
        if (!playerCheck)
        {
            Wander();
            return;
        }

        _timer = 0.0f;
        PlayerPositionCheck(playerPosition);

        //プレイヤーの位置によって後退と前進をする。
        switch (_moveState)
        {
            case MoveState.MoveForward:
                _navMeshAgent.speed = _moveSpeed;
                _navMeshAgent.SetDestination(playerPosition.position);
                break;

            case MoveState.MoveBackward:
                _navMeshAgent.speed = backMoveSpeed;

                Vector3 directionToEnemy = (transform.position - playerPosition.position).normalized;
                Vector3 backPosition = transform.position + (directionToEnemy * 2.0f);

                _navMeshAgent.SetDestination(backPosition);
                break;

            case MoveState.Stay:
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.ResetPath();
                }
                break;
        }
    }

    /// <summary>
    /// 突進予備動作の処理
    /// </summary>
    private void WindUpMove()
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        _windUpTimer += Time.deltaTime;

        float windUpForce = _windUpTimer / _windUpDuration;
        windUpForce = Mathf.SmoothStep(0f, 1f, windUpForce);
        float speed = Mathf.Lerp(_windUpSpeed, 0f, windUpForce);
        Vector3 backVector = -_rushDirection * speed * Time.deltaTime;

        _navMeshAgent.Move(backVector);

        if (_windUpTimer > _windUpDuration)
        {
            _rushState = RushState.Rush;
        }
    }

    /// <summary>
    /// 突進するときの処理
    /// </summary>
    /// <param name="playerPosition">プレイヤーの位置</param>
    private void RushMove(Vector3 playerPosition)
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        Vector3 moveVector = _rushDirection * _rushSpeed * Time.deltaTime;

        _navMeshAgent.Move(moveVector);
        _rushEnemyAnimation.RushAnim(true);
        _rushTimer += Time.deltaTime;

        //指定時間経ったら突進解除
        if (_rushTimer > _rushDuration)
        {
            _rushTimer = 0.0f;
            _attackTimer = 0.0f;
            _rushEnemyAnimation.RushAnim(false);
            _rushState = RushState.Aiming;

            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.ResetPath();
            }
        }
    }

    /// <summary>
    /// プレイヤーの位置によって移動方向を変えるメソッド
    /// </summary>
    /// <param name="playerPosition">プレイヤーの位置</param>
    private void PlayerPositionCheck(Transform playerPosition)
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);

        //追っている時はチャージしない
        if (distance < _moveDistance - _distanceOffset)
        {
            _rushEnemyAnimation.MoveAnim(true);
            _moveState = MoveState.MoveBackward;
        }
        else if (distance > _moveDistance + _distanceOffset)
        {
            _rushEnemyAnimation.MoveAnim(true);
            _moveState = MoveState.MoveForward;
        }
        else
        {
            _rushEnemyAnimation.MoveAnim(false);
            _moveState = MoveState.Stay;
        }

        _attackTimer += Time.deltaTime;
    }
}
