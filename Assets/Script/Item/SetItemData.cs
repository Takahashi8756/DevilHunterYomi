using UnityEngine;

/// <summary>
/// アイテムの情報を管理するクラス
/// </summary>
public class SetItemData : MonoBehaviour
{
    //アイテム情報を入れたScriptableObjectを入れる。
    [Header("【取得用変数】")]
    [SerializeField] private ItemData _itemData = default;

    public ItemData ItemData => _itemData;
}
