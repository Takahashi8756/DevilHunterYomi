using UnityEngine;

/// <summary>
/// アイテムの向きを変えるクラス
/// </summary>
public class ItemDirection : MonoBehaviour
{
    /// <summary>
    /// 常時指定の方向を向かせるメソッド
    /// </summary>
    /// <param name="playerTransform">プレイヤーの位置を代入</param>
    public void DirectionUpdate(Transform playerTransform)
    {
        Vector3 position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(position);
    }
}
