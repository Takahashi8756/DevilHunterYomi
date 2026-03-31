using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵を動作させる基底クラス
/// </summary>
public class EnemyMove : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//
    [Header("【移動用変数】")]
    [SerializeField] protected float _moveSpeed = 1.0f;

    [Header("【徘徊用変数】")]
    [SerializeField] protected float _wanderRadius = 5.0f;
    [SerializeField] protected float _minWanderInterval = 2.0f;
    [SerializeField] protected float _maxWanderInterval = 4.0f;

    [Header("【取得用変数】")]
    [SerializeField] protected EnemyKnockBack _enemyKnockBack = default;
    [SerializeField] protected NavMeshAgent _navMeshAgent = default;

    protected float _timer = 0.0f;
    protected float _wanderInterval = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        _navMeshAgent.updateRotation = false;
        _wanderInterval = Random.Range(_minWanderInterval, _maxWanderInterval);
    }

    public virtual void Move(bool playerCheck, Transform playerPosition)
    {
        
    }

    /// <summary>
    /// 未発見時ランダムな方向に移動させるメソッド
    /// </summary>
    public void Wander()
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        _navMeshAgent.speed = _moveSpeed;

        if (_navMeshAgent.pathPending)
        {
            return;
        }

        //移動地点に行くまで続く
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _timer += Time.deltaTime;

            if (_timer >= _wanderInterval)
            {
                Vector3 newPosition = GetRandomPoint(transform.position, _wanderRadius);
                _navMeshAgent.SetDestination(newPosition);
                _wanderInterval = Random.Range(_minWanderInterval, _wanderInterval);
                _timer = 0.0f;
            }
        }
    }

    /// <summary>
    /// 周辺のランダムな移動可能地点を計算するメソッド
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="range">範囲</param>
    /// <returns>移動地点</returns>
    private Vector3 GetRandomPoint(Vector3 center, float range)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
}
