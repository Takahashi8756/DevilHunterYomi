using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class PrologueTextManager : MonoBehaviour
{
    [Header("【取得用変数】")]
    [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();
    [SerializeField] private PrologueAudioManager _prologueAudioManager = default;

    [Header("【テキスト送りの速度】")]
    [SerializeField] private float _textScrollSpeed = 0.1f;

    private int _index = 0;

    public void ToReset()
    {
        foreach(TMP_Text text in _texts)
        {
            text.maxVisibleCharacters = 0;
            text.enabled = false;
        }

        _index = 0;
    }

    public void StartShowText()
    {
        if (_texts.Count <= _index)
        {
            return;
        }

        _texts[_index].enabled = true;
        StartCoroutine(ScrollText());
    }

    public void NextText()
    {
        if (_texts.Count <= _index)
        {
            return;
        }

        _texts[_index].enabled = false;
        _index++;
    }

    private IEnumerator ScrollText()
    {
        WaitForSeconds deley = new WaitForSeconds(_textScrollSpeed);
        int length = _texts[_index].text.Length;

        for(int i = 0; i < length; i++)
        {
            _texts[_index].maxVisibleCharacters = i;
            _prologueAudioManager.PlayTypeAudio();
            yield return deley;
        }

        _texts[_index].maxVisibleCharacters = length;
    }
}
