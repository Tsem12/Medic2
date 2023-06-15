using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool isUnlocked;
    public int dataIndex;
    public CardBehaviour cardBehaviour;
    public string cardName;
    public string description;
    public Sprite cardSprite;
    public Sprite cardSpriteGrey;
    public int manaCost;

    bool doHeal
    {
        get
        {
            return cardBehaviour == CardBehaviour.heal || cardBehaviour == CardBehaviour.regeneration  || cardBehaviour == CardBehaviour.massHeal || cardBehaviour == CardBehaviour.panacea;
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


    public bool ApplyEffectOfTheCard(Character partyMember)
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
                if(partyMember.GetCurrentHealth() == partyMember.GetMaxHealth())
                {
                    return false;
                }
                partyMember.GetComponent<IHealable>().Heal(healthHealed);
                partyMember.GetComponent<ICharacter>().GetParticulHandeler().ActiveEffect(ParticulesHandeler.CardEffect.Heal);
                break;
            case CardBehaviour.resurection:
                if(!partyMember.IsDead())
                {
                    return false;
                }
                partyMember.Revive(healthPercentage);
                partyMember.GetComponent<ICharacter>().GetParticulHandeler().ActiveEffect(ParticulesHandeler.CardEffect.Ressurect);
                break;

            case CardBehaviour.manaProfusion:
                manaObject.manaRestauration = true;
                break;

            case CardBehaviour.massHeal:
                int i = 0;
                foreach (var item in refs.fightManager.PartyMembers)
                {
                    if (item.GetCurrentHealth() < item.GetMaxHealth() && !item.IsDead())
                    {
                        item.GetComponent<IHealable>().Heal(healthHealed);
                        partyMember.GetParticulHandeler().ActiveEffect(ParticulesHandeler.CardEffect.Heal);
                    }
                    else
                    {
                        i++;
                    }
                }
                if (i == refs.fightManager.PartyMembers.Length)
                {
                    return false;
                }
                break;
            case CardBehaviour.panacea:
                foreach (var item in partyMember.Status.ToList())
                {
                    partyMember.TryRemoveStatus(item.status);
                }
                if (partyMember.GetCurrentHealth() < partyMember.GetMaxHealth() && !partyMember.IsDead())
                {
                    partyMember.GetComponent<IHealable>().Heal(healthHealed);
                    partyMember.GetComponent<ICharacter>().GetParticulHandeler().ActiveEffect(ParticulesHandeler.CardEffect.Panacea);
                }
                break;

            case CardBehaviour.spiritShield:
                partyMember.AddStatus(new Status(Status.StatusEnum.Shielded, 1));
                partyMember.GetComponent<ICharacter>().GetParticulHandeler().ActiveShield(Status.StatusEnum.Shielded);
                break;

            case CardBehaviour.regeneration:
                partyMember.AddStatus(new Status(Status.StatusEnum.Regenerating,turnActive,healthHealed));
                break;

            case CardBehaviour.resonanceShield:
                partyMember.AddStatus(new Status(Status.StatusEnum.ShieldedWithReflect, turnActive));
                partyMember.GetComponent<ICharacter>().GetParticulHandeler().ActiveShield(Status.StatusEnum.ShieldedWithReflect);
                break;

            case CardBehaviour.initiative:
                partyMember.AddStatus(new Status(Status.StatusEnum.Initiative, 1));
                refs.fightManager.OrderCharacters();
                break;

            case CardBehaviour.blessingOfStrength:
                partyMember.AddStatus(new Status(Status.StatusEnum.Strengthened,1,damageAdded));
                partyMember.GetParticulHandeler().ActiveEffect(Status.StatusEnum.Strengthened);
                break;
        }

        return true;
    }

    [Button("TestSave")]
    public void Save()
    {
        GameData gameData;
        gameData = SaveSystem.Load();
        gameData.spellUnlocked[dataIndex] = isUnlocked;
        SaveSystem.save(gameData);
    }

    [Button("TestLoad")]
    public void Load()
    {
        GameData gameData;
        gameData = SaveSystem.Load();
        isUnlocked = gameData.spellUnlocked[dataIndex];
    }
}

