using UnityEngine;
using UnityEngine.UI;

public class StatusGageUI : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Slider _maxHealthGage = default;
    [SerializeField] private Slider _strengthGage = default;
    [SerializeField] private Slider _criticalGage = default;
    [SerializeField] private Slider _speedGage = default;
    [SerializeField] private GameObject _healthMaxText = default;
    [SerializeField] private GameObject _strengthMaxText = default;
    [SerializeField] private GameObject _criticalMaxText = default;
    [SerializeField] private GameObject _speedMaxText = default;

    [Header("【テキスト】")]
    [SerializeField] private Text _addHealthText = default;
    [SerializeField] private Text _addStrengthText = default;
    [SerializeField] private Text _addCriticalText = default;
    [SerializeField] private Text _addSpeedText = default;

    [Header("【アニメーター】")]
    [SerializeField] private Animator _addHealthValueAnim = default;
    [SerializeField] private Animator _addStrengthValueAnim = default;
    [SerializeField] private Animator _addCriticalValueAnim = default;
    [SerializeField] private Animator _addSpeedValueAmim = default;

    //定数
    private const string ADDANIM_TRIGGER = "Add";

    public void SetMaxValue(float maxHealth, float maxStrength, float maxCritical, float maxSpeed)
    {
        _maxHealthGage.maxValue = maxHealth;
        _strengthGage.maxValue = maxStrength;
        _criticalGage.maxValue = maxCritical;
        _speedGage.maxValue = maxSpeed;
    }

    public void SetDefaultValue(float maxHealth, float maxStrength, float maxCritical, float maxSpeed)
    {
        _maxHealthGage.value = maxHealth;
        _strengthGage.value = maxStrength;
        _criticalGage.value = maxCritical;
        _speedGage.value = maxSpeed;

        _healthMaxText.SetActive(_maxHealthGage.value >= _maxHealthGage.maxValue);
        _strengthMaxText.SetActive(_strengthGage.value >= _strengthGage.maxValue);
        _criticalMaxText.SetActive(_criticalGage.value >= _criticalGage.maxValue);
        _speedMaxText.SetActive(_speedGage.value >= _speedGage.maxValue);
    }

    public void AddMaxHealthValue(float value)
    {
        _maxHealthGage.value += value;
        _addHealthText.text = "+" + value;
        _addHealthValueAnim.SetTrigger(ADDANIM_TRIGGER);
        _healthMaxText.SetActive(_maxHealthGage.value >= _maxHealthGage.maxValue);
    }

    public void AddStrengthValue(float value)
    {
        _strengthGage.value += value;
        _addStrengthText.text = "+" + value;
        _addStrengthValueAnim.SetTrigger(ADDANIM_TRIGGER);
        _strengthMaxText.SetActive(_strengthGage.value >= _strengthGage.maxValue);
    }

    public void AddCriticalValue(float value)
    {
        _criticalGage.value += value;
        _addCriticalText.text = "+" + value;
        _addCriticalValueAnim.SetTrigger(ADDANIM_TRIGGER);
        _criticalMaxText.SetActive(_criticalGage.value >= _criticalGage.maxValue);
    }

    public void AddSpeedValue(float value)
    {
        _speedGage.value += value;
        _addSpeedText.text = "+" + value;   
        _addSpeedValueAmim.SetTrigger(ADDANIM_TRIGGER);
        _speedMaxText.SetActive(_speedGage.value >= _speedGage.maxValue);
    }
}
