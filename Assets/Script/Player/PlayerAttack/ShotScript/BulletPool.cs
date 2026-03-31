using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾丸のオブジェクトプールを作成するクラス
/// 【制作日時：2025/10/14】
/// </summary>
public class BulletPool : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【プール作成用変数】")]
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private GameObject _bulletPrefab = default;

    [Header("【取得用変数】")]
    [SerializeField] private BulletHitSignal _bulletHitSignal = default;

    //弾を格納するリスト
    private List<GameObject> _bulletPool = default;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        CreatePool();
    }

    /// <summary>
    /// オブジェクトプールを作成するメソッド：Awakeで実行
    /// </summary>
    private void CreatePool()
    {
        if(_bulletPrefab == null)
        {
            Debug.LogError("弾が設定されていないので、プールが作成できませんでした...");
            return;
        }

        //リストを初期化
        _bulletPool = new List<GameObject>();

        //プールサイズの分だけ弾を格納
        for(int i = 0; i < _poolSize; i++)
        {
            CreateNewBullet();
        }
    }

    /// <summary>
    /// 弾丸のプレハブを複製し、オブジェクトプールに格納するメソッド
    /// </summary>
    /// <returns>弾丸を返す</returns>
    private GameObject CreateNewBullet()
    {
        //弾丸を複製し無効化、オブジェクトプールに格納する。
        GameObject bullet = Instantiate(_bulletPrefab, this.transform);

        BulletGiveDamage bulletGiveDamage = bullet.GetComponent<BulletGiveDamage>();
        bulletGiveDamage.BulletHitSignal = _bulletHitSignal;

        bullet.SetActive(false);
        _bulletPool.Add(bullet);

        return bullet;
    }

    /// <summary>
    /// オブジェクトプールから弾を取得するメソッド
    /// </summary>
    /// <returns>プール内の無効化されている弾丸を返す</returns>
    public GameObject GetBullet()
    {
        if( _bulletPrefab == null)
        {
            return null;
        }

        //弾丸のオブジェクトプール内の"無効化されている弾丸"を返す。
        foreach(GameObject bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        //無効化されている弾が無いなら、新たに複製しプールに格納。
        return CreateNewBullet();
    }
}
