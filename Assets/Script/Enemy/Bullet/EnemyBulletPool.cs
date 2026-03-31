using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾のオブジェクトプールを管理するクラス
/// </summary>
public class EnemyBulletPool : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【プール作成用変数】")]
    [SerializeField] private int _bulletPoolSize = 10;
    [SerializeField] private GameObject _bulletPrefab = default;

    //弾を格納するリスト
    private List<GameObject> _bulletPool = new List<GameObject>();

    //========================================================================
    //メソッド
    //========================================================================

    private void Start()
    {
        CreatePool();
    }

    /// <summary>
    /// プールを作成するメソッド
    /// </summary>
    public void CreatePool()
    {
        if(_bulletPrefab == null)
        {
            return;
        }

        _bulletPool = new List<GameObject>();

        for(int i= 0; i < _bulletPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    /// <summary>
    /// 弾丸をプールに装填するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, this.transform);

        bullet.SetActive(false);
        _bulletPool.Add(bullet);

        return null;
    }

    public GameObject GetBullet()
    {
        if(_bulletPrefab == null)
        {
            return null;
        }

        foreach(GameObject bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return CreateNewBullet();
    }
}
