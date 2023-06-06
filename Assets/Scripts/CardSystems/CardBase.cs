using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public enum CardBehaviour
{
    heal,
    massHeal,
    regeneration,
    resurection,
    panacea,
    spiritShield,
    resonanceShield,
    blessingOfStrength,
    manaProfusion,
    initiative
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
    public string cardName;
    public Sprite cardSprite;
    public int manaCost;

    bool doHeal
    {
        get
        {
            return cardBehaviour == CardBehaviour.heal || cardBehaviour == CardBehaviour.regeneration  || cardBehaviour == CardBehaviour.massHeal;
        }
    }

    bool isTurnDependant
    {
        get
        {
            return cardBehaviour == CardBehaviour.regeneration || cardBehaviour == CardBehaviour.resonanceShield;
        }
    }

    [ShowIf("doHeal")]
    public int healthHealed;

    [ShowIf("isTurnDependant")]
    public int turnActive;

    [ShowIf("cardBehaviour", CardBehaviour.resurection)]
    public float healthPercentage;

    [ShowIf("cardBehaviour", CardBehaviour.spiritShield)]
    public int shieldBreakAfter;

    [ShowIf("cardBehaviour", CardBehaviour.blessingOfStrength)]
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
            case CardBehaviour.panacea:
                foreach (var item in partyMember._status)
                {
                    partyMember.TryRemoveStatus(item.status);
                }
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                break;

            case CardBehaviour.spiritShield:
                partyMember.AddStatus(new Status(Status.StatusEnum.Shielded, 1));
                break;

            case CardBehaviour.regeneration:
                partyMember.AddStatus(new Status(Status.StatusEnum.Regenerating,turnActive,healthHealed));
                break;

            case CardBehaviour.resonanceShield:
                partyMember.AddStatus(new Status(Status.StatusEnum.ShieldedWithReflect, turnActive));
                break;

            case CardBehaviour.initiative:
                partyMember.AddStatus(new Status(Status.StatusEnum.Initiative, 1));
                break;

            case CardBehaviour.blessingOfStrength:
                partyMember.AddStatus(new Status(Status.StatusEnum.Strengthened,1,damageAdded));
                break;


        }
    }
}

