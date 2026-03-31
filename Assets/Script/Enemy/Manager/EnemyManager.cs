using UnityEngine;

/// <summary>
/// 敵のスクリプトを管理するクラス
/// </summary>
public class EnemyManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【取得用変数】")]
    [SerializeField] private EnemyDirection _enemyDirection = default;
    [SerializeField] private EnemyVibration _enemyVibration = default;
    [SerializeField] private EnemyState _enemyState = default;
    [SerializeField] private EnemyPlayerCheck _enemyPlayerCheck = default;
    [SerializeField] private EnemyMove _enemyMove = default;
    [SerializeField] private EnemyAttack _enemyAttack = default;
    [SerializeField] private EnemyDeath _enemyDeath = default;
    [SerializeField] private PopDamageManager _popDamageManager = default;
    [SerializeField] private EnemyDamageUIManager _enemyUIManager = default;
    [SerializeField] private EnemySpawnManager _enemySpawnManager = default;

    //その他変数
    private Transform _playerPosition = default;
    private EnemyBulletPool _enemyBulletPool = default;
    private ItemDirector _itemDirector = default;
    private Transform _popDamageParent = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行されるメソッドを束ねたもの
    /// </summary>
    public void ToStart()
    {
        _enemyState.ToStart();
        _enemyAttack.ToStart(_enemyBulletPool);
        _enemyDeath.ToStart(_itemDirector);
        _popDamageManager.Tostart(_popDamageParent);
        _enemyMove.ToStart();
        _enemyUIManager.ToStart(_enemyState.EnemyHealth);
    }

    /// <summary>
    /// 取得したいコンポーネントを外部からセットするメソッド
    /// </summary>
    /// <param name="player">プレイヤーの位置（transform）</param>
    /// <param name="bulletPool">弾のオブジェクトプール（EnemyBulletPool）</param>
    public void SetValue(Transform player, EnemyBulletPool bulletPool, ItemDirector itemDirector, Transform popDamageParent)
    {
        _playerPosition = player;
        _enemyBulletPool = bulletPool;
        _itemDirector = itemDirector;
        _popDamageParent = popDamageParent;

        if(_enemySpawnManager != null)
        {
            _enemySpawnManager.SpawnEnemy();
        }
    }

    /// <summary>
    /// FixedUpdateにて実行されるメソッド
    /// </summary>
    public void FixedUpdateMethod()
    {
        bool playerCheck = false;
        _enemyDirection.DirectionUpdater(_playerPosition);

        switch (_enemyState._nowState)
        {
            case EnemyState.EnemyStatus.Spawn:
                return;

            case EnemyState.EnemyStatus.Active:

                playerCheck = _enemyPlayerCheck.PlayerCheck();

                break;

            case EnemyState.EnemyStatus.Death:

                _enemyDeath.ToDeathUpdate();

                break;
        }

        _enemyVibration.Vibration();
        _enemyMove.Move(playerCheck, _playerPosition);
        _enemyAttack.Attack(playerCheck);
    }
}
