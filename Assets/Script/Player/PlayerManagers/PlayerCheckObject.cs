using System;
using UnityEngine;

/// <summary>
/// プレイヤー周辺のオブジェクトを取得するクラス
/// </summary>
public class PlayerCheckObject : MonoBehaviour
{
    [Header("【判定作成用変数】")]
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private Vector3 _boxHalfExtents = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 _boxOffset = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("【取得用変数】")]
    [SerializeField] private PlayerUIManager _playerUIManager = default;

    //取得
    private SpawnEnemy _spawnEnemy = default;

    /// <summary>
    /// 生成時実行するメソッド
    /// </summary>
    /// <param name="mainSceneManager">シーン管理マネージャー</param>
    /// <param name="spawnEnemy">敵のスポーン</param>
    public void ToStart(MainSceneManager mainSceneManager, SpawnEnemy spawnEnemy)
    {
        _spawnEnemy = spawnEnemy;
    }

    /// <summary>
    /// 周辺にあるオブジェクトを取得するメソッド
    /// </summary>
    /// <param name="input">入力</param>
    public void CheckAndInput(PlayerInputManager input)
    {
        BaseGimmick hitGimmick = ObjectCheck();

        if(hitGimmick == null)
        {
            return;
        }

        if (hitGimmick.IsInteract)
        {
            _playerUIManager.InteractText(true);
            hitGimmick.EncountGimmick();
        }

        if (input.IsInteract)
        {
            hitGimmick.InteractGimmick();
        }
    }

    /// <summary>
    /// BoxCastで正面のオブジェクトを参照するメソッド
    /// </summary>
    /// <returns>当たったCollider。なければnull。</returns>
    private BaseGimmick ObjectCheck()
    {
        Vector3 center = transform.position + transform.rotation * _boxOffset;
        Quaternion orientation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, _boxHalfExtents, orientation, _targetLayers);

        if (hits.Length > 0)
        {
            return hits[0].gameObject.GetComponent<BaseGimmick>();
        }

        _playerUIManager.InteractText(false);
        return null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 boxSize = _boxHalfExtents * 2f;
        Gizmos.matrix = transform.localToWorldMatrix;

        Collider[] hits = Physics.OverlapBox(_boxOffset, _boxHalfExtents, transform.rotation, _targetLayers);

        if (hits != null && hits.Length > 0)
        {
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.9f); 
        }
        else
        {
            Gizmos.color = new Color(0.2f, 1f, 0.2f, 0.4f); 
        }

        Gizmos.DrawWireCube(_boxOffset, boxSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}
