using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardBase", menuName = "ScriptableObjects/CardBase")]
public class CardBase : ScriptableObject
{
    public CardBehaviour cardBehaviour;
    public bool isEnableInGame = true;
    public string cardName;
    public int manaCost;
    public ApplyTo applyTo;
    

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

    [ShowIf("cardBehaviour",CardBehaviour.resurection)]
    public bool revive;

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

    public void ApplyEffectOfTheCard(IHealable partyMember)
    {
        switch (cardBehaviour)
        {
            case CardBehaviour.heal:
                Debug.Log("InSwitch");
                partyMember.Heal(2);
                
                break;
        }
    }
}

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

public enum ApplyTo
{
    All,
    Target
}


