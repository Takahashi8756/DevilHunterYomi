using UnityEngine;

/// <summary>
/// 敵を振動させるクラス
/// 【制作日時：2025/10/21】
/// </summary>
public class EnemyVibration : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    //---【enum】---//

    private enum VibeState
    {
        Wait,
        Vibration,
    }

    private VibeState _vibeState = VibeState.Wait;

    //---【変数】---//

    [Header("【振動用変数】")]
    [SerializeField] private float _viberationTime = 1.0f;
    [SerializeField] private float _defaultVibe = 1.0f;
    [SerializeField] private float _criticalVibe = 2.0f;

    [Header("【取得用変数】")]
    [SerializeField] private Transform _enemyTransform = default;

    //計算用変数
    private float _timer = 0.0f;
    private float _vibeMagnitude = 0.0f;

    //初期位置格納
    private Vector3 _defaultPosition = Vector3.zero;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 振動を開始させるメソッド
    /// </summary>
    /// <param name="critical">クリティカルが発生したかどうかを代入</param>
    public void StartVibration(bool critical)
    {
        //クリティカルか否かで振動の強さを変更
        if (critical)
        {
            _vibeMagnitude = _criticalVibe;
        }
        else
        {
            _vibeMagnitude = _defaultVibe;
        }

        //タイマーを初期化して振動開始
        _timer = 0.0f;
        _vibeState = VibeState.Vibration;
    }

    /// <summary>
    /// 振動させるメソッド
    /// </summary>
    public void Vibration()
    {
        //switchにより状態によって処理を変更
        switch (_vibeState)
        {
            //Waitの場合は何もしない
            case VibeState.Wait:
                break;

            case VibeState.Vibration:

                //タイマーにdeltatimeを加算
                _timer += Time.deltaTime;

                if(_timer > _viberationTime)
                {
                    _timer = 0.0f;
                    _enemyTransform.localPosition = _defaultPosition;
                    _vibeState =VibeState.Wait;
                    return;
                }

                //タイマーの値と振動の強さから振動を計算
                float progress = _timer / _viberationTime;
                float currentPower = Mathf.Lerp(_vibeMagnitude, 0, progress);
                Vector3 randomOffset = Random.insideUnitSphere * currentPower;

                //敵の見た目の部分を、初期位置を中心に振動
                _enemyTransform.localPosition = _defaultPosition + randomOffset;

                break;
        }
    }
}
