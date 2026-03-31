using UnityEngine;

/// <summary>
/// ゲートを管理するクラス
/// </summary>
public class GateManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private SpriteRenderer _gateIcon = default;

    private bool _isShowIcon = false;
    public bool IsShowIcon => _isShowIcon;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// アイコンを表示するメソッド
    /// </summary>
    public void ShowIcon()
    {
        _gateIcon.enabled = true;
        _isShowIcon = true;
    }
}
