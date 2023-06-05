using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUpdater : MonoBehaviour
{
    [SerializeField] AllReferences refs;
    [SerializeField] ManaObject manaObejct;


    private void Start()
    {
        manaObejct.currentMana = manaObejct.maxMana;
        refs.fightManager.OnTurnBegin += TurnMana;
    }

    void TurnMana()
    {
        manaObejct.AddMana(manaObejct.increaseManaTurn);
        manaObejct.ManaAddTurn();
    }
}
