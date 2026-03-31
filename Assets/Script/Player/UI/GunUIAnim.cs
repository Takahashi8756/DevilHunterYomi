using UnityEngine;

/// <summary>
/// 銃UIのアニメーションを再生する用クラス
/// </summary>
public class GunUIAnim : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private Animator _gunWalkAnim = default;
    [SerializeField] private Animator _gunShotAnim = default;
    [SerializeField] private Animator _gunCooldownPosition = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 銃の上下運動を管理するメソッド
    /// </summary>
    /// <param name="speed">移動速度を代入</param>
    public void WalkSpeed(float speed)
    {
        _gunWalkAnim.SetFloat("WalkValue", speed);
    }

    /// <summary>
    /// 撃った時のアニメーションを再生するメソッド
    /// </summary>
    public void ShotAnim()
    {
        _gunShotAnim.SetTrigger("Shot");
    }

    /// <summary>
    /// クールダウン中のアニメーションを再生するメソッド
    /// </summary>
    public void CoolDown()
    {
        _gunCooldownPosition.SetTrigger("CoolDown");
    }

    /// <summary>
    /// クールダウンから射撃可能になった時のアニメを再生するメソッド
    /// </summary>
    public void Active()
    {
        _gunCooldownPosition.SetTrigger("Active");
    }
}
