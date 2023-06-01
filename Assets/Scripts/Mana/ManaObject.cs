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
    public int increasedMana;
    public int turnsNumber;
    public int newSpellCost;

    public event Action manaStart;
    public event Action manaBoost;
    public event Action manaRestauration;

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

    public void ManaBoost(int turns, int amount)
    {
        turnsNumber = turns;
        increasedMana = amount;
        manaBoost?.Invoke();
    }

    public void ManaRestauration(int cost)
    {
        newSpellCost = cost;
        manaRestauration?.Invoke();
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
