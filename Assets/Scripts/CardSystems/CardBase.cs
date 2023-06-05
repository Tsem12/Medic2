using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public enum CardBehaviour
{
    heal,
    unNaturalRegeneration,
    massHeal,
    regeneration,
    resurection,
    antidote,
    spiritShield,
    spiritBarrier,
    resonanceShield,
    blessingOfMars,
    blessingOfJupiter,
    manaBoost,
    manaRestauration,
    speedBoost
}


[CreateAssetMenu(fileName = "CardBase", menuName = "ScriptableObjects/CardBase")]
public class CardBase : ScriptableObject
{
    [Header("If your not a GP don't touch!")]
    public ManaObject manaObject;
    public InputHandlerObject input;
    public AllReferences refs;

    [Space(30)]

    public CardBehaviour cardBehaviour;
    public bool isEnableInGame = true;
    public string cardName;
    public Sprite cardSprite;
    public int manaCost;

    bool doHeal
    {
        get
        {
            return cardBehaviour == CardBehaviour.heal || cardBehaviour == CardBehaviour.regeneration || cardBehaviour == CardBehaviour.unNaturalRegeneration || cardBehaviour == CardBehaviour.massHeal || cardBehaviour == CardBehaviour.antidote;
        }
    }

    bool isTurnDependant
    {
        get
        {
            return cardBehaviour == CardBehaviour.regeneration || cardBehaviour == CardBehaviour.resonanceShield || cardBehaviour == CardBehaviour.blessingOfMars || cardBehaviour == CardBehaviour.blessingOfJupiter || cardBehaviour == CardBehaviour.manaBoost || cardBehaviour == CardBehaviour.speedBoost;
        }
    }

    [ShowIf("doHeal")]
    public int healthHealed;

    [ShowIf("isTurnDependant")]
    public int turnActive;

    [ShowIf("cardBehaviour", CardBehaviour.resurection)]
    public float healthPercentage;

    [ShowIf("cardBehaviour", CardBehaviour.manaBoost)]
    public bool addedMana;

    [ShowIf("cardBehaviour", CardBehaviour.unNaturalRegeneration)]
    public float poisonChance;

    [ShowIf("cardBehaviour", CardBehaviour.spiritShield)]
    public int shieldBreakAfter;

    [ShowIf("cardBehaviour", CardBehaviour.blessingOfMars)]
    public int damageMultiplier;

    [ShowIf("cardBehaviour", CardBehaviour.blessingOfJupiter)]
    public int damageAdded;

    public void ApplyEffectOfTheCard(Character partyMember)
    {
        manaObject.ReduceMana(manaCost);
        if(manaObject.manaRestauration)
        {
            manaObject.AddMana(manaCost);
            manaObject.manaRestauration = false;
        }


        switch (cardBehaviour)
        {

            case CardBehaviour.heal:
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                break;

            case CardBehaviour.unNaturalRegeneration:
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                if (UnityEngine.Random.Range(0f,1f) <= poisonChance/100f)
                {
                    partyMember.AddStatus(new Status(Status.StatusEnum.Poisoned, turnActive));
                }
                else
                {
                    Debug.Log("Poison missed");
                }
                break;

            case CardBehaviour.resurection:
                if(partyMember.GetCurrentHealth() > 0)
                {
                    input.Cancel();
                }
                else
                {
                    partyMember.Revive(healthPercentage);
                }
                break;

            case CardBehaviour.manaRestauration:
                manaObject.manaRestauration = true;
                break;
            case CardBehaviour.manaBoost:
                
                break;
            case CardBehaviour.massHeal:
                foreach (var item in refs.fightManager.PartyMembers)
                {
                    item.GetComponent<IHealable>().Heal(healthHealed);
                }
                break;


        }
    }
}

