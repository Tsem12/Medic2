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
    manaProfusion,
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
            return cardBehaviour == CardBehaviour.heal || cardBehaviour == CardBehaviour.regeneration  || cardBehaviour == CardBehaviour.massHeal || cardBehaviour == CardBehaviour.antidote;
        }
    }

    bool isTurnDependant
    {
        get
        {
            return cardBehaviour == CardBehaviour.regeneration || cardBehaviour == CardBehaviour.resonanceShield || cardBehaviour == CardBehaviour.blessingOfMars || cardBehaviour == CardBehaviour.blessingOfJupiter || cardBehaviour == CardBehaviour.speedBoost;
        }
    }

    [ShowIf("doHeal")]
    public int healthHealed;

    [ShowIf("isTurnDependant")]
    public int turnActive;

    [ShowIf("cardBehaviour", CardBehaviour.resurection)]
    public float healthPercentage;

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
        if (manaObject.manaRestauration)
        {
            manaObject.AddMana(manaCost);
            manaObject.manaRestauration = false;
        }

        switch (cardBehaviour)
        {

            case CardBehaviour.heal:
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                break;
            case CardBehaviour.resurection:
                partyMember.Revive(healthPercentage);
                break;

            case CardBehaviour.manaProfusion:
                manaObject.manaRestauration = true;
                break;

            case CardBehaviour.massHeal:
                foreach (var item in refs.fightManager.PartyMembers)
                {
                    item.GetComponent<IHealable>().Heal(healthHealed);
                }
                break;
            case CardBehaviour.antidote:
                foreach (var item in partyMember._status)
                {
                    partyMember.TryRemoveStatus(item.status);
                }
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                break;

            case CardBehaviour.spiritShield:
                partyMember.AddStatus(new Status(Status.StatusEnum.Shielded));
                break;


        }
    }
}

