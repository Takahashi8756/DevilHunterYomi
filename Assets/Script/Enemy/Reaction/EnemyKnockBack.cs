using UnityEngine;

/// <summary>
/// 敵のノックバックを管理するクラス
/// </summary>
public class EnemyKnockBack : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    public enum KnockBackState
    {
        Wait,
        DefaultKnockBack,
    }

    [Header("【変数】")]
    [SerializeField] private float _defaultKnockBackPower = 5.0f;
    [SerializeField] private float _knockBackDuration = 1.0f;

    private float _timer = 0.0f;
    private Vector3 _forward = Vector3.zero;

    private KnockBackState _knockBackState = KnockBackState.Wait;
    public KnockBackState NowState => _knockBackState;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// ノックバックを開始するメソッド
    /// </summary>
    public void StartKnockBack()
    {
        _timer = 0.0f;
        _forward = transform.forward;

        _knockBackState = KnockBackState.DefaultKnockBack;
    }

    /// <summary>
    /// ノックバックによる移動を管理するメソッド（攻撃を受けた方向ではなく単純に背面にノックバックする）
    /// </summary>
    /// <returns></returns>
    public Vector3 KnockBack()
    {
        if(_knockBackState != KnockBackState.DefaultKnockBack)
        {
            return Vector3.zero;
        }

        _timer += Time.deltaTime;
        float currentPower = Mathf.Lerp(_defaultKnockBackPower, 0, _timer / _knockBackDuration);

        if(_timer > _knockBackDuration)
        {
            _timer = 0.0f;
            _knockBackState = KnockBackState.Wait;
        }

        return -_forward * currentPower;
    }
}
