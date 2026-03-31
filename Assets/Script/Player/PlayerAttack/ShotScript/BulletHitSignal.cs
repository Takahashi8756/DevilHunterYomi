using UnityEngine;

public class BulletHitSignal : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【取得用変数】")]
    [SerializeField, Tooltip("プレイヤーのUI管理スクリプト")]
    private PlayerUIManager _playerUIManager = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時呼び出されるメソッド：Startメソッドで実行
    /// </summary>
    private void ToStart()
    {
        if( _playerUIManager == null)
        {
            _playerUIManager = GameObject.FindWithTag("Player").GetComponent<PlayerUIManager>();
        }
    }

    /// <summary>
    /// ヒットしたらUIを動かすメソッド
    /// </summary>
    public void GiveHitSignal()
    {
        _playerUIManager.HitUI();
    }
}
