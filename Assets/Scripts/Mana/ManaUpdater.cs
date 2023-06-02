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
        manaObejct.manaBoost += OnManaBoost;
        manaObejct.manaRestauration += OnManaRestauration;
        refs.fightManager.OnTurnBegin += TurnMana;
        refs.fightManager.OnTurnBegin += Turn;
    }

    void OnManaBoost()
    {
        if(manaBoostRoutine == null)
        {
            manaBoostRoutine = StartCoroutine(ManaBoost());
        }
        else
        {
            StopCoroutine(manaBoostRoutine);
            manaBoostRoutine = StartCoroutine(ManaBoost());
        }
    }

    void OnManaRestauration()
    {
        if (manaRestaurationRoutine == null)
        {
            manaRestaurationRoutine = StartCoroutine(ManaRestauration());
        }
        else
        {
            StopCoroutine(manaRestaurationRoutine);
            manaRestaurationRoutine = StartCoroutine(ManaRestauration());
        }
    }


    IEnumerator ManaBoost()
    {
        beginTurn = refs.fightManager.CurrentTurn;
        while (beginTurn + manaObejct.turnsNumber >= refs.fightManager.CurrentTurn)
        {
            manaObejct.AddMana(manaObejct.increasedMana);
            yield return new WaitUntil(() => turnEvent);
            turnEvent = false;
        }
        manaBoostRoutine = null;
    }

    IEnumerator ManaRestauration()
    {
        yield return new WaitUntil(() => turnEvent);
        manaObejct.AddMana(manaObejct.newSpellCost);
        turnEvent = false;
        manaBoostRoutine = null;
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
