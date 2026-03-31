using UnityEngine;

/// <summary>
/// アイテムを出現させる時の移動を管理するクラス
/// </summary>
public class ItemPop : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【壁判定用変数】")]
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(0.5f, 1.0f, 0.5f);
    [SerializeField] private Vector3 _boxCenterOffset = new Vector3(0, -0.5f, 0);

    [Header("【移動用変数】")]
    [SerializeField] private float _maxMoveForce = 5.0f;
    [SerializeField] private float _minMoveForce = 1.0f;
    [SerializeField] private float _maxJumpForce = 5.0f;
    [SerializeField] private float _minJumpForce = 1.0f;
    [SerializeField] private float _popDuration = 2.0f;

    [Header("【取得用変数】")]
    [SerializeField] private ItemFall _itemFall = default;

    private bool _canMove = false;
    private Vector3 _popDirection = Vector3.zero;
    private float _moveForce = 0.0f;
    private float _jumpForce = 0.0f;
    private float _timer = 0.0f;
    private Color _gizmosColor = Color.blue;

    //---【定数】---//
    private const string WALL_NAME = "Wall";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 移動を開始させるメソッド
    /// </summary>
    public void MoveStart()
    {
        float randomMoveForce = Random.Range(_minMoveForce, _maxMoveForce);
        float randomJumpForce = Random.Range(_minJumpForce, _maxJumpForce);
        float angle = Random.Range(0.0f, 360.0f);

        _popDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0.0f, Mathf.Sin(angle * Mathf.Deg2Rad));
        _moveForce = randomMoveForce;
        _jumpForce = randomJumpForce;
        _timer = 0.0f;
        _itemFall.SetPopForce(_jumpForce);

        _canMove = true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        if (!_canMove)
        {
            transform.position += new Vector3(0.0f, _itemFall.Fall(), 0.0f);
            return;
        }

        _timer += Time.deltaTime;

        float force = _timer / _popDuration;
        force = Mathf.SmoothStep(0, 1, force);
        float speed = Mathf.Lerp(_moveForce, 0, force);
        _popDirection *= speed;
        Vector3 moveDirection = new Vector3(_popDirection.x, _itemFall.Fall(), _popDirection.z);

        transform.position += WallCheck(moveDirection);

        if(_timer > _popDuration)
        {
            _timer = 0.0f;
            _canMove = false;
        }
    }

    /// <summary>
    /// 壁を検知し移動方向を計算する用メソッド
    /// </summary>
    /// <param name="direction">移動方向</param>
    /// <returns>最終的な移動方向</returns>
    private Vector3 WallCheck(Vector3 direction)
    {
        Vector3 move = direction;

        Vector3 center = transform.position + Vector3.up * _boxHalfExtents.y + _boxCenterOffset;
        Quaternion rotation = Quaternion.identity;

        RaycastHit[] hitObjects = Physics.BoxCastAll(center, _boxHalfExtents, move.normalized, rotation, move.magnitude, LayerMask.GetMask(WALL_NAME));

        if (hitObjects.Length > 0)
        {
            Vector3 adjustedMove = move;

            //ProjectOnPlaneを使用し、壁に対して法線方向に滑るような移動を実現
            foreach (RaycastHit hit in hitObjects)
            {
                adjustedMove = Vector3.ProjectOnPlane(adjustedMove, hit.normal);
            }

            _gizmosColor = Color.red;

            return adjustedMove;
        }
        else
        {
            _gizmosColor = Color.blue;

            return move;
        }
    }
}
