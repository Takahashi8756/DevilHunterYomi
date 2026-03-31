using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameClearDirection : MonoBehaviour
{
    [Header("【設定用変数】")]
    [SerializeField] private float _slowTimeScale = default;

    [Header("【TimeLine制御】")]
    [SerializeField] private PlayableDirector _clearTimeline = default;

    [Header("【取得用変数】")]
    [SerializeField] private BGMManager _bgmManager = default;

    //ムービー終了を通知
    public event Action OnEndMovie = default;

    public void StartClearMovie()
    {
        Time.timeScale = _slowTimeScale;
        _bgmManager.BGMFadeOut();
        _clearTimeline.Play();
    }

    public void ReturnTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    public void EndMovie()
    {
        OnEndMovie?.Invoke();
    }
}