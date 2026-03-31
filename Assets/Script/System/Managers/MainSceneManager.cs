using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// メインゲームシーンを管理する用のクラス
/// </summary>
public class MainSceneManager : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    private enum BossBattleState
    {
        Wait,
        BossActive,
        BossDead,
    }

    [Header("【タイムリミット用変数】")]
    [SerializeField] private float _timeLimit = 180.0f;
    [SerializeField] private float _bossTimeLimit = 120.0f;
    [SerializeField] private float _alertTimeLimit = 30.0f;
    [SerializeField] private Text _timeLimitText = default;
    [SerializeField] private Slider _timeLimitGage = default;
    [SerializeField] private Animator _timeLimitAlertAnim = default;

    [Header("【取得用変数】")]
    [SerializeField] private BGMManager _bgmManager = default;
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private GameClearDirection _gameClearDirection = default;

    [Header("【タイム計測】")]
    [SerializeField] private bool _isCountDown = true;

    //定数
    private const string TIME_PREFS_KEY = "TimeScore";
    private const string CLEAR_SCENE_NAME = "ClearScene";
    private const string ALERT_TRIGGER_NAME = "Alert";
    private const int FRAME_RATE = 60;

    //タイマー
    private float _timer = 0.0f;
    private float _scoreTimer = 0.0f;
    private bool _isCount = true;

    //enum
    private BossBattleState _battleState = BossBattleState.Wait;

    //プロパティ
    public float TimeLimit => _timeLimit;
    public float Timer => _timer;

    //========================================================================
    //メソッド
    //========================================================================

    //起動時、Startの前に一度だけ実行されるメソッド
    private void Awake()
    {
        //値の初期化
        _scoreTimer = 0.0f;
        _timer = _timeLimit;
        _timeLimitGage.maxValue = _timeLimit;
        _timeLimitGage.value = _timer;
        Application.targetFrameRate = FRAME_RATE;

        _bgmManager.ToStart();
        _bgmManager.StartBGM();
    }

    public override void FixedUpdateMethod()
    {
        if(!_isCountDown)
        {
            return;
        }

        if(_playerState.NowStatus != PlayerState.PlayerStatus.Active)
        {
            return;
        }

        if (!_isCount)
        {
            return;
        }

        TimeLimitCounter();
    }

    /// <summary>
    /// タイムリミットを計算するメソッド
    /// </summary>
    private void TimeLimitCounter()
    {
        if (_battleState == BossBattleState.BossDead)
        {
            return;
        }

        if (_timer <= 0.0f)
        {
            GameOver();
            return;
        }

        _timer -= Time.deltaTime;
        _scoreTimer += Time.deltaTime;
        float minute = (int)_timer / 60;
        float second = (int)_timer % 60;

        _timeLimitText.text = minute.ToString("00") + ":" + second.ToString("00");
        _timeLimitGage.value = _timer;
        _timeLimitAlertAnim.SetBool(ALERT_TRIGGER_NAME, _timer <= _alertTimeLimit);
    }

    /// <summary>
    /// ゲームオーバー用メソッド
    /// </summary>
    public void GameOver()
    {
        _playerState.ToGameOver();
    }

    public void GameClear()
    {
        PlayerPrefs.SetFloat(TIME_PREFS_KEY, _scoreTimer);
        _playerState.StateChange(PlayerState.PlayerStatus.Movie);
        _gameClearDirection.StartClearMovie();
        _gameClearDirection.OnEndMovie += LeaveToScene;
    }

    private void LeaveToScene()
    {
        _gameClearDirection.OnEndMovie -= LeaveToScene;
        SceneManager.LoadScene(CLEAR_SCENE_NAME);
    }

    /// <summary>
    /// タイマーをボス戦用にセットするメソッド
    /// </summary>
    public void BossTimerSet()
    {
        _timer = _bossTimeLimit;
        _scoreTimer = 0.0f;
        _timeLimitGage.maxValue = _bossTimeLimit;
        _timeLimitGage.value = _timer;
        _battleState = BossBattleState.BossActive;
    }

    public void TimerStateChange(bool count)
    {
        _isCount = count;
    }
}
