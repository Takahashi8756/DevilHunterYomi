using System;
using UnityEngine;

public class GateContentsManager : MonoBehaviour
{
    public enum Select
    {
        Submit,
        Disable,
    }

    public enum State
    {
        Wait,
        Active,
    }

    [Header("【コンテンツ】")]
    [SerializeField] private GameObject _submitUI = default;
    [SerializeField] private GameObject _disableUI = default;

    [Header("【取得用変数】")]
    [SerializeField] private TitleAudioManager _titleAudioManager = default;

    //作業用変数
    private bool _isInput = false;
    private Select _nowSelect = Select.Disable;
    private State _nowState = State.Wait;

    //イベント
    public event Action<bool> IsSubmit = default;

    //定数
    private const string INPUT_BUTTON_NAME = "Interact";
    private const string HORIZONTAL_NAME = "Horizontal";

    public void ToStart()
    {
        _isInput = false;
        _submitUI.SetActive(false);
        _disableUI.SetActive(true);
        _nowSelect = Select.Disable;
        _nowState = State.Wait;
    }

    public void StartChoice()
    {
        _nowState = State.Active;
    }

    public void EndChoice()
    {
        _nowState = State.Wait;
    }

    public void UpdateMethod()
    {
        if(_nowState != State.Active)
        {
            return;
        }

        float inputHorizontal = Input.GetAxisRaw(HORIZONTAL_NAME);
        bool inputInteract = Input.GetButtonDown(INPUT_BUTTON_NAME);

        if(inputHorizontal == 0.0f)
        {
            _isInput = false;
        }

        if(inputHorizontal >= 1.0f || inputHorizontal <= -1.0f)
        {
            if (!_isInput)
            {
                switch (_nowSelect)
                {
                    case Select.Submit:

                        _isInput = true;
                        _submitUI.SetActive(false);
                        _disableUI.SetActive(true);
                        _nowSelect = Select.Disable;

                        break;

                    case Select.Disable:

                        _isInput = true;
                        _submitUI.SetActive(true);
                        _disableUI.SetActive(false);
                        _nowSelect = Select.Submit;

                        break;
                }

                _titleAudioManager.PlayChoiceAudio();
            }
        }

        if (inputInteract)
        {
            switch (_nowSelect)
            {
                case Select.Submit:
                    IsSubmit?.Invoke(true);
                    _titleAudioManager.PlayStartAudio();
                    break;

                case Select.Disable:
                    IsSubmit?.Invoke(false);
                    _titleAudioManager.PlaySubmitAudio();
                    break;
            }
        }
    }
}
