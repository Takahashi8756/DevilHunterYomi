using UnityEngine;

public class PopUIPositionManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private RectTransform _rectTransform = default;

    [Header("【UI表示のオフセット】")]
    [SerializeField] private Vector3 _offset = Vector2.zero;

    private Transform _target = default;

    public void TargetUpdate(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if(_target == null)
        {
            return;
        }

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        float scaleX = Screen.width / (float)Camera.main.pixelWidth;
        float scaleY = Screen.height / (float)Camera.main.pixelHeight;

        screenPos.x *= scaleX;
        screenPos.y *= scaleY;

        _rectTransform.position = screenPos + _offset;
    }
}
