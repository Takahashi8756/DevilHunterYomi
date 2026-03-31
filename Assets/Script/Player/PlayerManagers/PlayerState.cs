using System;
using UnityEngine;

/// <summary>
/// プレイヤーのステータス（体力など）を管理するクラス
/// 【制作日時：2025/10/17】
/// </summary>
public class PlayerState : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    public enum PlayerStatus
    {
        Active,
        ContentsSelect,
        Movie,
        Death,
    }

    public enum AttackState
    {
        Ready,
        KnifeAttack,
        DontAttack,
    }

    private PlayerStatus _nowStatus = PlayerStatus.Active;
    private AttackState _nowAttackState = AttackState.Ready;

    [Header("【ステータス(初期値)】")]
    [SerializeField] private float _playerHealth = 100.0f;
    [SerializeField] private float _playerStrength = 10.0f;
    [SerializeField] private float _playerCritical = 5.0f;
    [SerializeField] private float _criticalMagnification = 1.2f;
    [SerializeField] private float _playerSpeed = 10.0f;

    [Header("【ステータス(最大値)】")]
    [SerializeField] private float _maxHealthValue = 200.0f;
    [SerializeField] private float _maxStrengthValue = 100.0f;
    [SerializeField] private float _maxCriticalValue = 100.0f;
    [SerializeField] private float _maxSpeedValue = 50.0f;

    [Header("【取得用変数】")]
    [SerializeField] private PlayerKnockBack _playerKnockBack = default;
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;
    [SerializeField] private UIDamageReaction _uiDamageReaction = default;
    [SerializeField] private GetItemPopText _getItemPopText = default;
    //計算用変数
    private float _nowHealth = 0.0f;
    private float _maxHealth = 0.0f;
    private float _nowStrength = 0.0f;
    private float _nowCritical = 0.0f;
    private float _nowMagnification = 0.0f;
    private float _nowSpeed = 0.0f;

    //イベント
    public event Action GameOverEvent = default;
    public event Action OnDamageEvent = default;

    //プロパティ
    public float NowHealth => _nowHealth;
    public float MaxHealth => _maxHealth;
    public float NowStrength => _nowStrength;
    public float NowCritical => _nowCritical;
    public float NowMagnification => _nowMagnification;
    public float NowSpeed => _nowSpeed;
    public PlayerStatus NowStatus => _nowStatus;
    public AttackState NowAttackState => _nowAttackState;


    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 値を初期化するメソッド：Startにて実行
    /// </summary>
    public void ToStart()
    {
        //値の初期化
        _nowHealth = _playerHealth;
        _maxHealth = _playerHealth;
        _nowStrength = _playerStrength;
        _nowCritical = _playerCritical;
        _nowMagnification = _criticalMagnification;
        _nowSpeed = _playerSpeed;

        _playerUIManager.SetMaxValue(_maxHealth);
        _playerUIManager.SetValue(_nowHealth);
        _playerUIManager.SetMaxStateValue(_maxHealthValue, _maxStrengthValue, _maxCriticalValue, _maxSpeedValue);
        _playerUIManager.SetDefaultStateValue(_maxHealth, _nowStrength, _nowCritical, _nowSpeed);
    }

    /// <summary>
    /// ダメージ計算用メソッド
    /// </summary>
    /// <param name="damage">プレイヤーに与えたいダメージの数値を代入</param>
    public void Damage(float damage, float knockBackForce, Vector3 damagePoint)
    {        
        if(_nowStatus != PlayerStatus.Active)
        {
            return;
        }

        _nowHealth = Mathf.Max(0.0f, _nowHealth -= damage);
        _playerUIManager.SetValue(_nowHealth);
        _playerUIManager.DamageVignette();
        _playerUIManager.FaceDamage();
        _playerSEManager.PlayDamageClip();
        _uiDamageReaction.DamageReaction();
        _playerKnockBack.StartKnockBack(damagePoint, knockBackForce);
        OnDamageEvent?.Invoke();

        if(_nowHealth <= 0.0f)
        {
            ToGameOver();
        }
    }

    /// <summary>
    /// アイテムを取得する用メソッド
    /// </summary>
    /// <param name="itemData">アイテムのデータ</param>
    public void GetItem(ItemData itemData)
    {
        AdditionNowHealth(itemData.AdditionHealthValue);
        AdditionMaxHealth(itemData.AdditionMaxHealthValue);
        AdditionStrength(itemData.AdditionStrengthValue);
        AdditionSpeed(itemData.AdditionSpeedValue);
        AdditionCritical(itemData.AdditionCriticalValue);
        _getItemPopText.PopGetItemText(itemData.ItemName, itemData.OutlineColor);
    }

    /// <summary>
    /// 体力回復用メソッド
    /// </summary>
    /// <param name="addValue">回復量の値を代入</param>
    private void AdditionNowHealth(float addValue)
    {
        if (addValue <= 0.0f)
        {
            return;
        }

        _nowHealth = Mathf.Min(_maxHealth, _nowHealth += addValue);
        _playerUIManager.SetValue(_nowHealth);
        _playerSEManager.PlayPickUpClip();
    }

    /// <summary>
    /// 最大体力増加用メソッド
    /// </summary>
    /// <param name="addValue">増加量を代入</param>
    private void AdditionMaxHealth(float addValue)
    {
        if(addValue <= 0.0f)
        {
            return;
        }

        if (_maxHealthValue > _maxHealth)
        {
            _playerUIManager.AddHealth(addValue);
        }

        _maxHealth = Mathf.Min(_maxHealthValue, _maxHealth + addValue);
        _playerUIManager.SetMaxValue(_maxHealth);
        _playerSEManager.PlayPickUpClip();
    }

    /// <summary>
    /// 攻撃力増加用メソッド
    /// </summary>
    /// <param name="addValue">増加量を代入</param>
    private void AdditionStrength(float addValue)
    {
        if (addValue <= 0.0f)
        {
            return;
        }

        if (_maxStrengthValue > _nowStrength)
        {
            _playerUIManager.AddStrength(addValue);
        }

        _nowStrength = Mathf.Min(_maxStrengthValue, _nowStrength + addValue);
        _playerSEManager.PlayPickUpClip();
    }

    /// <summary>
    /// 会心率増加用メソッド
    /// </summary>
    /// <param name="addValue">増加量を代入</param>
    private void AdditionCritical(float addValue)
    {
        if (addValue <= 0.0f)
        {
            return;
        }

        if (_maxCriticalValue > _nowCritical)
        {
            _playerUIManager.AddCritical(addValue);
        }

        _nowCritical = Mathf.Min(_maxCriticalValue, _nowCritical + addValue);
        _playerSEManager.PlayPickUpClip();
    }

    /// <summary>
    /// 移動速度増加用メソッド
    /// </summary>
    /// <param name="addValue">増加量を代入</param>
    private void AdditionSpeed(float addValue)
    {
        if (addValue <= 0.0f)
        {
            return;
        }

        if (_maxSpeedValue > _nowSpeed)
        {
            _playerUIManager.AddSpeed(addValue);
        }

        _nowSpeed = Mathf.Min(_maxSpeedValue, _nowSpeed + addValue);
        _playerSEManager.PlayPickUpClip();
    }

    //ステートチェンジ用メソッド群

    public void ToReady()
    {
        _nowAttackState = AttackState.Ready;
    }

    public void ToKnifeAttack()
    {
        _nowAttackState = AttackState.KnifeAttack;
    }

    public void ToDontAttack()
    {
        _nowAttackState = AttackState.DontAttack;
    }

    public void ToGameOver()
    {
        GameOverEvent.Invoke();
        _nowStatus = PlayerStatus.Death;
    }

    public void StateChange(PlayerStatus playerState)
    {
        if(_nowStatus == PlayerStatus.Death)
        {
            return;
        }

        _nowStatus = playerState;
    }
}