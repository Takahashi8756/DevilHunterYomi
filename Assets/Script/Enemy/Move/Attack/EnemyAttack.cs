using UnityEngine;

/// <summary>
/// 敵の攻撃用基底クラス
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// 生成時実行する処理
    /// </summary>
    /// <param name="bulletPool">弾のある場所を代入</param>
    public virtual void ToStart(EnemyBulletPool bulletPool)
    {

    }

    /// <summary>
    /// 攻撃を実行する処理
    /// </summary>
    /// <param name="playerCheck">プレイヤーを検知したか</param>
    public virtual void Attack(bool playerCheck)
    {
        
    }
}
