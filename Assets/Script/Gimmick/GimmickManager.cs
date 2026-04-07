using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ギミックを管理するクラス
/// </summary>
public class GimmickManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private IntoGateManager _intoGateManager = default;
    [SerializeField] private ShowTalkTextUI _showTalkTextUI = default;

    [Header("【特殊ギミック取得用変数】")]
    [SerializeField] private List<GateGimmick> _gateManagerList = default;

    [Header("【通常ギミック取得用変数】")]
    [SerializeField] private List<BaseGimmick> _gimmickList = default;

    public void SetGate(GateGimmick gate)
    {
        _gateManagerList.Add(gate);
        gate.OnEncountText += ShowEncountText;
        gate.OnIntoGate += IntoGate;
    }

    public void SetGimmick(BaseGimmick gimmick)
    {
        _gimmickList.Add(gimmick);
        gimmick.OnEncountText += ShowEncountText;
    }

    private void ShowEncountText(string text)
    {
        _showTalkTextUI.ShowTalkText(text);
    }

    private void IntoGate()
    {
        _intoGateManager.ShowSelectCanvas();
    }

    private void OnEnable()
    {
        foreach (BaseGimmick gimmick in _gimmickList)
        {
            gimmick.OnEncountText += ShowEncountText;
        }

        foreach (GateGimmick gate in _gateManagerList)
        {
            gate.OnEncountText += ShowEncountText;
            gate.OnIntoGate += IntoGate;
        }
    }

    public void OnDisable()
    {
        foreach(BaseGimmick gimmick in _gimmickList)
        {
            gimmick.OnEncountText -= ShowEncountText;
        }

        foreach(GateGimmick gate in _gateManagerList)
        {
            gate.OnEncountText -= ShowEncountText;
            gate.OnIntoGate -= IntoGate;
        }
    }
}
