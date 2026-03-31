using UnityEngine;

public class FaceUI : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Animator _faceAnimator = default;

    private const string FACE_DAMAGE_TRIGGER = "Damage";

    public void DamageFace()
    {
        _faceAnimator.SetTrigger(FACE_DAMAGE_TRIGGER);
    }
}
