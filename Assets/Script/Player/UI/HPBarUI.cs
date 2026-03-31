using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Slider _slider = default;
    [SerializeField] private Text _hpText = default;

    public void SetMaxValue(float value)
    {
        _slider.maxValue = value;
        _hpText.text = _slider.value.ToString() + "/" + _slider.maxValue.ToString();    
    }

    public void SetValue(float value)
    {
        _slider.value = value;
        _hpText.text = _slider.value.ToString() + "/" + _slider.maxValue.ToString();
    }
}
