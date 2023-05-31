using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaEventHandler", menuName = "ScriptableObjects/ManaEventHandler")]
public class ManaObject : ScriptableObject
{
    public float currentMana;
    public float maxMana = 10f;

    public event Action manaStart;

    public void AddMana(int amount)
    {
        if (amount > 0f)
        {
            currentMana += amount;
            if (currentMana >= maxMana) currentMana = maxMana;
            manaStart?.Invoke();
        }
    }

    public void ReduceMana(int amount)
    {
        if (amount > 0f)
        {
            currentMana -= amount;
            if (currentMana <= 0f) currentMana = 0f;
            manaStart?.Invoke();
        }
    }

    [Button]
    void AddMana()
    {
        AddMana(2);
    }

    [Button]
    void ReduceMana()
    {
        ReduceMana(2);
    }
}
