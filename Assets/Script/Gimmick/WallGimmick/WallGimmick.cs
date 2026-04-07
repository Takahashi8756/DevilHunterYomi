using System;
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

    //イベント
    public override event Action<string> OnEncountText;

    public override bool IsInteract => _isInteract;

    public override void EncountGimmick()
    {
        
    }

    public override void InteractGimmick()
    {
        
    }

    public override void DestroyGimmick()
    {
        GameObject effect = Instantiate(_destroyEffect);
        effect.transform.position = _playEffectPosition.position;

        OnEncountText = null;
        Destroy(gameObject);
    }
}
