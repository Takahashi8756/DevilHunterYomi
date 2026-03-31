using Unity.VisualScripting;
using UnityEngine;

public class EnemyPlayerCheck : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【警戒状態判定作成用】")]
    [SerializeField] private Vector3 _capsuleStartPoint = Vector3.zero;
    [SerializeField] private Vector3 _capsuleEndPoint = Vector3.zero;
    [SerializeField] private float _capsuleRadius = 1.0f;

    [Header("【通常状態判定作成用】")]
    [SerializeField] private float _eyeSightDistance = 5.0f;
    [SerializeField] private Vector3 _eyeOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private LayerMask _obstacleLayerMask;

    //Gizmosの色を保管
    private Color _gizmosColor = Color.yellow;
    private Color _eyeRayCastColor = Color.green;

    //警戒状態かの判定
    private bool _isAlertMode = false;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    //プロパティ
    public bool IsAlertMode => _isAlertMode;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 範囲内にプレイヤーがいるか検知する用メソッド
    /// </summary>
    /// <returns>範囲内にプレイヤーがいたらtrueを返す</returns>
    public bool PlayerCheck()
    {
        int layerMask = LayerMask.GetMask(PLAYER_LAYER_NAME);

        if (_isAlertMode)
        {
            bool isInside = CheckCapsuleRange(layerMask);

            if (isInside)
            {
                return true;
            }
            else
            {
                _isAlertMode = false;
                return false;
            }
        }
        else
        {
            bool isSeen = CheckEyeSightRange(layerMask, _obstacleLayerMask);

            if(isSeen)
            {
                _eyeRayCastColor = Color.red;
                _isAlertMode = true;
                return true;
            }
            else
            {
                _eyeRayCastColor = Color.green;
            }
        }

        return false;
    }

    private bool CheckCapsuleRange(int layerMask)
    {
        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        return Physics.CheckCapsule(startPoint, endPoint, _capsuleRadius, layerMask);
    }

    private bool CheckEyeSightRange(int playerLayer, LayerMask obstacles)
    {
        Vector3 origin = transform.position + _eyeOffset;
        Vector3 direction = transform.forward;

        int searchLayer = playerLayer | obstacles;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, _eyeSightDistance, searchLayer))
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                return true;
            }
        }

        return false;
    }

    public void DamageToAlertMode()
    {
        _isAlertMode = true;
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディター上で範囲を見える用にするためのメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;

        Gizmos.color = _gizmosColor;
        Vector3 start = center + _capsuleStartPoint;
        Vector3 end = center + _capsuleEndPoint;

        Gizmos.DrawWireSphere(start, _capsuleRadius);
        Gizmos.DrawWireSphere(end, _capsuleRadius);
        Gizmos.DrawLine(start + Vector3.left * _capsuleRadius, end + Vector3.left * _capsuleRadius);
        Gizmos.DrawLine(start + Vector3.right * _capsuleRadius, end + Vector3.right * _capsuleRadius);

        Gizmos.color = _eyeRayCastColor;
        Vector3 rayOrigin = center + _eyeOffset;
        Vector3 rayDirection = transform.forward * _eyeSightDistance;
        Gizmos.DrawRay(rayOrigin, rayDirection);
    }
#endif
}
