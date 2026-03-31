using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Updater
{
    private enum Select
    {
        Start,
        Option,
        Quit,
        SelectedOption,
    }

    public enum PauseState
    {
        Wait,
        Pause,
    }

    [Header("【UI】")]
    [SerializeField] private GameObject _startButton = default;
    [SerializeField] private GameObject _optionButton = default;
    [SerializeField] private GameObject _quitButton = default;
    [SerializeField] private GameObject _titleUIs = default;
    [SerializeField] private CanvasGroup _pauseCanvas = default;

    [Header("【取得用変数】")]
    [SerializeField] private OptionManager _optionManager = default;
    [SerializeField] private TitleAudioManager _titleAudioManager = default;
    [SerializeField] private CameraMove _cameraMove = default;

    //作業用変数
    private Select _nowSelect = Select.Start;
    private PauseState _pauseState = PauseState.Wait;
    private bool _isInput = false;

    //定数
    private const string INPUT_BUTTON_NAME = "Interact";
    private const string VERTICAL_NAME = "Vertical";
    private const string TILESCENE_NAME = "Title";
    private const string MOUSE_SENSITIVITY_PREFS_KEY = "Sensitivity";
    private const int FRAME_RATE = 60;

    //プロパティ
    public PauseState NowPauseState => _pauseState;

    private void Start()
    {
        _nowSelect = Select.Start;
        _startButton.SetActive(true);
        _optionButton.SetActive(false);
        _quitButton.SetActive(false);

        Application.targetFrameRate = FRAME_RATE;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// 毎フレーム呼び出されるメソッド
    /// </summary>
    public override void UpdateMethod()
    {
        if(_pauseState == PauseState.Wait)
        {
            return;
        }

        float inputVertical = Input.GetAxisRaw(VERTICAL_NAME);

        if (inputVertical == 0)
        {
            _isInput = false;
        }

        if (inputVertical >= 1)
        {
            switch (_nowSelect)
            {
                case Select.Start:
                    SelectQuit();
                    break;

                case Select.Option:
                    SelectStart();
                    break;

                case Select.Quit:
                    SelectOption();
                    break;
            }
        }
        else if (inputVertical <= -1)
        {
            switch (_nowSelect)
            {
                case Select.Start:
                    SelectOption();
                    break;

                case Select.Option:
                    SelectQuit();
                    break;

                case Select.Quit:
                    SelectStart();
                    break;
            }
        }

        bool choice = Input.GetButtonDown(INPUT_BUTTON_NAME);

        if (choice)
        {
            switch (_nowSelect)
            {
                case Select.Start:
                    ChoiceStart();
                    break;

                case Select.Option:
                    ChoiceOption();
                    break;

                case Select.Quit:
                    ChoiceQuit();
                    break;
            }

            _titleAudioManager.PlaySubmitAudio();
        }

        ReturnOption();
    }

    /// <summary>
    /// PAUSEを開始するメソッド
    /// </summary>
    public void Pause()
    {
        _isInput = false;
        Time.timeScale = 0;
        _pauseCanvas.alpha = 1;
        _pauseState = PauseState.Pause;
    }

    /// <summary>
    /// 再開ボタンを選択した時のメソッド
    /// </summary>
    private void SelectStart()
    {
        if (_isInput)
        {
            return;
        }

        _nowSelect = Select.Start;
        _startButton.SetActive(true);
        _optionButton.SetActive(false);
        _quitButton.SetActive(false);
        _isInput = true;

        _cameraMove.UpdateSensitivity(PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_PREFS_KEY));
        _titleAudioManager.PlayChoiceAudio();
    }

    /// <summary>
    /// オプションボタンを選択した時のメソッド
    /// </summary>
    private void SelectOption()
    {
        if (_isInput)
        {
            return;
        }

        _nowSelect = Select.Option;
        _startButton.SetActive(false);
        _optionButton.SetActive(true);
        _quitButton.SetActive(false);
        _isInput = true;
        _titleAudioManager.PlayChoiceAudio();
    }

    /// <summary>
    /// ゲーム終了を選択した時のメソッド
    /// </summary>
    private void SelectQuit()
    {
        if (_isInput)
        {
            return;
        }

        _nowSelect = Select.Quit;
        _startButton.SetActive(false);
        _optionButton.SetActive(false);
        _quitButton.SetActive(true);
        _isInput = true;
        _titleAudioManager.PlayChoiceAudio();
    }

    /// <summary>
    /// 再開を実行した時のメソッド
    /// </summary>
    private void ChoiceStart()
    {
        _pauseCanvas.alpha = 0;
        Time.timeScale = 1;
        _pauseState = PauseState.Wait;
        _cameraMove.UpdateSensitivity(PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_PREFS_KEY));
    }

    /// <summary>
    /// オプションを実行した時のメソッド：オプション選択画面に移行
    /// </summary>
    private void ChoiceOption()
    {
        _nowSelect = Select.SelectedOption;
        _optionManager.OpenOption();
        _titleUIs.SetActive(false);
    }

    /// <summary>
    ///　ゲーム終了を実行した時のメソッド
    /// </summary>
    private void ChoiceQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(TILESCENE_NAME);
    }

    /// <summary>
    /// オプションから帰ってきた時のメソッド
    /// </summary>
    private void ReturnOption()
    {
        if (_nowSelect != Select.SelectedOption)
        {
            return;
        }

        if (!_optionManager.IsOption())
        {
            _nowSelect = Select.Start;
            _titleUIs.SetActive(true);
            _startButton.SetActive(true);
            _optionButton.SetActive(false);
            _quitButton.SetActive(false);

        }
    }
}
