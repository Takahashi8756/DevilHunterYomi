using System.Net;
using UnityEngine;

/// <summary>
/// プレイヤーの移動処理をまとめるクラス
/// 【制作日時：2025/9/24】
/// </summary>
public class PlayerMove : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//
    [Header("【スクリプト取得】")]
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private PlayerJump _playerJump = default;
    [SerializeField] private PlayerDodge _playerDodge = default;
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerKnockBack _playerKnockBack = default;
    [SerializeField] private PlayerWallCheck _playerWallCheck = default;
    [SerializeField] private PlayerGroundCheck _playerGroundCheck = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 入力処理メソッド：Updateにて実行
    /// </summary>
    public void MoveInput(PlayerInputManager input)
    {
        //上下左右の入力取得
        float horizontal = input.Horizontal * _playerState.NowSpeed * Time.deltaTime;
        float vertical = input.Vertical * _playerState.NowSpeed * Time.deltaTime;

        //ジャンプの入力検知
        if (input.IsJump)
        {
            _playerJump.Jump();
        }

        //回避の入力検知
        if (input.IsDodge)
        {
            _playerDodge.InputDodge(new Vector3(horizontal, 0.0f, vertical));
        }

        Move(horizontal, vertical);
    }

    /// <summary>
    /// 出力処理
    /// </summary>
    private void Move(float horizontal, float vertical)
    {

        Vector3 moveDirection = Vector3.zero;

        //回避のステートによって処理を変更
        if (_playerDodge.NowDodgeState != PlayerDodge.DodgeState.Dodge)
        {
            moveDirection = new Vector3(horizontal, _playerJump.Fall(), vertical);

            //ジャンプ中でなければ、歩くアニメーションを再生
            if (_playerGroundCheck.GroundCheck())
            {
                float moveMagnitude = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
                _playerUIManager.WalkAnim(moveMagnitude);
            }
            else
            {
                _playerUIManager.WalkAnim(0.0f);
            }
        }
        //回避中は移動入力を無効にし、歩くアニメーションをストップ
        else
        {
            moveDirection = new Vector3(_playerDodge.GetDodgeVector().x, _playerJump.Fall(), _playerDodge.GetDodgeVector().z);
            _playerUIManager.WalkAnim(0.0f);
        }

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection += _playerKnockBack.KnockBack();
        transform.position += _playerWallCheck.WallCheck(moveDirection);
    }
}
