using UnityEngine;

/// <summary>
/// プレイヤーに値を渡す用クラス
/// </summary>
public class GiveValue : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [Header("【判定作成用変数】")]
    [SerializeField] private Vector3 _capsuleStartPoint = Vector3.zero;
    [SerializeField] private Vector3 _capsuleEndPoint = Vector3.zero;
    [SerializeField] private float _capsuleRadius = 1.0f;

    [Header("【取得用変数】")]
    [SerializeField] private SetItemData _setItemData = default;

    //配列
    private Collider[] _overlapResult = new Collider[1];

    //Gizmosの色を保管
    private Color _gizmosColor = Color.green;

    //定数
    private const string PLAYER_LAYER_NAME = "Player";

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// プレイヤーにぶつかったかどうかを検知するメソッド
    /// </summary>
    /// <returns>取得したPlayerStateを管理</returns>
    private PlayerState PlayerCheck()
    {
        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        //CheckCapsuleではコンポーネントの取得が出来なかったため、OverlapCapsuleNonAllocにて判定を実装
        int count = Physics.OverlapCapsuleNonAlloc(startPoint, endPoint, _capsuleRadius, _overlapResult, LayerMask.GetMask(PLAYER_LAYER_NAME));

        if (count > 0)
        {
            PlayerState playerState = _overlapResult[0].GetComponent<PlayerState>();
            _gizmosColor = Color.red;
            return playerState;
        }

        _gizmosColor = Color.green;
        return null;
    }

    /// <summary>
    /// プレイヤーに加算するステータス情報を渡すメソッド
    /// </summary>
    public void Give()
    {
        PlayerState state = PlayerCheck();

        if(state == null)
        {
            return;
        }

        ItemData itemData = _setItemData.ItemData;
        state.GetItem(itemData);

        Destroy(this.gameObject);
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディター上で範囲を見える用にするためのメソッド
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;

        Vector3 center = transform.position;
        Vector3 startPoint = center + _capsuleStartPoint;
        Vector3 endPoint = center + _capsuleEndPoint;

        Gizmos.DrawWireSphere(startPoint, _capsuleRadius);
        Gizmos.DrawWireSphere(endPoint, _capsuleRadius);
    }
#endif
}
