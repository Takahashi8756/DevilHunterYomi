using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Text _scoreText = default;
    [SerializeField] private Text _highScoreText = default;
    [SerializeField] private Text _highScoreTelop = default;

    private const string TIME_PREFS_KEY = "TimeScore";
    private const string HIGHSCORE_PREFS_KEY = "HighScore";
    private const string INTERACT_INPUTNAME = "Interact";
    private const string TITLE_SCENENAME = "title";

    private const float PROVISIONAL_SCORE = 999.9f;

    /// <summary>
    /// 生成時実行：PlayerPrefsからタイムを取得し表示する。
    /// </summary>
    private void Start()
    {
        float timeScore = PlayerPrefs.GetFloat(TIME_PREFS_KEY);
        float highScore = PlayerPrefs.GetFloat(HIGHSCORE_PREFS_KEY);

        if(highScore <= 0.0f)
        {
            highScore = PROVISIONAL_SCORE;
        }

        if(timeScore < highScore)
        {
            highScore = timeScore;
            _highScoreTelop.text = "HighScore!";
            PlayerPrefs.SetFloat(HIGHSCORE_PREFS_KEY, highScore);
        }

        float minute = (int)timeScore / 60;
        float second = (int)timeScore % 60;
        _scoreText.text = "ClearTime = " + minute.ToString("00") + ":" + second.ToString("00");

        minute = (int)highScore / 60;
        second = (int)highScore % 60;
        _highScoreText.text = "HighScore = " + minute.ToString("00") + ":" + second.ToString("00");
    }

    private void Update()
    {
        if (Input.GetButtonDown(INTERACT_INPUTNAME))
        {
            SceneManager.LoadScene(TITLE_SCENENAME);
        }
    }
}
