using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.UI;

/// <summary>
/// ゲート突入時の演出を管理
/// </summary>
public class IntoGateManager : Updater
{
    private enum FadeState
    {
        Wait,
        Fade,
    }

    [Header("【UI取得】")]
    [SerializeField] private CanvasGroup _intoGateCanvas = default;
    [SerializeField] private PlayableDirector _intoGateMovie = default;
    [SerializeField] private Image _fadeImage = default;

    [Header("【スクリプト取得】")]
    [SerializeField] private BGMManager _bgmManager = default;
    [SerializeField] private BaseGateEvent _baseGateEvent = default;
    [SerializeField] private PlayerCheckObject _playerCheckObject = default;
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private MainSceneManager _mainSceneManager = default;
    [SerializeField] private GateContentsManager _gateContentsManager = default;
    [SerializeField] private PauseManager _pauseManager = default;

    [Header("【設定用変数】")]
    [SerializeField] private float _fadeDuration = 2.0f;

    //作業用変数
    private float _timer = 0.0f;
    private FadeState _fadeState = FadeState.Wait;

    private void OnEnable()
    {
        _playerCheckObject.EnterTheGate += ShowCanvas;
        _gateContentsManager.IsSubmit += Choice;
        _playerState.OnDamageEvent += EndChoice;
    }

    private void OnDisable()
    {
        _playerCheckObject.EnterTheGate -= ShowCanvas;
        _gateContentsManager.IsSubmit -= Choice;
        _playerState.OnDamageEvent -= EndChoice;
    }

    /// <summary>
    /// UI表示用ムービーを再生
    /// </summary>
    private void ShowCanvas()
    {
        _intoGateMovie.Play();
        _intoGateCanvas.alpha = 1.0f;
        _playerState.StateChange(PlayerState.PlayerStatus.ContentsSelect);
    }

    /// <summary>
    /// 選択開始（ムービー側から操作）
    /// </summary>
    public void SelectStart()
    {
        _gateContentsManager.ToStart();
        _gateContentsManager.StartChoice();
    }

    /// <summary>
    /// ゲート突入か否か
    /// </summary>
    /// <param name="choice"></param>
    private void Choice(bool choice)
    {
        if (choice)
        {
            IntoGateStart();
        }
        else
        {
            EndChoice();
        }
    }

    /// <summary>
    /// フェードを開始するメソッド
    /// </summary>
    public void IntoGateStart()
    {
        _fadeImage.DOFade(1.0f, _fadeDuration);
        _bgmManager.BGMFadeOut();
        _fadeState = FadeState.Fade;
        _mainSceneManager.TimerStateChange(false);
        _gateContentsManager.EndChoice();
        _playerState.StateChange(PlayerState.PlayerStatus.Movie);
    }

    /// <summary>
    /// 選択を終了
    /// </summary>
    private void EndChoice()
    {
        _intoGateCanvas.alpha = 0.0f;
        _gateContentsManager.EndChoice();
        _playerState.StateChange(PlayerState.PlayerStatus.Active);
    }

    /// <summary>
    /// 毎フレーム実行されるメソッド
    /// </summary>
    public override void UpdateMethod()
    {
        if(_pauseManager.NowPauseState == PauseManager.PauseState.Pause)
        {
            return;
        }
        _gateContentsManager.UpdateMethod();
    }

    /// <summary>
    /// 一定間隔で実行されるメソッド
    /// </summary>
    public override void FixedUpdateMethod()
    {
        if(_fadeState == FadeState.Wait)
        {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer > _fadeDuration)
        {
            _baseGateEvent.IntoGateEvent();
            _intoGateCanvas.alpha = 0.0f;
            _fadeState = FadeState.Wait;
        }
    }
}
