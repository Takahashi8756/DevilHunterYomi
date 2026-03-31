using UnityEngine;

/// <summary>
/// アイテムのスクリプトを管理するクラス
/// </summary>
public class ItemManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private FollowPlayer _followPlayer = default;
    [SerializeField] private ItemPop _itemPop = default;
    [SerializeField] private GiveValue _giveValue = default;
    [SerializeField] private ItemDirection _itemDirection = default;
    [SerializeField] private ItemDelete _itemDelete = default;

    private Transform _playerTransform = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行するメソッド
    /// </summary>
    public void ToStart()
    {
        _itemPop.MoveStart();
    }

    /// <summary>
    /// プレイヤーの位置を取得するメソッド（ItemDirector側から実行）
    /// </summary>
    /// <param name="playerTransform">プレイヤーの位置</param>
    public void SetValue(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    /// <summary>
    /// アイテムを動かすメソッド
    /// </summary>
    public void FixedUpdateMethod()
    {
        _itemPop.Move();
        _followPlayer.Follow();
        _giveValue.Give();
        _itemDirection.DirectionUpdate(_playerTransform);
        _itemDelete.FixedUpdateMethod();
    }
}
