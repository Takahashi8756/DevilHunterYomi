using UnityEngine;

/// <summary>
/// ボスをテレポートさせるクラス
/// </summary>
public class BossTeleport : MonoBehaviour
{
    public enum TeleportState
    {
        Wait,
        Teleport,
    }

    [Header("【攻撃判定作成用変数】")]
    [SerializeField] private Vector3 _capsuleStartPoint = Vector3.zero;
    [SerializeField] private Vector3 _capsuleEndPoint = Vector3.zero;
    [SerializeField] private float _capsuleRadius = 1.0f;

    [Header("【攻撃用変数】")]
    [SerializeField] private float _smashForce = 2.0f;
    [SerializeField] private float _appearanceDuration = 2.0f;

    //作業用変数
    private Color _gizmosColor = Color.red;
    private Vector3 _playerPosition = Vector3.zero;
    private float _timer = 0.0f;
    private TeleportState _nowState = TeleportState.Wait;
    private PlayerState _playerState = default;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    //プロパティ
    public TeleportState NowTeleportState => _nowState;

    /// <summary>
    /// 生成時実行：プレイヤーのステートを代入（仮）
    /// </summary>
    /// <param name="playerState"></param>
    public void ToStart(PlayerState playerState)
    {
        _playerState = playerState;
    }

    /// <summary>
    /// テレポートを開始させる処理
    /// </summary>
    /// <param name="playerPosition"></param>
    public void Teleport(Transform playerPosition)
    {
        if (_nowState != TeleportState.Wait)
        {
            return;
        }

        _playerPosition = playerPosition.position;
        _nowState = TeleportState.Teleport;
    }

    private void TeleportAttack()
    {
        transform.position  = new Vector3(_playerPosition.x, transform.position.y, _playerPosition.z);

        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        if(Physics.CheckCapsule(startPoint, endPoint, _capsuleRadius, LayerMask.GetMask(PLAYER_LAYER_NAME)))
        {
            _playerState.Damage(0.0f, _smashForce, transform.position);
        }

        _nowState = TeleportState.Wait;
    }

    public void TeleportCounter()
    {
        if(_nowState != TeleportState.Teleport)
        {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer > _appearanceDuration)
        {
            _timer = 0.0f;
            TeleportAttack();   
        }
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
    }
#endif
}
