using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageUIManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Animator _hpAnimator = default;
    [SerializeField] private Slider _hpSlider = default;

    private const string HPGAGE_TRIGGER_NAME = "Damage";

    public void ToStart(float hp)
    {
        _hpSlider.maxValue = hp;
        _hpSlider.value = hp;
    }

    public void UpdateSlider(float value)
    {
        _hpSlider.value = value;
        _hpAnimator.SetTrigger(HPGAGE_TRIGGER_NAME);
    }
}
