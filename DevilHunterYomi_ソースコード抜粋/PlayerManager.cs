using UnityEngine;

/// <summary>
/// プレイヤーの動きを統括するクラス
/// 【制作日時：2025/9/24】
/// </summary>
public class PlayerManager : Updater
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【変数】---//

    [Header("【スクリプト取得】")]
    [Header("【プレイヤー自身のスクリプト】")]
    [SerializeField] private PlayerState _playerState = default;
    [SerializeField] private PlayerRespawnManager _playerRespawnManager = default;
    [SerializeField] private PlayerInputManager _playerInputManager = default;
    [SerializeField] private PlayerMove _playerMove = default;
    [SerializeField] private PlayerDodge _playerDodge = default;
    [SerializeField] private CameraMove _cameraMove = default;
    [SerializeField] private PlayerRespawnManager _playerRespawn = default;
    [SerializeField] private InputShot _inputShot = default;
    [SerializeField] private PlayerReload _playerReload = default;
    [SerializeField] private KnifeAttack _knifeAttack = default;
    [SerializeField] private PlayerCheckObject _playerCheckObject = default;
    [SerializeField] private PlayerSEManager _playerSEManager = default;
    [SerializeField] private InputPause _inputPause = default;
    [SerializeField] private UIDamageReaction _uiDamageReaction = default;

    [Header("【プレイヤー外のスクリプト】")]
    [SerializeField] private BulletPool _bulletPool = default;
    [SerializeField] private ManagerDirector _managerDirector = default;
    [SerializeField] private PauseManager _pauseManager = default;

    //========================================================================
    //メソッド
    //========================================================================

    private void Start()
    {
        _playerInputManager.ToStart();
        _bulletPool.ToStart();
        _inputShot.ToStart(_bulletPool);
        _playerCheckObject.ToStart(_managerDirector.Manager.MainSceneManager, _managerDirector.Manager.SpawnEnemy);
        _playerState.ToStart();
        _knifeAttack.ToStart();
        _playerReload.ToStart();
        _cameraMove.ToStart();
        _playerRespawnManager.ToStart();
        _inputPause.ToStart(_pauseManager);
    }

    /// <summary>
    /// Updateで実行されるメソッド
    /// </summary>
    public override void UpdateMethod()
    {
        switch (_playerState.NowStatus)
        {
            case PlayerState.PlayerStatus.Active:
                _playerInputManager.UpdateInput();
                break;

            case PlayerState.PlayerStatus.ContentsSelect:
                _playerInputManager.ResetInput();
                break;

            case PlayerState.PlayerStatus.Movie:
                return;

            case PlayerState.PlayerStatus.Death:
                return;
        }

        if (_pauseManager.NowPauseState == PauseManager.PauseState.Pause)
        {
            return;
        }

        _playerMove.MoveInput(_playerInputManager);
        _cameraMove.InputCameraMove(_playerInputManager);
        _knifeAttack.InputAttack(_playerInputManager);
        _inputShot.InputShotBullet(_playerInputManager);
        _playerReload.InputReload(_playerInputManager);
        _playerCheckObject.CheckAndInput(_playerInputManager);
        _inputPause.UpdateMethod(_playerInputManager);
        
    }

    /// <summary>
    /// FixedUpdateで実行されるメソッド
    /// </summary>
    public override void FixedUpdateMethod()
    {
        _playerDodge.DodgeUpdater();
        _knifeAttack.AttackCoolDown();
        _playerReload.Reload();
        _playerRespawn.RespawnCheck();
        _playerSEManager.PlayAudioCoolDown();
        _uiDamageReaction.FixedUpdateMethod();
    }
}
