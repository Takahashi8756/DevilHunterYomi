using UnityEngine;

/// <summary>
/// アップデート処理をまとめて実行する用のスクリプト
/// 【制作者：髙橋英士】
/// 【制作日時：2025/9/24】
/// </summary>
public class UpdateManager : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================


    [Header("【スクリプト取得】")]
    [SerializeField, Tooltip("Updateメソッドで実行したいスクリプトを入れる場所")]
    private Updater[] _updaters = default;

    [SerializeField, Tooltip("FixedUpdateメソッドで実行したいスクリプトを入れる場所")]
    private Updater[] _fixedUpdaters = default;


    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 毎フレーム実行されるメソッド（入力処理などに使う、TimeScaleの影響を受けない）
    /// </summary>
    private void Update()
    {
        //_updater配列に格納されているUpdaterを継承したスクリプトのメソッドを実行
        foreach (Updater updater in _updaters)
        {
            updater.UpdateMethod();
        }
    }

    /// <summary>
    /// 一定時間ごとに実行されるメソッド（入力処理以外の処理に使う、TimeScaleの影響を受ける）
    /// </summary>
    private void FixedUpdate()
    {
        //_fixedUpdaters配列に格納されているUpdaterを継承したスクリプトのメソッドを実行
        foreach (Updater fixedUpdater in _fixedUpdaters)
        {
            fixedUpdater.FixedUpdateMethod();
        }
    }
}
