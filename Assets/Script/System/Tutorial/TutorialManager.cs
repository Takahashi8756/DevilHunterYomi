using UnityEngine;
using DG.Tweening;

/// <summary>
/// チュートリアル管理用クラス
/// </summary>
public class TutorialManager : Updater
{
    [Header("【取得用変数】")]
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private CanvasGroup _titleCanvasGroup = default;

    [Header("【出現時間】")]
    [SerializeField] private float _showTitleDuration = 0.5f;

    [Header("チュートリアル取得用")]
    [SerializeField] private BaseTutorial[] _baseTutorials = new BaseTutorial[1];   

    //作業用変数
    private int _tutorialIndex = 0;
    private bool _isStart = false;

    private void Start()
    {
        _playerState.StateChange(PlayerState.PlayerStatus.Movie);
        _titleCanvasGroup.DOFade(1.0f, _showTitleDuration);

        foreach (BaseTutorial tutorial in _baseTutorials)
        {
            tutorial.OnEndTutorial += SubscriptionTutorial;
        }
    }

    /// <summary>
    /// チュートリアルのアップデートを管理するメソッド
    /// </summary>
    public override void FixedUpdateMethod()
    {
        if(!_isStart)
        {
            return;
        }

        if(_tutorialIndex >= _baseTutorials.Length)
        {
            return;
        }

        _baseTutorials[_tutorialIndex].UpdateTutorial();        
    }

    /// <summary>
    /// チュートリアル終了を購読するメソッド
    /// </summary>
    private void SubscriptionTutorial()
    {
        _tutorialIndex++;

        //全てのチュートリアルが終了した際、購読解除し早期リターン
        if(_tutorialIndex >= _baseTutorials.Length)
        {
            foreach(BaseTutorial tutorial in _baseTutorials)
            {
                tutorial.OnEndTutorial -= SubscriptionTutorial;
            }
            return;
        }

        _baseTutorials[_tutorialIndex].StartTutorial();
    }

    /// <summary>
    /// プレイヤーのステートを変えるメソッド
    /// </summary>
    public void PlayerStateChange()
    {
        _isStart = true;
        _playerState.StateChange(PlayerState.PlayerStatus.Active);

        if(_tutorialIndex >= _baseTutorials.Length)
        {
            return;
        }

        _baseTutorials[_tutorialIndex].StartTutorial();
    }
}
