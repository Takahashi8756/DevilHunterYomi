using UnityEngine;

/// <summary>
/// アイテムがプレイヤーに付いてくるようにするクラス
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【判定作成用変数】")]
    [SerializeField] private Vector3 _capsuleStartPoint = Vector3.zero;
    [SerializeField] private Vector3 _capsuleEndPoint = Vector3.zero;
    [SerializeField] private float _capsuleRadius = 1.0f;

    [Header("【移動用変数】")]
    [SerializeField] private float _moveSpeed = 3.0f;

    //配列
    private Collider[] _overlapResult = new Collider[1];

    //Gizmosの色を保管
    private Color _gizmosColor = Color.yellow;
    private bool _isCheckPlayer = false;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    public bool IsCheckPlayer => _isCheckPlayer;

    //========================================================================
    //メソッド
    //========================================================================


    private Transform PlayerCheck()
    {
        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        //CheckCapsuleではコンポーネントの取得が出来なかったため、OverlapCapsuleNonAllocにて判定を実装
        int count = Physics.OverlapCapsuleNonAlloc(startPoint, endPoint, _capsuleRadius, _overlapResult, LayerMask.GetMask(PLAYER_LAYER_NAME));

        if (count > 0)
        {
            Transform playerTransform = _overlapResult[0].transform;
            _gizmosColor = Color.red;
            return playerTransform;
        }

        _gizmosColor = Color.yellow;
        return null;
    }

    /// <summary>
    /// プレイヤーを検知した際、その方向に移動させるメソッド：FixedUpdateにて実装
    /// </summary>
    public void Follow()
    {
        Transform player = PlayerCheck();

        if (player == null)
        {
            return;
        }

        Vector3 direction = (player.position - transform.position).normalized;

        transform.position += direction * _moveSpeed * Time.deltaTime;
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディター上で範囲を見える用にするためのメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;

        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        Gizmos.DrawWireSphere(startPoint, _capsuleRadius);
        Gizmos.DrawWireSphere(endPoint, _capsuleRadius);
    }
#endif
}
