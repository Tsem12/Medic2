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
    public int increaseManaTurn;

    [HideInInspector]
    public int turnsNumber;
    [HideInInspector]
    public bool manaRestauration;
    [HideInInspector]
    public bool isManaBoost;

    public event Action manaUpdate;
    public event Action manaAddTurn;

    public void AddMana(int amount)
    {
        if (amount > 0f)
        {
            currentMana += amount;
            if (currentMana >= maxMana) currentMana = maxMana;
            manaUpdate?.Invoke();
        }
    }

    public void ReduceMana(int amount)
    {
        if (amount > 0f)
        {
            currentMana -= amount;
            if (currentMana <= 0f) currentMana = 0f;
            manaUpdate?.Invoke();
        }
    }

    public void ManaAddTurn()
    {
        if(manaRestauration)
        {
            manaRestauration = false;
        }
        manaAddTurn?.Invoke();
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
