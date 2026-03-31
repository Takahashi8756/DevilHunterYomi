using UnityEngine;

/// <summary>
/// ゲート突入時のイベントを管理するベーススクリプト
/// </summary>
public abstract class BaseGateEvent : MonoBehaviour
{
    /// <summary>
    /// ゲート突入時のイベント
    /// </summary>
    public abstract void IntoGateEvent();
}
