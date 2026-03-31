using UnityEngine;

/// <summary>
/// プレイヤーの入力を管理するクラス
/// 【制作日時：2025/10/27】
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    private float _horizontal = 0.0f;
    private float _vertical = 0.0f;
    private float _mouseAxisX = 0.0f;
    private float _mouseAxisY = 0.0f;
    private float _rightStickAxisX = 0.0f;
    private float _rightStickAxisY = 0.0f;
    private bool _isJump = false;
    private bool _isDodge = false;
    private bool _isShot = false;
    private bool _isReload = false;
    private bool _isAttack = false;
    private bool _isInteract = false;
    private bool _isPause = false;

    //取得用プロパティ
    public float Horizontal => _horizontal;
    public float Vertical => _vertical;
    public float MouseAxisX => _mouseAxisX;
    public float MouseAxisY => _mouseAxisY;
    public float RightStickAxisX => _rightStickAxisX;
    public float RightStickAxisY => _rightStickAxisY;
    public bool IsJump => _isJump;
    public bool IsDodge => _isDodge;
    public bool IsShot => _isShot;
    public bool IsReload => _isReload;
    public bool IsAttack => _isAttack;
    public bool IsInteract => _isInteract;
    public bool IsPause => _isPause;

    //---【定数】---//

    private const string HORIZONTAL_NAME = "Horizontal";
    private const string VERTICAL_NAME = "Vertical";
    private const string X_AXIS_NAME = "Mouse X";
    private const string Y_AXIS_NAME = "Mouse Y";
    private const string X_STICK_AXISNAME = "RightStickHorizontal";
    private const string Y_STICK_AXISNAME = "RightStickVertical";
    private const string JUMP_BUTTON_NAME = "Jump";
    private const string DODGE_BUTTON_NAME = "Dodge";
    private const string SHOT_BUTTON_NAME = "Shot";
    private const string RELOAD_BUTTON_NAME = "Reload";
    private const string CLOSERANGE_ATTACK_BUTTON_NAME = "Attack";
    private const string INTERACT_INPUT_BUTTON_NAME = "Interact";
    private const string PAUSE_INPUT_BUTTON_NAME = "Pause";

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        ResetInput();
    }

    /// <summary>
    /// 現在の入力を更新するメソッド：Updateにて実行
    /// </summary>
    public void UpdateInput()
    {
        _horizontal = Input.GetAxis(HORIZONTAL_NAME);
        _vertical = Input.GetAxis(VERTICAL_NAME);
        _mouseAxisX = Input.GetAxis(X_AXIS_NAME);
        _mouseAxisY = Input.GetAxis(Y_AXIS_NAME);
        _rightStickAxisX = Input.GetAxis(X_STICK_AXISNAME);
        _rightStickAxisY = Input.GetAxis(Y_STICK_AXISNAME);
        _isJump = Input.GetButtonDown(JUMP_BUTTON_NAME);
        _isDodge = Input.GetButtonDown(DODGE_BUTTON_NAME);
        _isShot = Input.GetButtonDown(SHOT_BUTTON_NAME);
        _isReload = Input.GetButtonDown(RELOAD_BUTTON_NAME);
        _isAttack = Input.GetButtonDown(CLOSERANGE_ATTACK_BUTTON_NAME);
        _isInteract = Input.GetButtonDown(INTERACT_INPUT_BUTTON_NAME);
        _isPause = Input.GetButtonDown(PAUSE_INPUT_BUTTON_NAME);
    }

    public void ResetInput()
    {
        _horizontal = 0.0f;
        _vertical = 0.0f;
        _mouseAxisX = 0.0f;
        _mouseAxisY = 0.0f;
        _rightStickAxisX = 0.0f;
        _rightStickAxisY = 0.0f;
        _isJump = false;
        _isDodge = false;
        _isAttack = false;
        _isShot = false;
        _isReload = false;
        _isInteract = false;
        _isPause = false;
    }
}
