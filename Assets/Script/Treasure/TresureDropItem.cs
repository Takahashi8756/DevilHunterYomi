using UnityEngine;

public class TresureDropItem : MonoBehaviour
{
    [Header("【変数】")]
    [SerializeField] private int _numberOfDrops = 5;

    [Header("【出るアイテム】")]
    [SerializeField] private GameObject[] _dropItems = new GameObject[1];

    private ItemDirector _itemDirector = default;

    public void ToStart(ItemDirector itemDirector)
    {
        _itemDirector = itemDirector;
    }

    public void OpenTresure()
    {
        if(_itemDirector == null)
        {
            return;
        }

        Drop();
        Destroy(gameObject);
    }

    private void Drop()
    {
        for (int i = 0; i < _numberOfDrops; i++)
        {
            int randomIndex = Random.Range(0, _dropItems.Length);
            GameObject item = Instantiate(_dropItems[randomIndex], transform.position, Quaternion.identity);
            _itemDirector.SetItem(item);
        }
    }
}
