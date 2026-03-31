using UnityEngine;

/// <summary>
/// チュートリアル管理用クラス
/// </summary>
public class TutorialManager : Updater
{
    [Header("【取得用変数】")]
    [SerializeField] private PlayerState _playerState = default;

    [Header("チュートリアル取得用")]
    [SerializeField] private BaseTutorial[] _baseTutorials = new BaseTutorial[1];   

    private int _tutorialIndex = 0;
    private bool _isStart = false;

    private void Start()
    {
        _playerState.StateChange(PlayerState.PlayerStatus.Movie);

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
        Debug.Log("チュートリアル" +  _tutorialIndex + "終了");

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
