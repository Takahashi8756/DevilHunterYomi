using UnityEngine;
using UnityEngine.UI;

public class PopDamageManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private Transform _targetTransform = default;
    [SerializeField] private GameObject _damageTextUI = default;
    [SerializeField] private GameObject _criticalDamageTextUI = default;

    [Header("【設定用変数】")]
    [SerializeField] private float _uiDestroyDuration = 5.0f;
    [SerializeField] private float _minDeviation = 0.3f;
    [SerializeField] private float _maxDeviation = 0.4f;
    [SerializeField] private float _circleDeviation = 1.2f;

    //テキストの親オブジェクト
    private Transform _damagePopParent = default;

    //定数
    private const string TARGET_OBJECT_NAME = "target";

    public void Tostart(Transform target)
    {
        _damagePopParent = target;
    }

    /// <summary>
    /// 受けたダメージをキャンバス上にポップさせるメソッド
    /// </summary>
    /// <param name="damage">受けたダメージ数</param>
    public void PopDamage(float damage, bool isCritical)
    {
        if(_damageTextUI == null)
        {
            return;
        }

        GameObject uiText;

        //クリティカルなら表示UIを変更
        if (isCritical)
        {
            uiText = Instantiate(_criticalDamageTextUI);
        }
        else
        {
            uiText = Instantiate(_damageTextUI);
        }

        GameObject target = new GameObject(TARGET_OBJECT_NAME);
        
        target.transform.SetParent(_targetTransform);
        uiText.transform.SetParent(_damagePopParent);

        uiText.SetActive(true);

        //ダメージUIの出現地点をブレさせる
        Vector3 circlePos = Random.insideUnitCircle * _circleDeviation;
        target.transform.position = transform.position + Vector3.up * Random.Range(_minDeviation, _maxDeviation) + new Vector3(circlePos.x, 0, circlePos.y);

        //カメラがRenderTextureによって縮小されているため、キャンバスの大きさに補正
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);

        float scaleX = Screen.width / (float)Camera.main.pixelWidth;
        float scaleY = Screen.height / (float)Camera.main.pixelHeight;

        screenPos.x *= scaleX;
        screenPos.y *= scaleY;

        uiText.GetComponent<RectTransform>().position = screenPos;
        uiText.GetComponent<Text>().text = Mathf.Round(damage).ToString();
        uiText.GetComponent<PopUIPositionManager>().TargetUpdate(target.transform);

        //指定時間後にUIを削除
        Destroy(target, _uiDestroyDuration);
        Destroy(uiText, _uiDestroyDuration);
    }
} 
