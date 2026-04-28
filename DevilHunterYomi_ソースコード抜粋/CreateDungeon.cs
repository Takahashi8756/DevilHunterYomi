using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
/// ダンジョンを作成するクラス
/// </summary>
public class CreateDungeon : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【ダンジョン作成用変数】")]
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private int _wallHeight = 2;
    [SerializeField] private int _numberOfTreasure = 3;
    [SerializeField] private float _setBlockDistance = 3.0f;

    [Header("【プレハブ取得用変数】")]
    [SerializeField] private GameObject _groundPrefab = default;
    [SerializeField] private GameObject _WallPrefab = default;
    [SerializeField] private GameObject _treasureBoxPrefab = default;
    [SerializeField] private GameObject _gatePrefab = default;

    [Header("【その他取得用変数】")]
    [SerializeField] private Transform _createTransForm = default;
    [SerializeField] private Transform _groundParent = default;
    [SerializeField] private Transform _wallParent = default;
    [SerializeField] private Transform _tresureParent = default;
    [SerializeField] private Transform _gateParent = default;
    [SerializeField] private ItemDirector _itemDirector = default;
    [SerializeField] private NavMeshSurface _navMeshSurface = default;
    [SerializeField] private GimmickManager _gimmickManager = default;

    //マップデータ格納用配列
    private int[,] _mapData = new int[0, 0];

    //定数
    private const int PATH_INDEX = 0;
    private const int WALL_INDEX = 1;
    private const int TRESURE_INDEX = 2;
    private const int GATE_INDEX = 3;
    private const int MAX_RANDOM_RANGE = 5;
    private const int SET_TRESURE_INDEX = 3;

    //プロパティ
    public int[,] MapData => _mapData;
    public float SetBlockDistance => _setBlockDistance;
    public Transform CreateTransform => _createTransForm;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 指定の位置にダンジョンを生成するメソッド
    /// </summary>
    public int[,] Create()
    {
        MapDelete();
        _mapData = ResetMapData();
        _mapData = CreateMaze(_mapData);
        _mapData = SetTreasureBox(_mapData);
        _mapData = SetGate(_mapData);
        SetBlocks(_mapData);

        return _mapData;
    }

    /// <summary>
    /// マップデータを初期化するメソッド
    /// </summary>
    /// <returns>_width、_hightの分拡張されたマップデータの二次元配列を返す</returns>
    private int[,] ResetMapData()
    {
        int[,] map = new int[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                if (y == 0 || y == _height - 1)
                {
                    map[x, y] = WALL_INDEX;
                }
                else if(x == 0 || x == _width - 1)
                {
                    map[x, y] = WALL_INDEX;
                }
                else if(x % 2 == 0 && y % 2 == 0)
                {
                    map[x, y] = WALL_INDEX;
                }
                else
                {
                    map[x, y] = PATH_INDEX;
                }
            }
        }

        return map;
    }

    /// <summary>
    /// 迷路を作成するメソッド
    /// </summary>
    /// <param name="mapData">初期化されたmapDataを代入</param>
    /// <returns>マップ内の</returns>
    private int[,] CreateMaze(int[,] mapData)
    {
        int[,] maze = mapData;
        int randomIndex = 0;

        for(int y = 0; y < _height; y++)
        {
            if(y <= 1 || y >= _height - 2)
            {
                continue;
            }

            for(int x = 0; x < _width; x++)
            {
                if(x % 2 == 0 && y % 2 == 0)
                {
                    if(x == 0 || x == _width - 1)
                    {
                        continue;
                    }

                    if(y <= 2)
                    {
                        randomIndex = Random.Range(0, MAX_RANDOM_RANGE);
                    }
                    else
                    {
                        randomIndex = Random.Range(1, MAX_RANDOM_RANGE);
                    }

                    if (randomIndex == 0)
                    {
                        maze[x, y - 1] = WALL_INDEX;
                    }
                    else if(randomIndex == 1)
                    {
                        maze[x + 1, y] = WALL_INDEX;
                    }
                    else if(randomIndex == 2)
                    {
                        maze[x, y + 1] = WALL_INDEX;
                    }
                    else if(randomIndex == 3)
                    {
                        maze[x - 1, y] = WALL_INDEX;
                    }
                    else if(randomIndex == 4)
                    {
                        maze[x, y] = PATH_INDEX;
                    }
                }
            }
        }

        return maze;
    }

    /// <summary>
    /// 行き止まりに宝箱を配置するメソッド
    /// </summary>
    /// <param name="mapData">迷路を作成した後のmapDataを代入</param>
    /// <returns>_numberOfTresureの分だけ宝箱を配置したmapDataを返す</returns>
    private int[,] SetTreasureBox(int[,] mapData)
    {
        int[,] treasureMaze = mapData;
        int number = _numberOfTreasure;

        for (int y = 0; y < _height; y++)
        {
            if (y == 0 || y == _height - 1)
            {
                continue;
            }

            for (int x = 0; x < _width; x++)
            {
                if(x == 1 || y == 1)
                {
                    continue;
                }

                if (x == 0 || x == _width - 1)
                {
                    continue;
                }

                if (treasureMaze[x, y] == WALL_INDEX)
                {
                    continue;
                }

                int index = 0;

                if (treasureMaze[x, y - 1] == WALL_INDEX)
                {
                    index++;
                }

                if (treasureMaze[x + 1, y] == WALL_INDEX)
                {
                    index++;
                }

                if (treasureMaze[x, y + 1] == WALL_INDEX)
                {
                    index++;
                }

                if (treasureMaze[x - 1, y] == WALL_INDEX)
                {
                    index++;
                }

                if(index == SET_TRESURE_INDEX && number > 0)
                {
                    treasureMaze[x, y] = TRESURE_INDEX;
                    number--;
                }
            }
        }

        return treasureMaze;
    }

    /// <summary>
    /// ダンジョンの出口を配置するメソッド
    /// </summary>
    /// <param name="mapData"></param>
    /// <returns></returns>
    private int[,] SetGate(int[,] mapData)
    {
        int[,] setGateMap = mapData;
        int rate = _height * _width;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                int randomIndex = Random.Range(0, rate);

                if (setGateMap[x,y] == PATH_INDEX && randomIndex == 0)
                {
                    setGateMap[x,y] = GATE_INDEX;
                    return setGateMap;
                }

                rate--;
            }
        }

        setGateMap[_width - 2, _height - 2] = GATE_INDEX;
        return setGateMap;
    }

    /// <summary>
    /// mapData内の要素に応じ、実際にブロックを配置するメソッド
    /// </summary>
    /// <param name="mapData">最終的なmapDataを代入する。</param>
    private void SetBlocks(int[,] mapData)
    {
        Vector3 basePosition = _createTransForm.position;

        for(int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                Vector3 blockPosition = new Vector3(basePosition.x + (x * _setBlockDistance), 0, basePosition.z + (y * _setBlockDistance));
                Instantiate(_groundPrefab, blockPosition, Quaternion.identity, _groundParent);

                Vector3 roofPosition = blockPosition + Vector3.up * (_setBlockDistance * (_wallHeight + 1));
                Instantiate(_groundPrefab, roofPosition, Quaternion.identity, _groundParent);

                if (mapData[x, y] == WALL_INDEX)
                {
                    for(int i = 1;  i <= _wallHeight; i++)
                    {
                        Vector3 wallPosition = blockPosition + (Vector3.up * _setBlockDistance) * i;
                        Instantiate(_WallPrefab, wallPosition, Quaternion.identity, _wallParent);
                    }
                }
                else if(mapData[x, y] == TRESURE_INDEX)
                {
                    Vector3 tresurePosition = blockPosition + Vector3.up * _setBlockDistance;
                    GameObject tresure = Instantiate(_treasureBoxPrefab, tresurePosition, Quaternion.identity, _tresureParent);
                    tresure.GetComponentInChildren<TresureDropItem>().ToStart(_itemDirector);
                    _gimmickManager.SetGimmick(tresure.GetComponentInChildren<BaseGimmick>());
                }
                else if (mapData[x,y] == GATE_INDEX)
                {
                    Vector3 gatePosition = blockPosition + Vector3.up * _setBlockDistance;
                    GameObject gate = Instantiate(_gatePrefab, gatePosition, Quaternion.identity, _gateParent);
                    _gimmickManager.SetGate(gate.GetComponentInChildren<GateGimmick>());
                }
            }
        }

        _navMeshSurface.BuildNavMesh();
    }

    /// <summary>
    /// マップをすべて削除するメソッド
    /// </summary>
    private void MapDelete()
    {
        foreach(Transform child in _groundParent)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in _wallParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _tresureParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _gateParent)
        {
            Destroy(child.gameObject);
        }
    }
}
