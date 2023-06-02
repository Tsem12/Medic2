using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputHandlerObject", menuName = "ScriptableObjects/InputHandlerObject")]
public class InputHandlerObject : ScriptableObject
{
    public event Action pressedEvent;
    public event Action unPressedEvent;
    public event Action cancelPressEvent;
    public bool wasCanceled = false;

    public void PressEvent()
    {
        pressedEvent?.Invoke();
    }

    public void UnPressEvent()
    {
        if(!wasCanceled)
        {
            unPressedEvent?.Invoke();
        }
        wasCanceled = false;
    }

    public void CancelPress()
    {
        cancelPressEvent?.Invoke();
        wasCanceled = true;
    }
}
