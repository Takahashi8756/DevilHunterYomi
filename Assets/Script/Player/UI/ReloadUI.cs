using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField, Tooltip("リロードのスライダー")]
    private Slider _reloadSlider = default;
    [SerializeField, Tooltip("クリティカルリロードの範囲")]
    private Transform _criticalRangeImage = default;
    [SerializeField, Tooltip("スライダーそのもの")]
    private GameObject _sliderObject = default;
    [SerializeField, Tooltip("クリティカルリロード失敗のイメージ")]
    private Image _missCriticalImage = default;

    //========================================================================
    //メソッド
    //========================================================================


    public void SetMaxValue(float value)
    {
        _reloadSlider.maxValue = value;
    }

    public void SetCriticalRange(float value)
    {
        Vector3 rangeTransform = _criticalRangeImage.transform.localScale;

        float criticalRange = Mathf.InverseLerp(0, 1, value);
        _criticalRangeImage.transform.localScale = new Vector3(criticalRange, rangeTransform.y, rangeTransform.z);
    }

    public void UpdateSlider(float value)
    {
        _reloadSlider.value = value;
    }

    public void showReloadUI()
    {
        _sliderObject.SetActive(true);
    }

    public void hideReloadUI()
    {
        _reloadSlider.value = 0;
        _sliderObject.SetActive(false);
    }

    public void MissCriticalImage(bool value)
    {
        if (value)
        {
            _missCriticalImage.enabled = true;
        }
        else
        {
            _missCriticalImage.enabled = false;
        }
    }
}
