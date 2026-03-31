using UnityEngine;

/// <summary>
/// ダンジョンを管理するクラス
/// </summary>
public class DungeonManager : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private CreateDungeon _createDungeon = default;
    [SerializeField] private SpawnEnemy _spawnEnemy = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行されるメソッド
    /// </summary>
    private void Start()
    {
        int[,] mapData = _createDungeon.Create();
        float distance = _createDungeon.SetBlockDistance;
        Vector3 setBlockPosition = _createDungeon.CreateTransform.position;

        _spawnEnemy.SpawnStart(mapData, distance, setBlockPosition);
    }

    /// <summary>
    /// 一定間隔で実行されるメソッド
    /// </summary>
    public override void FixedUpdateMethod()
    {
        _spawnEnemy.SpawnUpdater();
    }
}
