using UnityEngine;

public class PlayerEffectManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private SlashEffect _slashEffect = default;

    public void Slash()
    {
        _slashEffect.PlaySlashEffect();
    }
}
