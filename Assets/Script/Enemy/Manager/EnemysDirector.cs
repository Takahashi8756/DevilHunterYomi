using System.Collections.Generic;
using UnityEngine;

public class EnemysDirector : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【取得用変数】")]
    [SerializeField] private Transform _playerTransform = default;
    [SerializeField] private EnemyBulletPool _enemyBulletPool = default;
    [SerializeField] private ItemDirector _itemDirector = default;
    [SerializeField] private Transform _popDamageParent = default;

    [Header("【配列用変数】")]
    [SerializeField] private float _deleteElementDuration = 5.0f;
    [SerializeField] private List<EnemyManager> _enemyManagers = new List<EnemyManager>();

    private float _timer = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    private void Start()
    {
        foreach(EnemyManager enemyManager in _enemyManagers)
        {
            if (enemyManager == null)
            {
                continue;
            }

            enemyManager.SetValue(_playerTransform, _enemyBulletPool, _itemDirector, _popDamageParent);
            enemyManager.ToStart();
        }
    }

    /// <summary>
    /// リストに敵を格納するメソッド
    /// </summary>
    /// <param name="enemy">生成した敵</param>
    public void SetEnemy(GameObject enemy)
    {
        EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();

        if (enemyManager != null)
        {
            _enemyManagers.Add(enemyManager);
            enemyManager.SetValue(_playerTransform, _enemyBulletPool, _itemDirector, _popDamageParent);
            enemyManager.ToStart();
        }
    }

    public override void FixedUpdateMethod()
    {
        foreach(EnemyManager enemyManager in _enemyManagers)
        {
            if(enemyManager == null)
            {
                continue;
            }

            enemyManager.FixedUpdateMethod();
        }

        DeleteElement();
    }

    private void DeleteElement()
    {
        _timer += Time.deltaTime;

        if (_timer > _deleteElementDuration)
        {
            _timer = 0.0f;
            _enemyManagers.RemoveAll(EnemyManager => EnemyManager == null);
        }
    }
}
