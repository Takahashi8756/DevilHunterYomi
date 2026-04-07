using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShotTutorial : BaseTutorial
{
    [Header("【敵取得】")]
    [SerializeField] private List<GameObject> _enemyList = new List<GameObject>();

    [Header("【スクリプト取得】")]
    [SerializeField] private EnemysDirector _enemyDirector = default;

    [Header("【セリフ】")]
    [SerializeField, TextArea] private string _text = default;
    [SerializeField] private ShowTalkTextUI _showTalkTextUI = default;

    [Header("【CanvasGroup】")]
    [SerializeField] private CanvasGroup _tutorialCanvas = default;
    [SerializeField] private float _fadeDuration = 1.0f;

    private int _enemyCount = 0;

    //終了通知
    public override event Action OnEndTutorial = default;

    public override void StartTutorial()
    {
        _tutorialCanvas.DOFade(1, _fadeDuration).SetLink(gameObject);
        _showTalkTextUI.ShowTalkText(_text);

        foreach(GameObject enemy in _enemyList)
        {
            _enemyCount++;
            _enemyDirector.SetEnemy(enemy);
            enemy.GetComponentInChildren<EnemyState>().EnemyDeathEvent += TutorialEnemyDeath;
        }
    }

    public override void UpdateTutorial()
    {
        
    }

    /// <summary>
    /// チュートリアルの敵死亡時呼び出されるメソッド
    /// </summary>
    private void TutorialEnemyDeath()
    {
        _enemyCount--;

        if(_enemyCount <= 0)
        {
            _tutorialCanvas.DOFade(0, _fadeDuration).SetLink(gameObject);
            OnEndTutorial.Invoke();
        }
    }
}
