using UnityEngine;
using UnityEngine.UI;

public class ShowTalkTextUI : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Text _talkText = default;
    [SerializeField] private Animator _talkTextAnim = default;

    private const string SHOW_TRIGGER_NAME = "Show";

    public void ShowTalkText(string text)
    {
        _talkText.text = text;
        _talkTextAnim.SetTrigger(SHOW_TRIGGER_NAME);
    }
}
