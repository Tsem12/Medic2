using DG.Tweening;
using NaughtyAttributes;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private RectTransform rect;
    Vector3 initScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        initScale = Vector3.zero;

        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(_text);
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible) continue;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence.Join(animator.DOColorChar(i, Vector4.zero, 0));
        }
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible) continue;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence.Append(animator.DOColorChar(i, Color.black, 0));
            sequence.Append(animator.DOPunchCharScale(i, 5f, 1f / animator.textInfo.characterCount * 2));
        }
        sequence.Append(_text.DOScale(10f, 0.3f).SetEase(Ease.InQuart).SetLoops(2, LoopType.Yoyo)).OnComplete(() =>
        {
            for (int i = 0; i < animator.textInfo.characterCount; ++i)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;
                Vector3 currCharOffset = animator.GetCharOffset(i);
                sequence.Join(animator.DOShakeCharOffset(i, 1f, 5f));
            }
        });
    }
    [Button]
    public void Appear()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutCubic);
    }

    [Button]
    public void Disapear()
    {
        rect.localScale = Vector3.one;
        rect.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);

    }
}
