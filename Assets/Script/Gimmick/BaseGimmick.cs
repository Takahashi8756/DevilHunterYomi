using System;
using UnityEngine;

/// <summary>
/// ギミックの素体
/// </summary>
public abstract class BaseGimmick : MonoBehaviour
{
    public abstract bool IsInteract { get; }
    public abstract event Action<string> OnEncountText;

    /// <summary>
    /// 見つけた時に実行するメソッド
    /// </summary>
    public abstract void EncountGimmick();

    /// <summary>
    /// インタラクトした時に実行するメソッド
    /// </summary>
    public abstract void InteractGimmick();

    /// <summary>
    /// ギミックを破壊するときに実行するメソッド
    /// </summary>
    public abstract void DestroyGimmick();
}
