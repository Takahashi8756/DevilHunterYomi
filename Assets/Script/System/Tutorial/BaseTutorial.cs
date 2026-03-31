using System;
using UnityEngine;

public abstract class BaseTutorial : MonoBehaviour
{
    public abstract event Action OnEndTutorial;

    /// <summary>
    /// チュートリアル開始メソッド
    /// </summary>
    public abstract void StartTutorial();


    /// <summary>
    /// チュートリアルのUpdateメソッド
    /// </summary>
    public abstract void UpdateTutorial();
}
