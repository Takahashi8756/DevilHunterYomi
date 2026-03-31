using UnityEngine;

/// <summary>
/// 敵に攻撃がヒットした時光るようにするメソッド
/// </summary>
public class EnemyFlash : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private Animator _animator = default;

    private const string TRIGGER_NAME = "Flash";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// ダメージを受けた時に光らせるメソッド
    /// </summary>
　　public void Damage()
    {
        _animator.SetTrigger(TRIGGER_NAME);
    }
}
