using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーの近接攻撃を管理するクラス
/// 【制作日時：2025/10/27】
/// </summary>
public class KnifeAttack : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【enum】---//

    private enum AttackState
    {
        Ready,
        Cooldown,
        Wait,
    }

    private AttackState _attackState = AttackState.Ready;

    //---【変数】---//

    [Header("【攻撃用変数】")]
    [SerializeField] private float _attackMagnification = 3.0f;
    [SerializeField] private float _attackCoolTime = 1.5f;

    [Header("【BoxCast作成用変数】")]
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(0.1f, 0.1f, 0.5f);
    [SerializeField] private Vector3 _boxCenterOffset = new Vector3(0, -0.5f, 0);
    [SerializeField] private float _maxDistance = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;
    [SerializeField] private PlayerEffectManager _playerEffectManager = default;
    [SerializeField] private Transform _rangeTransform = default;

    //計算用変数
    private float _timer = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行されるメソッド：Startメソッドで実行
    /// </summary>
    public void ToStart()
    {
        _timer = 0.0f;
    }

    /// <summary>
    /// アタックの入力を検知し、処理を実行するメソッド
    /// </summary>
    public void InputAttack(PlayerInputManager input)
    {
        //判定内にいる敵を取得する。
        EnemyState[] hitEnemy = HitObjectCheck();

        //switchで処理を変更
        switch (_attackState)
        {
            case AttackState.Ready:

                Attack(hitEnemy, input.IsAttack);

                break;
        }
    }

    /// <summary>
    /// 範囲内の敵にダメージを与える
    /// </summary>
    /// <param name="enemys">範囲内の全ての敵を格納した配列を代入</param>
    /// <param name="inputButton">ボタンが入力されたかどうかの判定を代入</param>
    public void Attack(EnemyState[] enemys, bool inputButton)
    {
        if(_playerState.NowAttackState == PlayerState.AttackState.DontAttack)
        {
            return;
        }

        if (inputButton)
        {
            _playerEffectManager.Slash();
            _attackState = AttackState.Cooldown;
            _playerState.ToKnifeAttack();
            _playerUIManager.ToCoolDown();
            _playerSEManager.PlayKnifeAttackClip();

            if (enemys.Length <= 0)
            {
                return;
            }

            //ダメージ（近接ボーナス上乗せ）、クリティカル率、クリティカル倍率を取得
            float damage = _playerState.NowStrength * _attackMagnification;
            float critical = _playerState.NowCritical;
            float magnifaction = _playerState.NowMagnification;

            //判定内の全ての敵にダメージを与える
            foreach (EnemyState enemy in enemys)
            {
                enemy.Damage(damage, critical, magnifaction);
            }

            _playerUIManager.HitUI();
        }
    }

    /// <summary>
    /// 判定内にいる敵を取得するメソッド：BoxCastにて実装
    /// </summary>
    /// <returns>ヒットしている敵のEnemyStateを配列として全て返す</returns>
    private EnemyState[] HitObjectCheck()
    {
        //現在のポジションからボックスの始点、向きなどを計算
        Vector3 center = _rangeTransform.position + Vector3.up * _boxCenterOffset.y + _boxCenterOffset;
        Vector3 forward = _rangeTransform.forward;
        Quaternion rotation = _rangeTransform.rotation;

        //ぶつかっているオブジェクトを全て取得
        RaycastHit[] hitEnemys = Physics.BoxCastAll(center, _boxHalfExtents, forward, rotation, _maxDistance);

        //EnemyStateを格納するリストを作成
        List<EnemyState> enemyList = new List<EnemyState>();

        //配列内の、EnemyStateをもっているオブジェクトのスクリプトを取得
        foreach (RaycastHit enemy in hitEnemys)
        {
            EnemyState enemyState = enemy.collider.gameObject.GetComponent<EnemyState>();

            if (enemyState != null)
            {
                enemyList.Add(enemyState);
            }
        }

        //結果をListに格納
        EnemyState[] enemys = enemyList.ToArray();

        return enemys;
    }

    /// <summary>
    /// 攻撃のクールダウンを管理するメソッド：FixedUpdateにて実行
    /// </summary>
    public void AttackCoolDown()
    {
        switch (_attackState)
        {

            case AttackState.Cooldown:

                _timer += Time.deltaTime;

                CoolDownStateChange();

                break;
        }
    }

    /// <summary>
    /// クールダウンのタイマーの値が一定以上になったらステートを変えるメソッド
    /// </summary>
    private void CoolDownStateChange()
    {
        if (_timer > _attackCoolTime)
        {
            _timer = 0.0f;
            _attackState = AttackState.Ready;
            _playerState.ToReady();
            _playerUIManager.ToActive();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // プレイヤーの中心位置を計算
        Vector3 center = _rangeTransform.position + Vector3.up * _boxHalfExtents.y + _boxCenterOffset;

        // ワイヤーフレームでBoxを描画
        Gizmos.DrawWireCube(center, _boxHalfExtents * 2);
    }
#endif
}
