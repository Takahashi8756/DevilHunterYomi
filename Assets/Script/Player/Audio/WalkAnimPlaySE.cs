using UnityEngine;

/// <summary>
/// プレイヤーの歩く音を再生する用クラス
/// </summary>
public class WalkAnimPlaySE : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【取得用変数】")]
    [SerializeField] private PlayerSEManager _playerSEManager = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 歩くSEを再生するメソッド：Animation側から実行
    /// </summary>
    public void PlaySE()
    {
        _playerSEManager.PlayWalkAudio();
    }
}
