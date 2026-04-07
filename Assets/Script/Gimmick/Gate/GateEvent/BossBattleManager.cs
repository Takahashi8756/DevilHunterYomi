using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

/// <summary>
/// ボスバトル移行用＋ボスHP監視スクリプト
/// </summary>
public class BossBattleManager : BaseGateEvent
{
    [Header("【ボス用変数】")]
    [SerializeField] private GameObject _bossBlockObject = default;
    [SerializeField] private Transform _enemyParent = default;
    [SerializeField] private Transform _bossSpawnTransform = default;
    [SerializeField] private EnemysDirector _enemyDirector = default;

    [Header("【ムービー用変数】")]
    [SerializeField] private CanvasGroup _mainCanvas = default;
    [SerializeField] private CanvasGroup _movieCanvas = default;
    [SerializeField] private GameObject _movieCamera = default;
    [SerializeField] private PlayableDirector _bossMovieDirector = default;
    [SerializeField] private Transform _playerMovieTransform = default;
    [SerializeField] private Transform _playerRespawnPoint = default;

    [Header("【その他取得用変数】")]
    [SerializeField] private Transform _playerTransform = default;
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private MainSceneManager _mainSceneManager = default;
    [SerializeField] private BGMManager _bgmManager = default;

    //作業用変数
    private GameObject _bossObject = default;

    public override void IntoGateEvent()
    {
        //キャンバスを切り替え
        _mainCanvas.alpha = 0.0f;
        _movieCanvas.alpha = 1.0f;
        _movieCamera.SetActive(true);

        //プレイヤーを一旦安全な場所に移動
        _playerTransform.position = _playerMovieTransform.position;
        _playerState.StateChange(PlayerState.PlayerStatus.Movie);

        _bossMovieDirector.Play();
    }

    /// <summary>
    /// ボスを生成するメソッド
    /// </summary>
    public void SpawnBoss()
    {
        _bossObject = Instantiate(_bossBlockObject, _enemyParent);
        UnityEngine.AI.NavMeshAgent agent = _bossObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.Warp(_bossSpawnTransform.position);
        _bossObject.GetComponent<EnemyState>().EnemyDeathEvent += BossKilled;
        _enemyDirector.SetEnemy(_bossObject);
    }

    /// <summary>
    /// ムービー終了時実行するメソッド
    /// </summary>
    public void EndMovie()
    {
        //キャンバスを切り替え
        _movieCamera.SetActive(false);
        _mainCanvas.alpha = 1.0f;
        _movieCanvas.DOFade(0.0f, 1.0f);

        //プレイヤーを移動
        _playerTransform.position = _playerRespawnPoint.position;
        _playerTransform.rotation = _playerRespawnPoint.rotation;

        //戦闘開始
        _mainSceneManager.BossTimerSet();
        _mainSceneManager.TimerStateChange(true);
        _bgmManager.SetBGM(1);
        _bgmManager.StartBGM();
        _playerState.StateChange(PlayerState.PlayerStatus.Active);
    }

    /// <summary>
    /// ボスが死亡した際に呼び出すメソッド
    /// </summary>
    private void BossKilled()
    {
        _bossObject.GetComponent<EnemyState>().EnemyDeathEvent -= BossKilled;
        _mainSceneManager.GameClear();
    }
}
