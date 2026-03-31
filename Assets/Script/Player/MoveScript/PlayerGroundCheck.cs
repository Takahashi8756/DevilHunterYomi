using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("【設地判定用変数】")]
    [SerializeField] private Vector3 _groundCheckHalfExtents = new Vector3(0.45f, 0.1f, 0.45f);
    [SerializeField] private Vector3 _groundCheckOffset = new Vector3(0, 0.1f, 0);

    //gizmosの色
    private Color _gizmosColor = Color.yellow;

    //---【定数】---//

    private const string WALL_NAME = "Wall";
    private const string GROUND_NAME = "Ground";

    /// <summary>
    /// 接地しているかどうかを判定するメソッド：BoxCastで実装
    /// </summary>
    /// <returns>Box内に地面もしくは壁が存在するならtrueを返す</returns>
    public bool GroundCheck()
    {
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

        return hitObject;
    }

#if UNITY_EDITOR
    /// <summary>
    /// GizmosでBoxCastの判定を表示するメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Vector3 center = transform.position + _groundCheckOffset;
        Gizmos.DrawWireCube(center, _groundCheckHalfExtents * 2);
    }
#endif
}
