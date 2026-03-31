using System;
using UnityEngine;

/// <summary>
/// 敵のもつステータスを管理するクラス
/// 【制作日時：2025/10/20】
/// </summary>
public class EnemyState : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【enum】---//

    public enum EnemyStatus
    {
        Spawn,
        Active,
        Death,
    }

    public EnemyStatus _nowState = EnemyStatus.Spawn;

    //---【変数】---//

    [Header("【ステータス(初期値)】")]
    [SerializeField] private float _enemyHealth = 100.0f;
    [SerializeField] private float _enemyStrength = 10.0f;

    [Header("【取得用変数】")]
    [SerializeField] private EnemyVibration _enemyVibration = default;
    [SerializeField] private EnemyKnockBack _enemyKnockBack = default;
    [SerializeField] private EnemyFlash _enemyFlash = default;
    [SerializeField] private EnemySoundManager _enemySoundManager = default;
    [SerializeField] private PopDamageManager _popDamageManager = default;
    [SerializeField] private EnemyPlayerCheck _enemyPlayerCheck = default;
    [SerializeField] private Collider _enemyCollider = default;
    [SerializeField] private EnemyDamageUIManager _enemyUIManager = default;

    [Header("【その他変数】")]
    [SerializeField] private float _knockBackDamage = 100.0f;

    //敵のHPを格納する変数
    private float _nowEnemyHealth = 999.0f;

    //イベント
    public event Action EnemyDeathEvent = default;

    public float EnemyHealth => _nowEnemyHealth;
    public float EnemyStrength => _enemyStrength;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        //値の初期化
        _nowEnemyHealth = _enemyHealth;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damage">与えたいダメージ量を代入</param>
    public void Damage(float damage, float critical, float magnification)
    {
        switch (_nowState)
        {
            //生存中の処理
            case EnemyStatus.Active:

                //ランダム関数で0から100までの値を生成
                float random = UnityEngine.Random.Range(0.0f, 100.0f);
                bool isCritical = false;

                //ランダム関数の値がクリティカル率以下なら、クリティカルダメージを適用
                if(random <= critical)
                {
                    damage *= magnification;

                    isCritical = true;
                }

                KnockBack(damage);
                
                //HPをダメージ分減算する。
                _nowEnemyHealth -= damage;

                //敵を振動させリアクションを分かりやすく
                _enemyVibration.StartVibration(isCritical);
                _popDamageManager.PopDamage(damage, isCritical);
                _enemyFlash.Damage();
                _enemyPlayerCheck.DamageToAlertMode();
                _enemyUIManager.UpdateSlider(_nowEnemyHealth);

                if (isCritical)
                {
                    _enemySoundManager.PlayCriticalAudio();
                }
                else
                {
                    _enemySoundManager.PlayDamageAudio();
                }

                //HPが0以下なら死亡判定
                if (_nowEnemyHealth <= 0.0f)
                {
                    _enemyCollider.enabled = false;
                    EnemyDeathEvent?.Invoke();
                    _nowState = EnemyStatus.Death;
                }

                break;

            case EnemyStatus.Death:
                break;
        }
    }

    private void KnockBack(float damage)
    {
        if(damage >= _knockBackDamage)
        {
            _enemyKnockBack.StartKnockBack();
        }
    }

    public void ActiveEnemy()
    {
        if(_nowState != EnemyStatus.Spawn)
        {
            return;
        }

        _nowState = EnemyStatus.Active;
    }
}
