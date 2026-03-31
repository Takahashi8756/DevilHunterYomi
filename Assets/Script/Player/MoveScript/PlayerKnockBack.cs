using UnityEngine;

public class PlayerKnockBack : MonoBehaviour
{
    [Header("【計算用変数】")]
    [SerializeField] private float _knockBackDuration = 1.0f;

    //作業用変数
    private bool _isKnockBack = false;
    private Vector3 _direction = Vector3.zero;
    private float _force = 0.0f;
    private float _timer = 0.0f;

    //プロパティ
    public bool IsKnockBack => _isKnockBack;

    /// <summary>
    /// プレイヤーをノックバックさせるメソッド
    /// </summary>
    /// <param name="damagePosition">ダメージを受けた場所</param>
    /// <param name="strength">受けたノックバックの強さ</param>
    /// <returns></returns>
    public void StartKnockBack(Vector3 damagePosition, float force)
    {
        _isKnockBack = true;
        _timer = 0.0f;
        _force = force;

        Vector3 direction = transform.position - damagePosition;
        direction.y = 0.0f;
        _direction = direction.normalized;
    }

    /// <summary>
    /// ノックバックの方向と強さを返すVector3型メソッド
    /// </summary>
    /// <returns>向きと強さ</returns>
    public Vector3 KnockBack()
    {
        if(!_isKnockBack)
        {
            return Vector3.zero;
        }

        _timer += Time.deltaTime;

        if (_timer > _knockBackDuration)
        {
            _timer = 0.0f;
            _force = 0.0f;
            _isKnockBack = false;
        }

        float timeRatio = _timer / _knockBackDuration;
        timeRatio = Mathf.SmoothStep(0f, 1f, timeRatio);
        float currentForce = Mathf.Lerp(_force, 0f, timeRatio);

        return _direction * currentForce;
    }
}
