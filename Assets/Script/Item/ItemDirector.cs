using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム全てを統括するクラス
/// </summary>
public class ItemDirector : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private Transform _playerTransform = default;

    [Header("【配列用変数】")]
    [SerializeField] private float _deleteElementDuration = 5.0f;

    private List<ItemManager> _itemManagers = new List<ItemManager>();

    private float _timer = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成したアイテムをリストに入れるメソッド
    /// </summary>
    /// <param name="item">リストに入れたいアイテム</param>
    public void SetItem(GameObject item)
    {
        ItemManager itemManager = item.GetComponent<ItemManager>();
        if (itemManager != null)
        {
            _itemManagers.Add(itemManager);
            itemManager.SetValue(_playerTransform);
            itemManager.ToStart();
        }
    }

    /// <summary>
    /// リスト内の全てのアイテムを動かすメソッド：FixedUpdateで実行
    /// </summary>
    public override void FixedUpdateMethod()
    {
        foreach (ItemManager itemManager in _itemManagers)
        {
            if(itemManager == null)
            {
                continue;
            }

            itemManager.FixedUpdateMethod();
        }

        DeleteElement();
    }

    /// <summary>
    /// リストの何も無い部分を消去するメソッド（メモリ削減）
    /// </summary>
    private void DeleteElement()
    {
        _timer += Time.deltaTime;

        if(_timer > _deleteElementDuration)
        {
            _timer = 0.0f;
            _itemManagers.RemoveAll(ItemManager => ItemManager == null);
        }
    }
}
