using UnityEngine;

/// <summary>
/// ボスの射撃攻撃を管理するクラス
/// </summary>
public class BossShotAttack : MonoBehaviour
{
    [Header("【攻撃用変数】")]
    [SerializeField] private float _spreadAngle = 30.0f;

    [Header("【取得用変数】")]
    [SerializeField] private Transform _shotPoint = default;

    private EnemyBulletPool _enemyBulletPool = default;
    private EnemyState _enemyState = default;

    public void ToStart(EnemyBulletPool pool, EnemyState state)
    {
        _enemyBulletPool = pool;
        _enemyState = state;
    }

    /// <summary>
    /// 撃つ際の処理
    /// </summary>
    /// <param name="numberOfShot">発射数</param>
    public void Shot(int numberOfShot)
    {
        Vector3 shotDirection = _shotPoint.transform.forward;
        Quaternion baseRotation = Quaternion.LookRotation(shotDirection);

        if (numberOfShot == 1)
        {
            GameObject bullet = _enemyBulletPool.GetBullet();
            bullet.transform.position = _shotPoint.transform.position;
            bullet.transform.rotation = baseRotation;

            bullet.SetActive(true);

            bullet.GetComponent<TrailRenderer>().Clear();
            bullet.GetComponent<BulletData>().SetDamage(_enemyState.EnemyStrength);
            bullet.GetComponent<EnemyBulletManager>().ToReset();

            return;
        }

        float step = _spreadAngle / (numberOfShot - 1);
        float startAngle = -_spreadAngle / 2;

        for (int i = 0; i < numberOfShot; i++)
        {
            float angle = startAngle + step * i;
            Quaternion rotation = baseRotation * Quaternion.Euler(0, angle, 0);

            GameObject bullet = _enemyBulletPool.GetBullet();
            bullet.transform.position = _shotPoint.transform.position;
            bullet.transform.rotation = rotation;

            bullet.SetActive(true);

            bullet.GetComponent<TrailRenderer>().Clear();
            bullet.GetComponent<BulletData>().SetDamage(_enemyState.EnemyStrength);
            bullet.GetComponent<EnemyBulletManager>().ToReset();
        }
    }
}
