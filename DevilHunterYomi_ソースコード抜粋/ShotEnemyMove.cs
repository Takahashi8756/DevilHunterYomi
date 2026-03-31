using UnityEngine;

/// <summary>
/// 射撃する敵の動きを管理するクラス
/// </summary>
public class ShotEnemyMove : EnemyMove
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

    [Header("【特殊変数】")]
    [SerializeField] private float _moveDistance = 3.0f;
    [SerializeField] private float _distanceOffset = 0.5f;
    [SerializeField] private float backMoveSpeed = 1.0f;

    private MoveState _moveState = MoveState.MoveForward;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// プレイヤーの位置に応じて前進したり後退したりするメソッド：FixedUpdateにて実行
    /// </summary>
    /// <param name="playerCheck">プレイヤーが範囲内にいるかどうか</param>
    /// <param name="playerPosition">プレイヤーの位置</param>
    public override void Move(bool playerCheck, Transform playerPosition)
    {
        if(_enemyKnockBack.NowState == EnemyKnockBack.KnockBackState.DefaultKnockBack)
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
    /// プレイヤーの位置によって移動方向を変えるメソッド
    /// </summary>
    /// <param name="playerPosition">プレイヤーの位置</param>
    private void PlayerPositionCheck(Transform playerPosition)
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);

        if (distance < _moveDistance - _distanceOffset)
        {
            _moveState = MoveState.MoveBackward;
        }
        else if (distance > _moveDistance + _distanceOffset)
        {
            _moveState = MoveState.MoveForward;
        }
        else
        {
            _moveState = MoveState.Stay;
        }
    }
}
