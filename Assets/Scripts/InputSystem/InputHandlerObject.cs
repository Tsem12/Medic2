using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputHandlerObject", menuName = "ScriptableObjects/InputHandlerObject")]
public class InputHandlerObject : ScriptableObject
{
    public event Action pressedEvent;
    public event Action unPressedEvent;

    public void PressEvent()
    {
        pressedEvent?.Invoke();
    }

    public void UnPressEvent()
    {
        unPressedEvent?.Invoke();
    }
}
