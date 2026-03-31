using UnityEngine;

public class UIDamageReaction : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private RectTransform _mainUIsTransform = default;

    [Header("【リアクション設定用変数】")]
    [SerializeField] private float _reactionForce = 3.0f;
    [SerializeField] private float _reactionDuration = 0.5f;

    private float _timer = 0.0f;
    private bool _isReaction = false;

    public void DamageReaction()
    {
        _timer = 0.0f;
        _isReaction = true;
    }

    public void FixedUpdateMethod()
    {
        if(!_isReaction)
        {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer > _reactionDuration)
        {
            _timer = 0.0f;
            _mainUIsTransform.localPosition = Vector3.zero;
            _isReaction = false;
            return;
        }

        float progress = _timer / _reactionDuration;
        float currentPower = Mathf.Lerp(_reactionForce, 0, progress);
        Vector3 randomOffset = Random.insideUnitSphere * currentPower;

        _mainUIsTransform.localPosition = Vector3.zero + randomOffset;   
    }
}
