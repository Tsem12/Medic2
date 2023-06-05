using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUpdater : MonoBehaviour
{
    [SerializeField] AllReferences refs;
    [SerializeField] ManaObject manaObejct;

    bool turnEvent = false;
    int beginTurn;

    Coroutine manaBoostRoutine;
    Coroutine manaRestaurationRoutine;

    private void Start()
    {
        manaObejct.currentMana = manaObejct.maxMana;
        refs.fightManager.OnTurnBegin += TurnMana;
    }

    void Turn()
    {
        turnEvent = true;
    }

    void TurnMana()
    {
        manaObejct.AddMana(manaObejct.increaseManaTurn);
        manaObejct.ManaAddTurn();
    }
}
