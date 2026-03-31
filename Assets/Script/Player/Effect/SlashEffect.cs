using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private ParticleSystem _slashEffect = default;

    public void PlaySlashEffect()
    {
        _slashEffect.Play();
    }
}
