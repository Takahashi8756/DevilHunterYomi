using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : MonoBehaviour
{
    [Header("取得用変数")]
    [SerializeField] private Text _itemText = default;
    [SerializeField] private Outline _outline = default;

    public void SetValues(string name, Color color)
    {
        _itemText.text = name;
        _outline.effectColor = color;
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
