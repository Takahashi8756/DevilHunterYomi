using UnityEngine;

/// <summary>
/// 射撃の入力検知＋弾の発射用クラス
/// 【制作日時：2025/10/15】
/// </summary>
public class InputShot : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【enum】---//

    private enum ShotState
    {
        Active,
        Interval,
    }

    private ShotState _state = ShotState.Active;

    //---【変数】---//

    [Header("【射撃用変数】")]
    [SerializeField] private float _shotInterval = 0.1f;
    [SerializeField] private float _shotSpreadAngle = 2.0f;
    [SerializeField] private GameObject _shotPoint = default;
    

    [Header("【スクリプト取得】")]
    [SerializeField] private PlayerReload _playerReload = default;
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;

    //インターバル計算用タイマー
    private float _intervalTimer = 0.0f;

    //取得用
    private BulletPool _bulletPool = default;

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    /// <summary>
    /// 射撃の入力検知用メソッド：Updateメソッドで実行
    /// </summary>
    public void InputShotBullet(PlayerInputManager input)
    {
        //switchで状態を管理
        switch (_state)
        {
            //射撃可能状態
            case ShotState.Active:

                if(_playerState.NowAttackState != PlayerState.AttackState.Ready)
                {
                    return;
                }

                //Shotボタンが押されたら射撃し、ステートをインターバルに
                if (input.IsShot && _playerReload.BulletCount > 0)
                {
                    Shot();
                    _playerReload.ConsumeBullet();
                    _playerSEManager.PlayShotClip();
                    _state = ShotState.Interval;
                }

                break;

            //射撃後のインターバル状態
            case ShotState.Interval:

                //タイマーにDeltaTimeを加算
                _intervalTimer += Time.deltaTime;

                //タイマーの値が一定以上なら射撃可能に
                if(_intervalTimer > _shotInterval)
                {
                    _intervalTimer = 0.0f;
                    _state = ShotState.Active;
                }

                break;
        }
    }

    /// <summary>
    /// 弾をオブジェクトプール内から取得し、発射するメソッド
    /// </summary>
    private void Shot()
    {
        //スクリプトが未登録ならリターン
        if(_bulletPool == null)
        {
            return;
        }

        //弾丸のオブジェクトをプールから取得し、射撃ポイントから発射（向きは射撃ポイント依存）
        GameObject bullet = _bulletPool.GetBullet();
        bullet.GetComponent<TrailRenderer>().Clear();

        //プレイヤー正面の方向を取得
        Vector3 shotDirection = _shotPoint.transform.forward;

        //Random.Range関数を使って、shotSpreadAngleの値に応じたブレの値を出力
        float yaw = Random.Range(-_shotSpreadAngle, _shotSpreadAngle);
        float pitch = Random.Range(-_shotSpreadAngle, _shotSpreadAngle);

        //ブレの値からQuaternion型の数値を計算し、最終的な発射角度を求める
        Quaternion spreadRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 spreadDirection = spreadRotation * shotDirection;

        //弾の位置と角度を変更
        bullet.transform.position = _shotPoint.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(spreadDirection);

        //弾のもつダメージ量とクリティカル率、クリティカル倍率を変更
        BulletData bulletData = bullet.GetComponent<BulletData>();
        bulletData.SetDamage(_playerState.NowStrength);
        bulletData.SetCritical(_playerState.NowCritical);
        bulletData.SetMagnification(_playerState.NowMagnification);

        //弾の状態をリセット
        BulletManager bulletManager = bullet.GetComponent<BulletManager>();
        bulletManager.ToReset();

        //弾丸を有効化（これにより前に進む）
        bullet.SetActive(true);

        //UIを変更
        _playerUIManager.Shot();
        _playerUIManager.GunShot();
    }
}
