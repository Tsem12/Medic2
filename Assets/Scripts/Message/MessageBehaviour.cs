using DG.Tweening;
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
    //public void Appear()
    //{
    //    rect.DOScale()
    //}

    public void Disapear()
    {

    }
}
