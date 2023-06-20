using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnClick = new UnityEvent();
    public UnityEvent OnHold= new UnityEvent();
    public UnityEvent OnRelease = new UnityEvent();

    Coroutine coroutine = null;
    bool release = false;

    private void OnMouseDown()
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(TryHold());
        }
    }

    private void OnMouseUpAsButton()
    {
        release = true;
    }

    IEnumerator TryHold()
    {
        float time = Time.time;
        yield return new WaitForSeconds(0.2f);
        if(release)
        {
            OnClick.Invoke();
            release = false;
            coroutine = null;
            StopCoroutine(coroutine);
        }
        else
        {
            OnHold.Invoke();
            yield return new WaitUntil(() => release);
            release = false;
            OnRelease.Invoke();
            coroutine = null;
            StopCoroutine(coroutine);
        }
    }
}
