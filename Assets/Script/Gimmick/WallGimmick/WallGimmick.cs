using UnityEngine;

/// <summary>
/// 壁ギミック（チュートリアル用）クラス
/// </summary>
public class WallGimmick : BaseGimmick
{
    [Header("【インタラクト可能か】")]
    [SerializeField] private bool _isInteract = true;

    [Header("【消滅エフェクト】")]
    [SerializeField] private Transform _playEffectPosition = default;
    [SerializeField] private GameObject _destroyEffect = default;

    public override bool IsInteract => _isInteract;

    public override void EncountGimmick()
    {
        
    }

    public override void InteractGimmick()
    {
        
    }

    public override void DestroyGimmick()
    {
        Instantiate(_destroyEffect);
        Destroy(gameObject);
    }
}
