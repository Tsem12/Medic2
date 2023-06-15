using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUpdater : MonoBehaviour
{
    [SerializeField] AllReferences refs;
    [SerializeField] ManaObject manaObejct;


    private void Start()
    {
        refs.fightManager.OnTurnBegin += TurnMana;
    }

    private void OnDestroy()
    {
        refs.fightManager.OnTurnBegin -= TurnMana;
    }

    void TurnMana()
    {
        manaObejct.AddMana(manaObejct.increaseManaTurn);
        manaObejct.ManaAddTurn();
    }
}
