using DG.Tweening;
using NaughtyAttributes;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBehaviour : MonoBehaviour
{
    private RectTransform rect;
    Vector3 initScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        initScale = Vector3.zero;
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
