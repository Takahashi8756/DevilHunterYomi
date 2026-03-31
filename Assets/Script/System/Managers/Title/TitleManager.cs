using UnityEngine;

/// <summary>
/// タイトル画面を操作するためのクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    private enum Select
    {
        Start,
        Option,
        Quit,
        Selected,
        SelectedOption,
    }

    [Header("【UI】")]
    [SerializeField] private GameObject _startButton = default;
    [SerializeField] private GameObject _optionButton = default;
    [SerializeField] private GameObject _quitButton = default;
    [SerializeField] private GameObject _titleUIs = default;

    [Header("【取得用変数】")]
    [SerializeField] private Animator _fadeAnimator = default;
    [SerializeField] private BGMManager _bgmManager = default;
    [SerializeField] private OptionManager _optionManager = default;
    [SerializeField] private TitleAudioManager _titleAudioManager = default;

    private Select _nowSelect = Select.Start;
    private bool _isInput = false;

    private const string INPUT_BUTTON_NAME = "Interact";
    private const string VERTICAL_NAME = "Vertical";
    private const int FRAME_RATE = 60;

    private void Start()
    {
        _nowSelect = Select.Start;
        _startButton.SetActive(true);
        _optionButton.SetActive(false);
        _quitButton.SetActive(false);

        _bgmManager.ToStart();
        _bgmManager.StartBGM();

        Application.targetFrameRate = FRAME_RATE;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float inputVertical = Input.GetAxisRaw(VERTICAL_NAME);

        if(inputVertical == 0)
        {
            _isInput = false;
        }

        if(inputVertical >= 1)
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

        if(choice)
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

    private void FixedUpdate()
    {
        _bgmManager.FixedUpdateMethod();
    }

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

        _titleAudioManager.PlayChoiceAudio();
    }

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

    private void ChoiceStart()
    {
        _nowSelect = Select.Selected;
        _startButton.SetActive(false);
        _optionButton.SetActive(false);
        _quitButton.SetActive(false);
        _fadeAnimator.SetTrigger("Fade");
        _bgmManager.BGMFadeOut();

        _titleAudioManager.PlayStartAudio();
    }

    private void ChoiceOption()
    {
        _nowSelect = Select.SelectedOption;
        _optionManager.OpenOption();
        _titleUIs.SetActive(false);
    }

    private void ChoiceQuit()
    {
        Application.Quit();
    }

    private void ReturnOption()
    {
        if(_nowSelect != Select.SelectedOption)
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
