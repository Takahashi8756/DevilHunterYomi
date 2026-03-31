using UnityEngine;

/// <summary>
/// 突進する敵のアニメーションを管理
/// </summary>
public class RushEnemyAnimation : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private Animator _spriteAnimator = default;
    [SerializeField] private Animator _effectAnimator = default;

    //定数
    private const string MOVE_BOOL_NAME = "Walk";
    private const string RUSH_EFFECT_BOOL = "Rush";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 移動中だった場合専用のアニメーションを再生するメソッド
    /// </summary>
    /// <param name="move">移動中か否か</param>
    public void MoveAnim(bool move)
    {
        _spriteAnimator.SetBool(MOVE_BOOL_NAME, move);
    }

    /// <summary>
    /// 突進状態だった場合専用のアニメーションを再生するメソッド
    /// </summary>
    /// <param name="rush">突進中か否か</param>
    public void RushAnim(bool rush)
    {
        _effectAnimator.SetBool(RUSH_EFFECT_BOOL, rush);
    }
}
