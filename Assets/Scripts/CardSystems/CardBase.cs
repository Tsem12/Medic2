using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardBase", menuName = "ScriptableObjects/CardBase")]
public class CardBase : ScriptableObject
{
    public string cardName;
    public int manaCost;
    public CardBehaviour cardBehaviour;

    public enum CardBehaviour
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


