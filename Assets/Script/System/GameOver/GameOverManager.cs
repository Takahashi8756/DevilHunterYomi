using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバーを管理するクラス
/// </summary>
public class GameOverManager : Updater
{
    [Header("【取得用変数】")]
    [SerializeField] PlayableDirector _gameOverTimeline = default;
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private BGMManager _bgmManager = default;
    [SerializeField] private GameOverSEManager _gameOverSEMamager = default;

    [Header("【設定用変数】")]
    [SerializeField] private float _slowTimeScale = 0.3f;

    private bool _isInteract = false;

    //定数
    private const string INTERACT_INPUT_NAME = "Interact";
    private const string TITLE_SCENE_NAME = "title";

    private void OnEnable()
    {
        _playerState.GameOverEvent += GameOver;
    }

    private void OnDisable()
    {
        _playerState.GameOverEvent -= GameOver;
    }

    /// <summary>
    /// ゲームオーバームービーを実行するメソッド
    /// </summary>
    private void GameOver()
    {
        _gameOverTimeline.Play();
        _bgmManager.BGMFadeOut();
        _gameOverSEMamager.PlayGameOverAudio();

        Time.timeScale = _slowTimeScale;
    }

    /// <summary>
    /// 毎フレーム実行されるメソッド
    /// </summary>
    public override void UpdateMethod()
    {
        if(!_isInteract)
        {
            return;
        }

        if (Input.GetButtonDown(INTERACT_INPUT_NAME))
        {
            SceneManager.LoadScene(TITLE_SCENE_NAME);
        }
    }

    /// <summary>
    /// タイムスケールを元に戻すメソッド：Timelineから実行
    /// </summary>
    public void ReturnTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// タイトルに戻れるようにするかどうかを変更するメソッド：Timelineから実行
    /// </summary>
    /// <param name="interact">可能にするかどうか</param>
    public void UpdateInteract(bool interact)
    {
        _isInteract = interact;
    }
}
