using System;
using UnityEngine;

/// <summary>
/// ゲート突入のチュートリアル
/// </summary>
public class GateTutorial : BaseTutorial
{
    [Header("【ギミック】")]
    [SerializeField] private BaseGimmick _wallGimmick = default;
    [SerializeField] private GameObject _gateObject = default;

    [Header("【セリフ】")]
    [SerializeField, TextArea] private string _text = default;
    [SerializeField] private ShowTalkTextUI _showTalkTextUI = default;

    //終了時イベント
    public override event Action OnEndTutorial;

    public override void StartTutorial()
    {
        _showTalkTextUI.ShowTalkText(_text);

        _wallGimmick.DestroyGimmick();
        _gateObject.SetActive(true);
    }

    public override void UpdateTutorial()
    {
        
    }
}
