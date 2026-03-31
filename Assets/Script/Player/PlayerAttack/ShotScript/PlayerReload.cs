using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【enum】---//
   
    public enum ReloadState
    {
        Active,
        Reload,
    }

    private ReloadState _nowCapacityState = ReloadState.Active;
    public ReloadState NowCapacityState
    {
        get { return _nowCapacityState; }
    }

    //---【変数】---//

    [Header("【変数】")]
    [SerializeField] private int _capacity = 6;
    [SerializeField] private float _reloadDuration = 2.0f;
    [SerializeField] private float _criticalReloadOffset = 0.3f;

    [Header("【取得用変数】")]
    [SerializeField] private PlayerUIManager _playerUIManager = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;

    //計算用変数
    private int _bulletCount = 0;
    private float _halfReloadDuration = 0.0f;
    private float _minCriticalTime = 0.0f;
    private float _maxCriticalTime = 0.0f;
    private bool _emptyFlag = false;

    //タイマー
    private float _timer = 0.0f;

    //クリティカルリロード可能か
    private bool _criticalReloadTime = false;
    private bool _canCriticalReload = false;

    //現在の弾数を返す
    public int BulletCount
    {
        get { return _bulletCount; }
    }

    //========================================================================
    //メソッド
    //========================================================================

    public void ToStart()
    {
        _bulletCount = _capacity;
        _halfReloadDuration = _reloadDuration / 2;
        _minCriticalTime = _halfReloadDuration - _criticalReloadOffset;
        _maxCriticalTime = _halfReloadDuration + _criticalReloadOffset;
    }

    /// <summary>
    /// リロードボタンが押されたか検知する用のメソッド：Updateにて実行
    /// </summary>
    public void InputReload(PlayerInputManager input)
    {
        //Switchで状態によって処理を分ける
        switch (_nowCapacityState)
        {
            //射撃可能状態の場合
            case ReloadState.Active:

                //現在の弾数が最大値より小さい場合にリロードボタンを押すとリロード開始
                if (input.IsReload && _bulletCount < _capacity)
                {
                    //弾数を0にリセットし、ステートを変更
                    _bulletCount = 0;
                    _canCriticalReload = true;
                    _playerUIManager.ReloadStart(_reloadDuration, _criticalReloadOffset);
                    _playerUIManager.ToCoolDown();
                    _nowCapacityState = ReloadState.Reload;

                    return;
                }
                //現在の弾数が0以下だった場合に射撃ボタンを押すとリロード開始
                else if (input.IsShot && _emptyFlag)
                {
                    //弾数を0にリセットし、ステートを変更
                    _bulletCount = 0;
                    _canCriticalReload = true;
                    _playerUIManager.ReloadStart(_reloadDuration, _criticalReloadOffset);
                    _playerUIManager.ToCoolDown();
                    _nowCapacityState = ReloadState.Reload;

                    return;
                }

                if(_bulletCount <= 0)
                {
                    _emptyFlag = true;
                }

                break;

            //リロード状態の場合
            case ReloadState.Reload:

                if(_canCriticalReload && CriticalReloadCheck(input))
                {
                    if(!_criticalReloadTime)
                    {
                        _playerUIManager.CriticalReloadMiss();
                        _playerSEManager.PlayReloadMissClip();
                        _canCriticalReload = false;
                        return;
                    }

                    ReloadBullet();
                    _playerSEManager.PlayReloadClip();
                }

                break;
        }
    }

    /// <summary>
    /// リロード中の処理を担当するメソッド：FixedUpdateにて実行
    /// </summary>
    public void Reload()
    {
        //Switchで状態によって処理を分ける
        switch (_nowCapacityState)
        {
            //リロード状態の場合
            case ReloadState.Reload:

                _playerUIManager.UpdateReloadValue(_timer);

                //タイマーにdeltaTimeを加算
                _timer += Time.deltaTime;

                //タイマーの値が一定より大きい場合、リロード完了と見なし射撃可能に
                if(_timer > _reloadDuration)
                {
                    ReloadBullet();
                    _playerSEManager.PlayReloadClip();
                }

                //タイマーの値がクリティカルリロード可能範囲内なら、リロード可能に
                if(_timer >= _minCriticalTime &&  _timer <= _maxCriticalTime)
                {
                    _criticalReloadTime = true;
                }
                else
                {
                    _criticalReloadTime = false;
                }

                    break;
        }
    }

    /// <summary>
    /// 弾を消費させる用のメソッド：InputShot側で実行
    /// </summary>
    public void ConsumeBullet()
    {
        //現在の弾数が0より大きいなら、弾を一つ消費する。
        if(_bulletCount > 0)
        {
            _bulletCount--;
        }
    }

    public void ReloadBullet()
    {
        _bulletCount = _capacity;
        _emptyFlag = false;
        _timer = 0.0f;

        _playerUIManager.ReloadEnd();
        _playerUIManager.ToActive();
        _nowCapacityState = ReloadState.Active;
    }

    /// <summary>
    /// クリティカルリロードの入力を検知するメソッド
    /// </summary>
    /// <returns>ボタンが押されたらtrue</returns>
    private bool CriticalReloadCheck(PlayerInputManager input)
    {
        if(input.IsShot || input.IsReload)
        {
            return true;
        }
       
        return false;
    }
}
