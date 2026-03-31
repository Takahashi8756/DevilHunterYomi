using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("表示用変数")]
    [SerializeField] private string _itemName = "";
    [SerializeField] private Color _outlineColor = Color.white;

    [Header("【上昇値】")]
    [SerializeField] private float _additionHealthValue = 0.0f;
    [SerializeField] private float _additionMaxHealthValue = 0.0f;
    [SerializeField] private float _additionStrengthValue = 0.0f;
    [SerializeField] private float _additionCriticalValue = 0.0f;
    [SerializeField] private float _additionSpeedValue = 0.0f;

    //プロパティ
    public string ItemName => _itemName;
    public Color OutlineColor => _outlineColor;
    public float AdditionHealthValue => _additionHealthValue;
    public float AdditionMaxHealthValue => _additionMaxHealthValue;
    public float AdditionStrengthValue => _additionStrengthValue;
    public float AdditionCriticalValue => _additionCriticalValue;
    public float AdditionSpeedValue => _additionSpeedValue;
}
