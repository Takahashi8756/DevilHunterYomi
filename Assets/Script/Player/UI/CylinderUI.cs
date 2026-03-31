using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シリンダーのUIを管理するクラス
/// </summary>
public class CylinderUI : MonoBehaviour
{
    [Header("シリンダーのUI"), SerializeField]
    private Image[] _cylinderImages = new Image[6];

    //シリンダーの指標
    private int _cylinderIndex = 6;


    public void ResetCylinder()
    {
        foreach(Image cylinderImage  in _cylinderImages)
        {
            cylinderImage.enabled = true;
        }

        _cylinderIndex = _cylinderImages.Length;
    }

    public void ExpenditureBullet()
    {
        _cylinderImages[_cylinderIndex - 1].enabled = false;

        _cylinderIndex -= 1;
    }
}
