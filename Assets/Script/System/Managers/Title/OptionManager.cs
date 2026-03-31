using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// オプション画面を操作するためのクラス
/// </summary>
public class OptionManager : Updater
{
    private enum OptionState
    {
        Wait,
        Active,
    }

    private enum Select
    {
        Volume,
        Sensitivity,
        Back,
    }
        
    [Header("【取得用変数】")]
    [SerializeField] private Slider _volumeSlider = default;
    [SerializeField] private Slider _sensitivitySlider = default;
    [SerializeField] private AudioMixer _audioMixer = default;
    [SerializeField] private GameObject _optionUIs = default;
    [SerializeField] private TitleAudioManager _titleAudioManager = default;

    [Header("【選択中UI】")]
    [SerializeField] private GameObject _volumeSelectUI = default;
    [SerializeField] private GameObject _sensitivitySelectUI = default;
    [SerializeField] private GameObject _backSelectUI = default;

    [Header("【数値調整用変数】")]
    [SerializeField] private float _volumeAddValue = 0.1f;
    [SerializeField] private float _sensitivityAddValue = 20.0f;
    [SerializeField] private float _sensitivityDefaultValue = 200.0f;

    private OptionState _nowState = OptionState.Wait;
    private Select _nowSelect = Select.Volume;
    private bool _isInput = false;
    private bool _isGageInput = false;

    private const string VOLUME_PREFS_KEY = "MasterVolume";
    private const string MOUSE_SENSITIVITY_PREFS_KEY = "Sensitivity";
    private const string INPUT_BUTTON_NAME = "Interact";
    private const string HORIZONTAL_NAME = "Horizontal";
    private const string VERTICAL_NAME = "Vertical";
    private const float MAX_SENSITIVITY_VALUE = 500.0f;
    private const float MIN_SENSITIVITY_VALUE = 50.0f;

    private void Awake()
    {
        if (PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_PREFS_KEY) == 0)
        {
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_PREFS_KEY, _sensitivityDefaultValue);
        }
    }

    private void Start()
    {
        _sensitivitySlider.maxValue = MAX_SENSITIVITY_VALUE;
        _sensitivitySlider.minValue = MIN_SENSITIVITY_VALUE;
        _isInput = false;

        _volumeSlider.value = PlayerPrefs.GetFloat(VOLUME_PREFS_KEY);
        _sensitivitySlider.value = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_PREFS_KEY);
        SetMixserValue(_volumeSlider.value);

    }

    private void Update()
    {
        if(_nowState == OptionState.Wait)
        {
            return;
        }

        float inputHorizontal = Input.GetAxisRaw(HORIZONTAL_NAME);
        float inputVertical = Input.GetAxisRaw(VERTICAL_NAME);
        bool inputInteract = Input.GetButtonDown(INPUT_BUTTON_NAME);

        switch (_nowSelect)
        {
            case Select.Volume:
                SelectedVolume(inputHorizontal, inputVertical);
                break;

            case Select.Sensitivity:
                SelectedSensitivity(inputHorizontal, inputVertical);
                break;

            case Select.Back:
                SelectedBack(inputVertical, inputInteract);
                break;
        }

        if (inputVertical == 0)
        {
            _isInput = false;
        }

        if(inputHorizontal == 0)
        {
            _isGageInput = false;
        }
    }

    public void OpenOption()
    {
        _nowState = OptionState.Active;
        _optionUIs.SetActive(true);
    }

    public bool IsOption()
    {
        return _nowState == OptionState.Active;
    }

    private void SelectedVolume(float horizontal, float vertical)
    {
        if (!_isInput)
        {
            if (vertical >= 1)
            {
                _nowSelect = Select.Back;
                _volumeSelectUI.SetActive(false);
                _sensitivitySelectUI.SetActive(false);
                _backSelectUI.SetActive(true);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
            else if (vertical <= -1)
            {
                _nowSelect = Select.Sensitivity;
                _volumeSelectUI.SetActive(false);
                _sensitivitySelectUI.SetActive(true);
                _backSelectUI.SetActive(false);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
        }

        if (!_isGageInput)
        {
            if (horizontal >= 1)
            {
                _volumeSlider.value += _volumeAddValue;
                SetMixserValue(_volumeSlider.value);
                _isGageInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
            else if (horizontal <= -1)
            {
                _volumeSlider.value -= _volumeAddValue;
                SetMixserValue(_volumeSlider.value);
                _isGageInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
        }
    }

    private void SelectedSensitivity(float horizontal, float vertical)
    {
        if (!_isInput)
        {
            if (vertical >= 1)
            {
                _nowSelect = Select.Volume;
                _volumeSelectUI.SetActive(true);
                _sensitivitySelectUI.SetActive(false);
                _backSelectUI.SetActive(false);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
            else if (vertical <= -1)
            {
                _nowSelect = Select.Back;
                _volumeSelectUI.SetActive(false);
                _sensitivitySelectUI.SetActive(false);
                _backSelectUI.SetActive(true);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
        }

        if (!_isGageInput)
        {
            if (horizontal >= 1)
            {
                _sensitivitySlider.value += _sensitivityAddValue;
                _isGageInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
            else if (horizontal <= -1)
            {
                _sensitivitySlider.value -= _sensitivityAddValue;
                _isGageInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
        }
    }

    private void SelectedBack(float vertical, bool interact)
    {
        if (interact)
        {
            PlayerPrefs.SetFloat(VOLUME_PREFS_KEY, _volumeSlider.value);
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_PREFS_KEY, _sensitivitySlider.value);

            _nowSelect = Select.Volume;
            _volumeSelectUI.SetActive(true);
            _sensitivitySelectUI.SetActive(false);
            _backSelectUI.SetActive(false);

            _optionUIs.SetActive(false);
            _nowState = OptionState.Wait;
            _titleAudioManager.PlaySubmitAudio();
            return;
        }

        if (!_isInput)
        {
            if (vertical >= 1)
            {
                _nowSelect = Select.Sensitivity;
                _volumeSelectUI.SetActive(false);
                _sensitivitySelectUI.SetActive(true);
                _backSelectUI.SetActive(false);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
            else if (vertical <= -1)
            {
                _nowSelect = Select.Volume;
                _volumeSelectUI.SetActive(true);
                _sensitivitySelectUI.SetActive(false);
                _backSelectUI.SetActive(false);
                _isInput = true;
                _titleAudioManager.PlayChoiceAudio();
            }
        }
    }

    private void SetMixserValue(float value)
    {
        float mixerValue = value;

        if(value > 0)
        {
            mixerValue *= 6;
        }
        else
        {
            mixerValue *= 80;
        }

        _audioMixer.SetFloat("BGM", mixerValue);
    }
}
