using UnityEngine;

public class BGMManager : Updater
{
    private enum BGMState
    {
        Play,
        Stop,
        FadeOut,
    }

    [Header("【取得用変数】")]
    [SerializeField] private AudioSource _mainBGMSource = default;

    [Header("【調整用変数】")]
    [SerializeField] private float _fadeOutDuration = 3.0f;

    [Header("【流すBGM】")]
    [SerializeField] private AudioClip[] _BGMs = new AudioClip[1];

    private float _bgmVolume = 0.0f;
    private float _timer = 0.0f;
    private BGMState _bgmState = BGMState.Stop;

    public void ToStart()
    {
        _bgmVolume = _mainBGMSource.volume;
        SetBGM(0);
    }

    public override void FixedUpdateMethod()
    {
        switch (_bgmState)
        {
            case BGMState.FadeOut:

                FadeOut();

                break;
        }
    }

    public void SetBGM(int bgmIndex)
    {
        _mainBGMSource.clip = _BGMs[bgmIndex];
        _mainBGMSource.Play();
        _mainBGMSource.Pause();
    }

    public void StartBGM()
    {
        _mainBGMSource.volume = _bgmVolume;
        _mainBGMSource.Play();
        _mainBGMSource.time = 0.0f;
        _bgmState = BGMState.Play;
    }

    public void BGMFadeOut()
    {
        _bgmState = BGMState.FadeOut;
    }

    private void FadeOut()
    {
        if(_timer > _fadeOutDuration)
        {
            _timer = 0.0f;
            _mainBGMSource.Pause();
            _bgmState = BGMState.Stop;
            return;
        }

        float timer = _timer / _fadeOutDuration;
        timer = Mathf.SmoothStep(0, 1, timer);
        _mainBGMSource.volume = Mathf.Lerp(_bgmVolume, 0, timer);
        _timer += Time.unscaledDeltaTime;
    }
}
