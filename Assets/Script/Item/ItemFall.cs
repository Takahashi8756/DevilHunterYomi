using UnityEngine;

/// <summary>
/// アイテムの物理挙動を管理するクラス
/// </summary>
public class ItemFall : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【落下用変数】")]
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _maxGravity = 5.0f;

    [Header("【設地判定用変数】")]
    [SerializeField] private Vector3 _groundCheckHalfExtents = new Vector3(0.45f, 0.1f, 0.45f);
    [SerializeField] private Vector3 _groundCheckOffset = new Vector3(0, 0.1f, 0);

    //現在かかっている重力
    private float _gravityForce = 0.0f;

    //gizmosの色
    private Color _gizmosColor = Color.yellow;

    //---【定数】---//

    private const string WALL_NAME = "Wall";
    private const string GROUND_NAME = "Ground";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// アイテムに重力をかけるメソッド
    /// </summary>
    /// <returns>落下度合い</returns>
    public float Fall()
    {
        if (!GroundCheck())
        {
            _gravityForce -= _gravity * Time.deltaTime;
        }
        else if(_gravityForce < 0.0f)
        {
            _gravityForce = 0.0f;
        }

        //落下速度が上限を超えないように固定する。
        _gravityForce = Mathf.Max(_gravityForce, -_maxGravity);

        return _gravityForce;
    }

    /// <summary>
    /// 出現時の散らばり具合を入れるメソッド
    /// </summary>
    /// <param name="popForce">散らばり具合</param>
    public void SetPopForce(float popForce)
    {
        _gravityForce = popForce;
    }

    /// <summary>
    /// 接地しているかどうかを判定するメソッド：BoxCastで実装
    /// </summary>
    /// <returns>Box内に地面もしくは壁が存在するならtrueを返す</returns>
    private bool GroundCheck()
    {
        //ボックスの始点を決定
        Vector3 center = transform.position + _groundCheckOffset;
        Quaternion rotation = Quaternion.identity;

        //CheckBoxにて、範囲内にWallとGroundが少しでも存在するなら接地と判定
        bool hitObject = Physics.CheckBox(center, _groundCheckHalfExtents, rotation, LayerMask.GetMask(WALL_NAME, GROUND_NAME));

        if (hitObject)
        {
            _gizmosColor = Color.red;
        }
        else
        {
            _gizmosColor = Color.yellow;
        }

        //値を返す
        return hitObject;
    }

#if UNITY_EDITOR
    /// <summary>
    /// GizmosでBoxCastの判定を表示するメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;

        //プレイヤーの中心位置を参照
        Vector3 center = transform.position + _groundCheckOffset;

        //Gimosで判定を表示
        Gizmos.DrawWireCube(center, _groundCheckHalfExtents * 2);
    }
#endif
}
