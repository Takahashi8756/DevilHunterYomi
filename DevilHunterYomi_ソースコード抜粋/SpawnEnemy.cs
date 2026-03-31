using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の出現を管理するクラス
/// </summary>
public class SpawnEnemy : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    public enum EnemySpawnerState
    {
        Wait,
        Active,
        Interval,
    }

    [System.Serializable]
    private struct SpawnEnemyStruct
    {
        public GameObject _spawnEnemy;
        public float _spawnRate;
    }

    [Header("【敵出現用変数】")]
    [SerializeField] private float _enemySpawnInterval = 30.0f;
    [SerializeField] private int _spawnRate = 40;
    [SerializeField] private Transform _enemyParent = default;

    [Header("【敵の種類】")]
    [SerializeField] private List<SpawnEnemyStruct> _spawnEnemyData = default;

    [Header("【取得用変数】")]
    [SerializeField] private EnemysDirector _enemysDirector = default;

    //スポナーのステート
    private EnemySpawnerState _spawnerState = EnemySpawnerState.Wait;

    //その他作業用変数
    private int[,] _mapData = new int[0,0];
    private float _timer = 0.0f;
    private float _nowInterval = 0.0f;
    private float _setEnemyDistance = 0.0f;
    private Vector3 _basePosition = Vector3.zero;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 敵の生成を開始するメソッド
    /// </summary>
    /// <param name="mapData">マップの配列</param>
    /// <param name="distance">敵同士の距離</param>
    /// <param name="basePosition">ベースのポジション</param>
    public void SpawnStart(int[,] mapData, float distance, Vector3 basePosition)
    {
        _mapData = mapData;
        _setEnemyDistance = distance;
        _basePosition = basePosition;
        _nowInterval = _enemySpawnInterval;
        _spawnerState = EnemySpawnerState.Active;
    }

    /// <summary>
    /// 敵の自動スポーンを管理するメソッド
    /// </summary>
    public void SpawnUpdater()
    {
        switch (_spawnerState)
        {
            case EnemySpawnerState.Active:

                _mapData = Spawn(_mapData);
                RandomSpawn(_mapData);

                break;

            case EnemySpawnerState.Interval:

                Interval();

                break;
        }
    }

    /// <summary>
    /// 配列内に敵を生成するメソッド
    /// </summary>
    /// <param name="mapData">マップの配列データ</param>
    /// <returns></returns>
    private int[,] Spawn(int[,] mapData)
    {
        int[,] nowMap = mapData;

        for(int y = 0; y < nowMap.GetLength(1); y++)
        {
            for(int x = 0; x < nowMap.GetLength(0); x++)
            {
                if (x <= 4 && y <= 4)
                {
                    continue;
                }

                int randomIndex = Random.Range(1, 100);

                if(randomIndex >= _spawnRate)
                {
                    continue;
                }

                //既に敵を生成した場所には敵を生成しない
                if(nowMap[x, y] == 0)
                {
                    nowMap[x, y] = 5;
                    break;
                }

                if (nowMap[x, y] == 5)
                {
                    nowMap[x, y] = 0;
                }
            }
        }

        _spawnerState = EnemySpawnerState.Interval;
        return nowMap;
    }

    /// <summary>
    /// 配列の指定の位置に敵を生成するメソッド
    /// </summary>
    /// <param name="mapData">マップの配列</param>
    private void RandomSpawn(int[,] mapData)
    {
        for(int y = 0; y < mapData.GetLength(1); y++)
        {
            for(int x = 0; x < mapData.GetLength(0); x++)
            {

                if (mapData[x, y] == 5)
                {
                    Vector3 enemyPosition = new Vector3(_basePosition.x + (_setEnemyDistance * x), _setEnemyDistance, _basePosition.z + (_setEnemyDistance * y));
                    Spawn(enemyPosition);
                }
            }
        }
    }

    /// <summary>
    /// 与えられたポジションに、ランダムな敵を出現させるメソッド
    /// </summary>
    /// <param name="position">出現地点</param>
    private void Spawn(Vector3 position)
    {
        float rate = 0;
        for (int i = 0; i < _spawnEnemyData.Count; i++)
        {
            rate += _spawnEnemyData[i]._spawnRate;
        }

        if(rate <= 0)
        {
            return;
        }

        float choiceEnemy = Random.Range(0, rate);
        float sumRate = 0.0f;
        for (int i = 0; i < _spawnEnemyData.Count; i++)
        {
            sumRate += _spawnEnemyData[i]._spawnRate;
            if(choiceEnemy <= sumRate)
            {
                GameObject enemy = Instantiate(_spawnEnemyData[i]._spawnEnemy, position, Quaternion.identity, _enemyParent);
                _enemysDirector.SetEnemy(enemy);
                break;
            }
        }
    }

    /// <summary>
    /// 敵ランダムスポーンのインターバル管理メソッド
    /// </summary>
    private void Interval()
    {
        if(_timer > _nowInterval)
        {
            _timer = 0.0f;
            _spawnerState = EnemySpawnerState.Active;
            return;
        }

        _timer += Time.deltaTime;
    }

    /// <summary>
    /// 生成を終了させるメソッド
    /// </summary>
    public void SpawnEnd()
    {
        _spawnerState = EnemySpawnerState.Wait;
    }
}
