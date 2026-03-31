using UnityEngine;

/// <summary>
/// 敵をプレイヤーの方に向かせる用メソッド
/// </summary>
public class EnemyDirection : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private Transform _enemyHeadTransform = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 敵を常時プレイヤーの方向を向かせるメソッド
    /// </summary>
    /// <param name="playerTransform"></param>
    public void DirectionUpdater(Transform playerTransform)
    {
        Vector3 position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        Vector3 positionY = new Vector3(0.0f, playerTransform.position.y, 0.0f);

        transform.LookAt(position);
        _enemyHeadTransform.LookAt(playerTransform);
    }
}
