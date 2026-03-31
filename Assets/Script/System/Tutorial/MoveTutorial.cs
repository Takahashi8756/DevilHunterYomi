using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 一つ目のチュートリアル
/// </summary>
public class MoveTutorial : BaseTutorial
{
    [Header("【判定作成用】")]
    [SerializeField] private Vector3 _boxHalfExtents = Vector3.zero;

    [Header("【セリフ】")]
    [SerializeField, TextArea] private string _text = default;
    [SerializeField] private ShowTalkTextUI _showTalkTextUI = default;

    [Header("【CanvasGroup】")]
    [SerializeField] private CanvasGroup _tutorialCanvas = default;
    [SerializeField] private float _fadeDuration = 1.0f;

    //Gizmosの色を保管
    private Color _gizmosColor = Color.yellow;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    //イベント
    public override event Action OnEndTutorial = default;


    public override void StartTutorial()
    {
        _tutorialCanvas.DOFade(1.0f, _fadeDuration);
        _showTalkTextUI.ShowTalkText(_text);
    }

    public override void UpdateTutorial()
    {
        int layer = LayerMask.GetMask(PLAYER_LAYER_NAME);

        if (CheckBoxRange(layer))
        {
            _tutorialCanvas.DOFade(0.0f, _fadeDuration);
            OnEndTutorial?.Invoke();
        }
    }

    private bool CheckBoxRange(int layerMask)
    {
        Vector3 center = transform.position;
        return Physics.CheckBox(center, _boxHalfExtents, Quaternion.identity, layerMask);
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディター上で範囲を見える用にするためのメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(center, _boxHalfExtents * 2);
    }
#endif
}
