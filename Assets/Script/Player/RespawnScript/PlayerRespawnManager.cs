using UnityEngine;

/// <summary>
/// プレイヤーのリスポーンを管理するクラス
/// 【制作日時：2025/9/30】
/// </summary>
public class PlayerRespawnManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【変数】")]
    [SerializeField] private float _respawnPosition = -20.0f;

    [Header("【取得】")]
    [SerializeField] private Transform _respawnPoint = default;
    [SerializeField] private Transform _bossRespawnPoint = default;

    //現在のリスポーンポジション
    private Transform _nowRespawnPosition = default;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        _nowRespawnPosition = _respawnPoint;
    }

    /// <summary>
    /// プレイヤーの位置をチェックし、一定値以下ならリスポーンさせるメソッド
    /// </summary>
    public void RespawnCheck()
    {
        //Y軸が規定値以下なら、リスポーンを実行
        if(transform.position.y <= _respawnPosition)
        {
            Respawn();
        }
    }

    public void BossRoomRespawn()
    {
        transform.position = _bossRespawnPoint.position;
        transform.rotation = Quaternion.identity;
        _nowRespawnPosition = _bossRespawnPoint;
    }

    /// <summary>
    /// プレイヤーを既定の位置にリスポーンさせるメソッド
    /// </summary>
    private void Respawn()
    {
        //リスポーンポイントが設定されているなら、その位置にリスポーン
        if(_respawnPoint != null)
        {
            transform.position = _respawnPoint.position;
        }
        //そうでないなら、0地点にリスポーン
        else
        {
            Debug.LogError("リスポーン地点が設定されていません");
            transform.position = Vector3.zero;
        }
    }
}
