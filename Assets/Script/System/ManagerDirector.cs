using UnityEngine;

/// <summary>
/// マネージャーを格納するクラス
/// 【制作者：髙橋英士】
/// 【制作日時：2025/9/24】
/// </summary>
[System.Serializable]
public class Managers
{
    #region 【変数】

    [Header("【マネージャー】")]
    [SerializeField] private MainSceneManager _mainSceneManager = default;
    [SerializeField] private UpdateManager _updateManager = default;
    [SerializeField] private GameStateManager _gameStateManager = default;
    [SerializeField] private CreateDungeon _createDungeon = default;
    [SerializeField] private SpawnEnemy _spawnEnemy = default;

    #endregion

    #region 【プロパティ】

    public MainSceneManager MainSceneManager => _mainSceneManager;
    public UpdateManager UpdateManager => _updateManager;
    public GameStateManager GameStateManager => _gameStateManager;
    public CreateDungeon CreateDungeon => _createDungeon;
    public SpawnEnemy SpawnEnemy => _spawnEnemy;
    #endregion
}

/// <summary>
/// マネージャーを取得するためのクラス
/// 【制作者：髙橋英士】
/// </summary>
public class ManagerDirector : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [SerializeField, Tooltip("マネージャーをまとめて取得するもの")]
    private Managers _managers = default;

    public Managers Manager => _managers;
}
