using UnityEngine;

/// <summary>
/// FPS的カメラ移動のスクリプト
/// 【作成者：髙橋英士】
/// 【制作日時：2025/9/24】
/// </summary>
public class CameraMove : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【移動用変数】")]
    [SerializeField] private float _maxRotateY = 80.0f;
    [SerializeField] private float _minRoteteY = -80.0f;

    [Header("【オブジェクト取得変数】")]
    [SerializeField] private GameObject _cameraDirection = default;

    //変数
    private float _pitch = 0.0f;
    private float _sensitivity = 1.0f;
    private float _stickSensitivity = 1.0f;

    private const string MOUSE_SENSITIVITY_PREFS_KEY = "Sensitivity";
    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        float sensitivity = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_PREFS_KEY);
        _sensitivity = sensitivity;
        _stickSensitivity = sensitivity;
    }

    /// <summary>
    /// カメラを移動させるためのメソッド：Updateメソッドで実行
    /// </summary>
    public void InputCameraMove(PlayerInputManager input)
    {
        //マウスの移動量を取得
        float mouseX = input.MouseAxisX * _sensitivity;
        float mouseY = input.MouseAxisY * _sensitivity;

        //コントローラーの右スティックの入力を取得
        float stickX = input.RightStickAxisX * _stickSensitivity;
        float stickY = -input.RightStickAxisY * _stickSensitivity;

        //入力から回転入力を計算
        float inputX = (mouseX + stickX) * Time.deltaTime;
        float inputY = (mouseY + stickY) * Time.deltaTime;

        //横方向はプレイヤーのY軸を直接変更
        transform.Rotate(Vector3.up * inputX);

        //縦方向は子オブジェクトのX軸を変更（Mathf.Clampで回転角度は制限）
        _pitch -= inputY;
        _pitch = Mathf.Clamp(_pitch, _minRoteteY, _maxRotateY);
        _cameraDirection.transform.localEulerAngles = new Vector3(_pitch, 0.0f, 0.0f);
    }

    public void UpdateSensitivity(float sensitivity)
    {
        _sensitivity = sensitivity;
    }
}
