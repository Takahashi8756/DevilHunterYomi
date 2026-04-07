using System;
using UnityEngine;

public class TresureDropItem : BaseGimmick
{
    [Header("【インタラクト可能か】")]
    [SerializeField] private bool _isInteract = true;

    [Header("【変数】")]
    [SerializeField] private int _numberOfDrops = 5;

    [Header("【出るアイテム】")]
    [SerializeField] private GameObject[] _dropItems = new GameObject[1];

    [Header("【発見時テキスト】")]
    [SerializeField, TextArea] private string _encountText = default;

    //作業用変数
    private ItemDirector _itemDirector = default;
    private bool _isEncount = false;

    public override bool IsInteract => _isInteract;
    public override event Action<string> OnEncountText;

    public void ToStart(ItemDirector itemDirector)
    {
        _itemDirector = itemDirector;
    }

    public override void EncountGimmick()
    {
        if (_isEncount)
        {
            return;
        }

        _isEncount = true;
        OnEncountText?.Invoke(_encountText);
    }

    public override void InteractGimmick()
    {
        if (_itemDirector == null)
        {
            return;
        }

        DestroyGimmick();
    }

    public override void DestroyGimmick()
    {
        Drop();
        OnEncountText = null;
        Destroy(gameObject);
    }

    private void Drop()
    {
        for (int i = 0; i < _numberOfDrops; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _dropItems.Length);
            GameObject item = Instantiate(_dropItems[randomIndex], transform.position, Quaternion.identity);
            _itemDirector.SetItem(item);
        }
    }
}
