using UnityEngine;

/// <summary>
/// 弾の持つ攻撃力、クリティカル率を管理するクラス
/// 【制作日時：2025/10/20】
/// </summary>
public class BulletData : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //弾の持つデータ
    private float _bulletDamage = 0.0f;
    private float _bulletCritical = 0.0f;
    private float _criticalMagnification = 0.0f;

    //プロパティ
    public float BulletDamage => _bulletDamage;
    public float BulletCritical => _bulletCritical;
    public float CriticalMagnification => _criticalMagnification;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 弾のもつダメージを設定するメソッド：InputShot側から実行
    /// </summary>
    /// <param name="damage">プレイヤーの攻撃力を代入</param>
    public void SetDamage(float damage)
    {
        _bulletDamage = damage;
    }

    /// <summary>
    /// 弾のもつクリティカル率を設定するメソッド：InputShot側から実行
    /// </summary>
    /// <param name="critical">プレイヤーのクリティカル率を代入</param>
    public void SetCritical(float critical)
    {
        _bulletCritical = critical;
    }

    /// <summary>
    /// クリティカルの倍率を設定するメソッド：InputShot側から実行
    /// </summary>
    /// <param name="magnification"></param>
    public void SetMagnification(float magnification)
    {
        _criticalMagnification = magnification;
    }
}
