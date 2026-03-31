using UnityEngine;

/// <summary>
/// プレイヤーの回避管理用クラス
/// </summary>
public class PlayerDodge : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================
    
    //---【enum】---//

    //回避状態の管理に使うenum
    public enum DodgeState
    {
        Ready,
        Dodge,
        Cooldown,
    }

    private DodgeState _nowDodgeState = DodgeState.Ready;

    public DodgeState NowDodgeState
    {
        get { return _nowDodgeState; }
        set { _nowDodgeState = value; }
    }

    //---【変数】---//

    [Header("【移動用変数】")]
    [SerializeField] private float _dodgeForce = 5.0f;
    [SerializeField] private float _dodgeDuration = 1.5f;
    [SerializeField] private float _dodgeCooltime = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;

    [Header("【オフセット】")]
    [SerializeField] private float _noInputOffset = 0.1f;

    //各種private変数
    private Vector3 _dodgeDirection = Vector3.zero;
    private float _dodgeTimer = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 回避の移動方向を決定するメソッド
    /// </summary>
    /// <param name="inputDirection">現在入力されている方向を代入：Vector3形式</param>
    /// <param name="foward">プレイヤーの正面を代入：Vector3形式</param>
    public void InputDodge(Vector3 inputDirection)
    {
        //現在回避可能でない、もしくは入力の値が一定以下なら早期リターン
        if(_nowDodgeState != DodgeState.Ready && inputDirection.sqrMagnitude <= _noInputOffset)
        {
            return;
        }

        _dodgeDirection = inputDirection.normalized;
        _dodgeTimer = 0.0f;

        _playerSEManager.PlayDodgeClip();
        _nowDodgeState = DodgeState.Dodge;
    }

    /// <summary>
    /// 回避の状態遷移用メソッド
    /// </summary>
    public void DodgeUpdater()
    {
        //ステートがReadyなら早期リターン
        if(_nowDodgeState == DodgeState.Ready)
        {
            return;
        }

        _dodgeTimer += Time.deltaTime;

        switch (_nowDodgeState)
        {
            //回避の実行時間の計算
            case DodgeState.Dodge:
                if(_dodgeTimer > _dodgeDuration)
                {
                    _dodgeTimer = 0.0f;
                    _nowDodgeState = DodgeState.Cooldown;

                    return;
                }
                break;

            //回避クールタイムの計算
            case DodgeState.Cooldown:

                float amountValue = Mathf.InverseLerp(1, 0, _dodgeTimer);
                _playerUIManager.UpdateDodgeUI(amountValue);

                if (_dodgeTimer > _dodgeCooltime)
                {
                    _dodgeTimer = 0.0f;
                    _nowDodgeState = DodgeState.Ready;

                    return;
                }
                break;
        }
    }

    /// <summary>
    /// 回避の方向、強さを計算し返すメソッド
    /// </summary>
    /// <returns>回避の移動量をVector3形式でreturn</returns>
    public Vector3 GetDodgeVector()
    {
        if(_nowDodgeState != DodgeState.Dodge)
        {
            return Vector3.zero;
        }

        float timer = _dodgeTimer / _dodgeDuration;
        timer = Mathf.SmoothStep(0f, 1f, timer);
        float speed = Mathf.Lerp(_dodgeForce, 0f, timer);

        return _dodgeDirection * speed;
    }
}
