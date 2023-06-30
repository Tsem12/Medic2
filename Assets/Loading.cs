using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _chara;
    [SerializeField] private GameObject _gfx;
    [SerializeField] private float _durationLenght;
    [SerializeField] private int _offsetForce;

    public UnityEvent loadEvent;

    private SpriteRenderer[] _charaUi;
    private int _count;


    private void Awake()
    {
        _charaUi = _chara.GetComponentsInChildren<SpriteRenderer>();
    }


    public void Load()
    {
        _gfx.SetActive(true);
        _background.DOColor(Color.black, 0.25f);
        _text.DOColor(Color.white, 0.25f);
        foreach(SpriteRenderer sp in _charaUi)
        {
            sp.DOColor(Color.white, 0.25f);
        }
        LauchTextAnim();
    }

    private void LauchTextAnim()
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(_text);
        _count = animator.textInfo.characterCount;
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            Vector3 currCharOffset = animator.GetCharOffset(i);

            Sequence sequence = DOTween.Sequence().SetDelay(i * 0.2f).OnComplete(() =>
            {
                loadEvent?.Invoke();
                _count--;
                CheckTextAnim();
            });
            sequence.Append(animator.DOOffsetChar(i, animator.GetCharOffset(i) + Vector3.up * _offsetForce, _durationLenght / 4).SetEase(Ease.InOutQuad));
            sequence.Append(animator.DOOffsetChar(i, animator.GetCharOffset(i) + Vector3.down * _offsetForce, _durationLenght / 2).SetEase(Ease.InOutQuad));
            sequence.Append(animator.DOOffsetChar(i, currCharOffset, _durationLenght / 4).SetEase(Ease.InOutQuad));
        }

    }

    private void CheckTextAnim()
    {
        if(_count <= 0)
        {
            LauchTextAnim();
        }
    }

}
