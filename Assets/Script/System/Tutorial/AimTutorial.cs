using System;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// カメラ移動のチュートリアルクラス
/// </summary>
public class AimTutorial : BaseTutorial
{
    [Header("【判定作成用】")]
    [SerializeField] private Vector3 _boxHalfExtents = Vector3.zero;

    [Header("【CanvasGroup】")]
    [SerializeField] private CanvasGroup _tutorialCanvas = default;
    [SerializeField] private float _fadeDuration = 1.0f;

    //Gizmosの色を保管
    private Color _gizmosColor = Color.yellow;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    public override event Action OnEndTutorial;

    public override void StartTutorial()
    {
        _tutorialCanvas.DOFade(1.0f, _fadeDuration).SetLink(gameObject);
    }

    public override void UpdateTutorial()
    {
        int layer = LayerMask.GetMask(PLAYER_LAYER_NAME);

        if (CheckBoxRange(layer))
        {
            _tutorialCanvas.DOFade(0.0f, _fadeDuration).SetLink(gameObject);
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
