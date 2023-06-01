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
        heal,
        unstableHeal,
        unNaturalRegeneration,
        massHeal,
        regeneration,
        vampirism,
        godBet,
        spiritGuard,
        spiritShield,
        spiritBarrier,
        blessingOfMars,
        blessingOfJupiter,
        focusBoost,
        allure,
        trap,
        speedBoost,
        fortifyMana
    }

    public void ApplyEffectOfTheCard(IHealable partyMember)
    {
        switch (cardBehaviour)
        {
            case CardBehaviour.heal:
                Debug.Log("InSwitch");
                partyMember.Heal(2);
                
                break;

            case CardBehaviour.unstableHeal:
                Debug.Log("la carte est  unstableheal");
                break;
        }
    }
    
}


