using System;
using UnityEngine;

/// <summary>
/// ゲートを管理するクラス
/// </summary>
public class GateGimmick : BaseGimmick
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【インタラクト可能か】")]
    [SerializeField] private bool _isInteract = true;

    [Header("【取得用変数】")]
    [SerializeField] private SpriteRenderer _gateIcon = default;
    [SerializeField] private IntoGateManager _intoGateManager = default;

    [Header("【発見時テキスト】")]
    [SerializeField, TextArea] private string _checkGateText = default;

    private bool _isShowIcon = false;

    public override bool IsInteract => _isInteract;

    //イベント
    public override event Action<string> OnEncountText;
    public event Action OnIntoGate;

    //========================================================================
    //メソッド
    //========================================================================

    public void SetGateEvent(IntoGateManager intoGateManager)
    {
        _intoGateManager = intoGateManager;
    }

    public override void EncountGimmick()
    {
        if (_isShowIcon)
        {
            return;
        }

        _gateIcon.enabled = true;
        _isShowIcon = true;

        OnEncountText?.Invoke(_checkGateText);
    }

    public override void InteractGimmick()
    {
        OnIntoGate?.Invoke();
    }

    public override void DestroyGimmick()
    {
        
    }
}
