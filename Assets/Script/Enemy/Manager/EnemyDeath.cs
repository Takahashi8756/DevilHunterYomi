using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵が死亡した時の処理を管理するクラス
/// </summary>
public class EnemyDeath : MonoBehaviour
{
    //========================================================================
    //フィールド変数、及び定数など
    //========================================================================

    [System.Serializable]
    private struct DropItem
    {
        public GameObject _item;
        public float _dropRate;
    }

    [Header("【変数】")]
    [SerializeField] private float _toDeathTime = 0.5f;
    [SerializeField] private int _numberOfDrops = 5;

    [Header("【取得用変数】")]
    [SerializeField] private GameObject _deathParticle = default;
    [SerializeField] private GameObject _parentObject = default;

    [Header("【ドロップアイテム】")]
    [SerializeField] private List<DropItem> _dropItemList = default;

    private float _timer = 0.0f;
    private ItemDirector _itemDirector = default;

    //========================================================================
    //メソッド
    //========================================================================

    /// <summary>
    /// 生成時実行する処理
    /// </summary>
    /// <param name="itemDirector">item管理クラス</param>
    public void ToStart(ItemDirector itemDirector)
    {
        _itemDirector = itemDirector;
    }

    /// <summary>
    /// 死亡時すぐ消えないようディレイをかける用メソッド
    /// </summary>
    public void ToDeathUpdate()
    {
        if( _timer > _toDeathTime)
        {
            _timer = 0.0f;
            Drop();

            GameObject particle = Instantiate(_deathParticle, transform.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Play();

            Destroy(_parentObject);
            return;
        }

        _timer += Time.deltaTime;
    }

    /// <summary>
    /// 配列内に入れたアイテムをランダムでドロップさせるメソッド
    /// </summary>
    private void Drop()
    {
        if(_dropItemList.Count <= 0)
        {
            return;
        }

        for(int i = 0;  i < _numberOfDrops; i++)
        {
            float rate = 0.0f;
            for(int j = 0; j < _dropItemList.Count; j++)
            {
                rate += _dropItemList[j]._dropRate;
            }

            float choiceItem = Random.Range(0.0f, rate);
            float sumRate = 0.0f;
            for(int j = 0;  j < _dropItemList.Count; j++)
            {
                sumRate += _dropItemList[j]._dropRate;
                if(choiceItem <= sumRate)
                {
                    GameObject item = Instantiate(_dropItemList[j]._item, transform.position, Quaternion.identity);
                    _itemDirector.SetItem(item);
                    break;
                }
            }
        }
    }
}
