using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonJuice : MonoBehaviour
{

    public UnityEvent ButtonAction;


    public void OnButtonClick()
    {
        transform.DOScale(Vector3.one *1.3f, 0.25f).SetEase(Ease.InOutQuint).SetLoops(2, LoopType.Yoyo).OnComplete(() => ButtonAction?.Invoke());
    }

}
