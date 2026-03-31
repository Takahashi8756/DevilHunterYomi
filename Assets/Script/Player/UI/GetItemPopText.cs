using UnityEngine;

public class GetItemPopText : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private GameObject _popTextPrefab = default;
    [SerializeField] private Transform _popArea = default;

    public void PopGetItemText(string name, Color color)
    {
        GameObject text = Instantiate(_popTextPrefab, _popArea);
        text.GetComponent<GetItemUI>().SetValues(name, color);
    }
}
