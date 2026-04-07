using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGateEvent : BaseGateEvent
{
    [Header("【取得用】")]
    [SerializeField] private CanvasGroup _tutorialCanvasGroup = default;

    private const string MAIN_SCENE_NAME = "MainScene";
    
    public override void IntoGateEvent()
    {
        _tutorialCanvasGroup.alpha = 1.0f;
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }
}
