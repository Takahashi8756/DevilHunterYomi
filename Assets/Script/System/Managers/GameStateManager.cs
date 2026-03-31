using UnityEngine;

/// <summary>
/// ゲームの状態を管理するスクリプト
/// 【作成者：髙橋英士】
/// 【制作日時：2025/9/24】
/// </summary>
public class GameStateManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    #region 【enum】

    public enum GameState
    {
        Pause,
        Active,
        Movie,
    }

    private GameState _nowGameState = GameState.Active;

    #endregion

    #region 【ゲッター】

    public GameState NowGameState
    {
        get { return _nowGameState; }
    }

    #endregion

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// ポーズ状態に移行するメソッド
    /// </summary>
    public void Pause()
    {
        _nowGameState = GameState.Pause;
        Cursor.visible = true;
    }

    /// <summary>
    /// 通常状態に移行するメソッド
    /// </summary>
    public void Active()
    {
        _nowGameState = GameState.Active;
        Cursor.visible = false;
    }

    /// <summary>
    /// ムービー状態に移行するメソッド
    /// </summary>
    public void Movie()
    {
        _nowGameState = GameState.Movie;
        Cursor.visible = false;
    }
}
