using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private PrologueTextManager _prologueTextManager = default;
    [SerializeField] private PrologueAudioManager _prologueAudioManager = default;
    [SerializeField] private PlayableDirector _playableDirector = default;
    [SerializeField] private Animator _skipFade = default;

    [Header("【シーン遷移までの時間】")]
    [SerializeField] private float _toMainSceneDuration = 4.0f;

    private bool _canSkip = false;
    private const string SKIP_INPUT_NAME = "Interact";
    private const string MAINGAMESCENE_NAME = "MainScene";
    private const string SKIPANIM_TRIGGER_NAME = "Skip";

    private void Update()
    {
        if (!_canSkip)
        {
            return;
        }

        if (Input.GetButtonDown(SKIP_INPUT_NAME))
        {
            StartCoroutine(ToMainGame());
        }
    }

    private void Start()
    {
        _prologueTextManager.ToReset();
    }

    public void ShowTextStart()
    {
        _prologueTextManager.StartShowText();
    }

    public void NextText()
    {
        _prologueTextManager.NextText();
    }

    public void CanSkipInputBoolChange(bool value)
    {
        _canSkip = value;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(MAINGAMESCENE_NAME);
    }

    private IEnumerator ToMainGame()
    {
        _canSkip = false;
        _playableDirector.Stop();
        _skipFade.SetTrigger(SKIPANIM_TRIGGER_NAME);
        _prologueAudioManager.PlaySkipAudio();

        yield return new WaitForSeconds(_toMainSceneDuration);
        SceneManager.LoadScene(MAINGAMESCENE_NAME);
    }
}
