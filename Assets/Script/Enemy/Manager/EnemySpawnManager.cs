using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 敵のランダム生成に関するクラス
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Animator _gateAnimator = default;
    [SerializeField] private Animator _enemySpriteAnimator = default;
    [SerializeField] private Animator _shadowAnimator = default;
    [SerializeField] private EnemyState _enemyState = default;
    [SerializeField] private Collider _enemyCollider = default;

    [Header("【マップアイコン】")]
    [SerializeField] private SpriteRenderer _mapIcon = default;
    [SerializeField] private float _fadeDuration = 1.0f;

    [Header("【出現時間設定用変数】")]
    [Header("【ゲート出現時間】")]
    [SerializeField] private float _minOpenGateDuration = 0.5f;
    [SerializeField] private float _maxOpenGateDuration = 2.0f;
    [Header("【敵スポーン時間】")]
    [SerializeField] private float _minSpawnDuration = 1.0f;
    [SerializeField] private float _maxSpawnDuration = 2.0f;
    [Header("【ゲート閉鎖時間】")]
    [SerializeField] private float _HideGateDuration = 1.0f;

    //定数
    private const string SPAWN_ANIM_TRIGGER_NAME = "Spawn";
    private const string HIDE_ANIM_TRIGGER_NAME = "Hide";

    /// <summary>
    /// 敵のスポーンを開始させるメソッド
    /// </summary>
    public void SpawnEnemy()
    {
        float openDuration = Random.Range(_minOpenGateDuration, _maxOpenGateDuration);
        float spawnDuration = Random.Range(_minSpawnDuration, _maxSpawnDuration);

        _enemyCollider.enabled = false;
        StartCoroutine(SpawnEnemyCorutine(openDuration, spawnDuration, _HideGateDuration));
    }

    private IEnumerator SpawnEnemyCorutine(float open, float spawn, float hide)
    {
        WaitForSeconds openDuration = new WaitForSeconds(open);
        WaitForSeconds spawnDuration = new WaitForSeconds(spawn);
        WaitForSeconds hideDuration = new WaitForSeconds(hide);

        yield return openDuration;
        _gateAnimator.SetTrigger(SPAWN_ANIM_TRIGGER_NAME);

        yield return spawnDuration;
        _enemySpriteAnimator.SetTrigger(SPAWN_ANIM_TRIGGER_NAME);
        _shadowAnimator.SetTrigger(SPAWN_ANIM_TRIGGER_NAME);
        _mapIcon.DOFade(1, _fadeDuration);

        yield return hideDuration;
        _gateAnimator.SetTrigger(HIDE_ANIM_TRIGGER_NAME);

        _enemyCollider.enabled = true;
        _enemyState.ActiveEnemy();
    }
}
