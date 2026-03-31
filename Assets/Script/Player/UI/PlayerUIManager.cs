using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("【スクリプト取得用変数】")]
    [SerializeField] private CylinderUI _cylinderUI = default;
    [SerializeField] private ReloadUI _reloadUI = default;
    [SerializeField] private StatusGageUI _statusGageUI = default;
    [SerializeField] private GunUIAnim _gunUIAnim = default;
    [SerializeField] private ShowTalkTextUI _talkTextUI = default;
    [SerializeField] private FaceUI _faceUI = default;
    [SerializeField] private HPBarUI _hpBarUI = default;

    [Header("【UI直接取得用変数】")]
    [SerializeField] private Image _dodgeCoolDownImage = default;
    [SerializeField] private Animator _hitAnimator = default;
    [SerializeField] private Animator _damageAnimator = default;
    [SerializeField] private Text _InteractText = default;

    //定数
    private const string DAMAGE_TRIGGER_NAME = "Damage";

    //---【ダメージ関連】---//

    public void DamageVignette()
    {
        _damageAnimator.SetTrigger(DAMAGE_TRIGGER_NAME);
    }

    //---【リロード関連】---//

    public void ReloadStart(float reloadDuration, float criticalRange)
    {
        _reloadUI.SetMaxValue(reloadDuration);
        _reloadUI.SetCriticalRange(criticalRange);
        _reloadUI.MissCriticalImage(false);
        _reloadUI.showReloadUI();
    }

    public void ReloadEnd()
    {
        _cylinderUI.ResetCylinder();
        _reloadUI.MissCriticalImage(false);
        _reloadUI.hideReloadUI();
    }

    public void CriticalReloadMiss()
    {
        _reloadUI.MissCriticalImage(true);
    }

    public void UpdateReloadValue(float value)
    {
        _reloadUI.UpdateSlider(value);
    }

    //---【シリンダー関連】---//

    public void Shot()
    {
        _cylinderUI.ExpenditureBullet();
    }

    //---【銃関連】---//

    public void GunShot()
    {
        _gunUIAnim.ShotAnim();
    }

    public void WalkAnim(float speed)
    {
        _gunUIAnim.WalkSpeed(speed);
    }

    public void ToCoolDown()
    {
        _gunUIAnim.CoolDown();
    }

    public void ToActive()
    {
        _gunUIAnim.Active();
    }

    //---【回避関連】---//

    public void UpdateDodgeUI(float value)
    {
        _dodgeCoolDownImage.fillAmount = value;
    }

    //---【エイム関連】---//

    public void HitUI()
    {
        _hitAnimator.SetTrigger("Hit");
    }

    //---【HPバー関連】---//

    public void SetMaxValue(float value)
    {
        _hpBarUI.SetMaxValue(value);
    }

    public void SetValue(float value)
    {
        _hpBarUI.SetValue(value);
    }

    //---【ステータスバー関連】---//

    public void SetMaxStateValue(float health, float strength, float critical, float speed)
    {
        _statusGageUI.SetMaxValue(health, strength, critical, speed);
    }

    public void SetDefaultStateValue(float health, float strength, float critical, float speed)
    {
        _statusGageUI.SetDefaultValue(health, strength, critical, speed);
    }

    public void AddHealth(float value)
    {
        _statusGageUI.AddMaxHealthValue(value);
    }

    public void AddStrength(float value)
    {
        _statusGageUI.AddStrengthValue(value);
    }

    public void AddCritical(float value)
    {
        _statusGageUI.AddCriticalValue(value);
    }

    public void AddSpeed(float value)
    {
        _statusGageUI.AddSpeedValue(value);
    }

    public void InteractText(bool show)
    {
        _InteractText.enabled = show;
    }

    public void ShowTalkText(string text)
    {
        _talkTextUI.ShowTalkText(text);
    }

    //その他

    public void FaceDamage()
    {
        _faceUI.DamageFace();
    }
}
