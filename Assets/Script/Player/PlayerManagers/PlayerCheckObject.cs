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

    [Header("【発見時テキスト】")]
    [SerializeField, TextArea] private string _checkGateText = default;
    [SerializeField, TextArea] private string _pickupTresureText = default;

    //取得
    private SpawnEnemy _spawnEnemy = default;

    //イベント
    public event Action EnterTheGate = default;

    //定数
    private const string GATE_TAG_NAME = "Gate";
    private const string TRESURE_TAG_NAME = "Tresure";

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
        Collider hitObject = ObjectCheck();

        if(hitObject == null)
        {
            return;
        }

        if (hitObject.CompareTag(GATE_TAG_NAME))
        {
            GateManager gateManager = hitObject.GetComponent<GateManager>();
            if (!gateManager.IsShowIcon)
            {
                _playerUIManager.ShowTalkText(_checkGateText);
                gateManager.ShowIcon();
            }
        }

        if (input.IsInteract)
        {
            if (hitObject.CompareTag(GATE_TAG_NAME))
            {
                InTheGate();
            }
            else if (hitObject.CompareTag(TRESURE_TAG_NAME))
            {
                _playerUIManager.ShowTalkText(_pickupTresureText);
                hitObject.GetComponent<TresureDropItem>().OpenTresure();
            }
        }
    }

    /// <summary>
    /// BoxCastで正面のオブジェクトを参照するメソッド
    /// </summary>
    /// <returns>当たったCollider。なければnull。</returns>
    private Collider ObjectCheck()
    {
        Vector3 center = transform.position + transform.rotation * _boxOffset;
        Quaternion orientation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, _boxHalfExtents, orientation, _targetLayers);

        if (hits.Length > 0)
        {
            _playerUIManager.InteractText(true);
            return hits[0];
        }

        _playerUIManager.InteractText(false);
        return null;
    }

    public void InTheGate()
    {
        EnterTheGate?.Invoke();
        _spawnEnemy.SpawnEnd();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 boxSize = _boxHalfExtents * 2f;
        Gizmos.matrix = transform.localToWorldMatrix;

        // 判定内にオブジェクトがいるかをチェック
        Collider[] hits = Physics.OverlapBox(_boxOffset, _boxHalfExtents, transform.rotation, _targetLayers);

        // ヒットがあるときは目立たせる色、ないときは薄い色
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
