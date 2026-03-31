using UnityEngine;

/// <summary>
/// 弾を移動させるクラス
/// 【制作日時：2025/10/21】
/// </summary>
public class BulletMove : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【移動用変数】")]
    [SerializeField, Tooltip("弾の移動速度")]
    private float _bulletSpeed = 1.0f;
    [SerializeField, Tooltip("弾の寿命（時間）")]
    private float _bulletLifeTime = 1.0f;

    //タイマー
    private float _timer = 0.0f;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToReset()
    {
        _timer = 0.0f;
    }

    /// <summary>
    /// 弾を移動させるメソッド：FixedUpdateにて実装
    /// </summary>
    public void MoveBullet(bool hit)
    {
        if (hit)
        {
            return;
        }

        //タイマーに加算
        _timer += Time.deltaTime;

        //タイマーの値が寿命を上回ったら、弾を非表示にする。
        if(_timer > _bulletLifeTime)
        {
            _timer = 0.0f;
            gameObject.SetActive(false);
        }

        //前方に移動させる。
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;
    }
}
