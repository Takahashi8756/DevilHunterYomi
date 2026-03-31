using UnityEngine;

/// <summary>
/// ジャンプ及び落下処理のクラス
/// </summary>
public class PlayerJump : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【移動用変数】")]
    [SerializeField] private float _jumpForce = 1.0f;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _maxFallSpeed = 10.0f;

    [Header("【取得用変数】")]
    [SerializeField] private PlayerGroundCheck _playerGroundCheck = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;

    //計算用変数
    private float _gravityForce = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 落下計算用floatメソッド
    /// </summary>
    public float Fall()
    {
        //地面に接地していないなら、重力をかけ続ける。
        if (!_playerGroundCheck.GroundCheck())
        {
            _gravityForce -= _gravity * Time.deltaTime;
        }
        //地面に接地しており、落下速度が0未満だったら0で固定し、落下しないようにする。
        else if(_gravityForce < 0.0f)
        {
            _gravityForce = 0.0f;
        }

        _gravityForce = Mathf.Max(_gravityForce, -_maxFallSpeed);
        return _gravityForce;
    }

    /// <summary>
    /// ジャンプ用メソッド
    /// </summary>
    public void Jump()
    {
        //接地しているならジャンプ可能
        if (_playerGroundCheck.GroundCheck())
        {
            _gravityForce = _jumpForce;
            _playerSEManager.PlayJumpClip();
        }
    }
}
