using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardHandlerObject", menuName = "ScriptableObjects/CardHandlerObject")]
public class CardHandlerObject : ScriptableObject
{
    public bool isChaningCards = false;
    public event Action switchCard;

    public void SwitchUpdate()
    {
        switchCard?.Invoke();
    }

    [Button("Test Switch")]
    void Test()
    {
        isChaningCards = true;
        SwitchUpdate();
    }

    [Button("Test ReSwitch")]
    void Test2()
    {
        isChaningCards = false;
        SwitchUpdate();
    }
}
