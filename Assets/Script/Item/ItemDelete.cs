using UnityEngine;

/// <summary>
/// 一定時間後アイテムを削除するメソッド
/// </summary>
public class ItemDelete : MonoBehaviour
{
    [Header("【設定用変数】")]
    [SerializeField] private float _deleteTime = 20.0f;

    //タイマー
    private float _timer = default;

    /// <summary>
    /// 毎フレーム実行するメソッド
    /// </summary>
    public void FixedUpdateMethod()
    {
        _timer += Time.deltaTime;

        //指定時間経ったらアイテムを削除
        if( _timer > _deleteTime)
        {
            _timer = 0;
            Destroy(gameObject);
            return;
        }
    }
}
