using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    [Header("【壁判定用変数】")]
    [SerializeField] private Vector3 _capsuleStartPoint = Vector3.zero;
    [SerializeField] private Vector3 _capsuleEndPoint = Vector3.zero;
    [SerializeField] private float _capsuleRadius = 1.0f;

    [Header("【判定する壁】")]
    [SerializeField] private LayerMask _checkLayer = default;

    private Color _gizmosColor = Color.green;

    /// <summary>
    /// 壁判定+移動方向決定用メソッド：CapsuleCastAllにて実装
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    /// <returns>周囲の壁を加味した移動可能方向を返す</returns>
    public Vector3 WallCheck(Vector3 moveDirection)
    {
        Vector3 move = moveDirection;

        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        RaycastHit[] hitObjects = Physics.CapsuleCastAll(startPoint, endPoint, _capsuleRadius, move.normalized, move.magnitude, _checkLayer);

        if (hitObjects.Length > 0)
        {
            Vector3 adjustedMove = move;

            //ProjectOnPlaneを使用し、壁に対して法線方向で滑るような移動を実現
            foreach (RaycastHit hit in hitObjects)
            {
                //めり込み時壁判定を無効化
                if (hit.distance == 0)
                {
                    continue;
                }

                adjustedMove = Vector3.ProjectOnPlane(adjustedMove, hit.normal);
            }

            _gizmosColor = Color.red;

            return adjustedMove;
        }
        else
        {
            _gizmosColor = Color.green;

            return move;
        }
    }


#if UNITY_EDITOR

    /// <summary>
    /// GizmosでBoxCastの判定を表示するメソッド
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
