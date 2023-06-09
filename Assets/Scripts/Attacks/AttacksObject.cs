using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacks", menuName = "Characters/Attacks")]
public class AttacksObject : ScriptableObject
{

    public enum DotAttacks
    {
        Poison,
        Fire,
        Restrain
    }

    public enum Buff
    {
        Strength,
        Initiative,
        Regenerating,
        Shield,
        ReflectShield,
        Taunt
    }

    public enum DeBuff
    {
        Stun,
        Fatigue,
        Sleeped,
        Disapearance
    }
    public enum AttackEffects
    {
        BaseAttack,
        Dot,
        Buff,
        DeBuff
    }



    [Header("Info")]

    public string attackName;

    [TextArea]
    public string description;

    public Sprite attackSpriteOverRide;


    private bool NoTurnDuration
    {
        get
        {
            return attackEffects == AttackEffects.BaseAttack || deBuff == DeBuff.Sleeped;
        }
    }

    private bool IsBuffWithValue
    {
        get
        {
            return attackEffects == AttackEffects.Buff && buff == Buff.Strength || buff == Buff.Regenerating && attackEffects != AttackEffects.BaseAttack;
        }
    }

    private bool IsDeBuffWithValue
    {
        get
        {
            return attackEffects == AttackEffects.DeBuff && deBuff == DeBuff.Fatigue;
        }
    }
    private bool IsbaseAtk
    {
        get
        {
            return attackEffects == AttackEffects.BaseAttack;
        }
    }

    [Header("Stats")]

    public bool isLifeSteal;
    [Range(0, 10)]
    public int atkDamage;
    public AttackEffects attackEffects;


    [ShowIf("attackEffects", AttackEffects.Dot)]
    public DotAttacks dotAttacks;

    [ShowIf("attackEffects", AttackEffects.Buff)]
    public Buff buff;

    [ShowIf("attackEffects", AttackEffects.DeBuff)]
    public DeBuff deBuff;

    [ShowIf("attackEffects", AttackEffects.Dot)]
    [Range(1, 10)]
    public int dotValuePerTurn;

    [HideIf("NoTurnDuration")]
    [Range(1, 10)]
    public int effectTurnDuration;

    [ShowIf("IsBuffWithValue")]
    [Range(1, 10)]
    [Tooltip("buffValue = atk boost for strenght or heal value per turn for regenerating")]
    public int buffValue;

    [ShowIf("IsDeBuffWithValue")]
    [Range(1, 10)]
    [Tooltip("deBuffValue = atk lower for fatigue")]
    public int deBuffValue;

    [HideIf("IsbaseAtk")]
    [Range(0f, 100f)]
    public float chanceToApplyEffect = 100f;


    public Status GetStatus()
    {
        float random = Random.Range(0f, 100f);

        if (attackEffects == AttackEffects.BaseAttack || random > chanceToApplyEffect)
            return null;


        switch (attackEffects)
        {
            case AttackEffects.Dot:

                switch (dotAttacks)
                {
                    case DotAttacks.Poison:
                        return new Status(Status.StatusEnum.Poisoned, effectTurnDuration, dotValuePerTurn);
                    case DotAttacks.Restrain:
                        return new Status(Status.StatusEnum.Restrained, effectTurnDuration, dotValuePerTurn);
                    case DotAttacks.Fire:
                        return new Status(Status.StatusEnum.Fired, effectTurnDuration, dotValuePerTurn);
                }
                break;
            case AttackEffects.Buff:

                switch (buff)
                {
                    case Buff.Strength:
                        return new Status(Status.StatusEnum.Strengthened, effectTurnDuration, buffValue);
                    case Buff.Initiative:
                        return new Status(Status.StatusEnum.Initiative, effectTurnDuration);
                    case Buff.Regenerating:
                        return new Status(Status.StatusEnum.Regenerating, effectTurnDuration, buffValue);
                    case Buff.Shield:
                        return new Status(Status.StatusEnum.Shielded, effectTurnDuration);
                    case Buff.ReflectShield:
                        return new Status(Status.StatusEnum.ShieldedWithReflect, effectTurnDuration);
                    case Buff.Taunt:
                        return new Status(Status.StatusEnum.Taunting, effectTurnDuration);
                }
                break;
            case AttackEffects.DeBuff:

                switch (deBuff)
                {
                    case DeBuff.Stun:
                        return new Status(Status.StatusEnum.Stunned, effectTurnDuration);
                    case DeBuff.Fatigue:
                        return new Status(Status.StatusEnum.Fatigue, effectTurnDuration, deBuffValue);
                    case DeBuff.Sleeped:
                        return new Status(Status.StatusEnum.Sleeped, true);
                    case DeBuff.Disapearance:
                        return new Status(Status.StatusEnum.Disapeared, effectTurnDuration);
                }
                break;
        }

        return null;
    }


    public Sprite GetAttackSprite(FightManager fm)
    {
        if (attackSpriteOverRide != null)
            return attackSpriteOverRide;

        switch (attackEffects)
        {
            case AttackEffects.BaseAttack:
                return fm.BaseAttack;

            case AttackEffects.Dot:

                switch (dotAttacks)
                {
                    case DotAttacks.Poison:
                        return fm.Poisoned;
                    case DotAttacks.Restrain:
                        return fm.Restrained;
                }
                break;
            case AttackEffects.Buff:

                switch (buff)
                {
                    case Buff.Strength:
                        return fm.Strengthened;
                    case Buff.Initiative:
                        return fm.Initiative;
                    case Buff.Regenerating:
                        return fm.Regenerating;
                    case Buff.Shield:
                        return fm.Shielded;
                }
                break;
            case AttackEffects.DeBuff:

                switch (deBuff)
                {
                    case DeBuff.Stun:
                        return fm.Stunned;
                    case DeBuff.Fatigue:
                        return fm.Fatigue;
                    case DeBuff.Sleeped:
                        return fm.Sleeped;
                }
                break;
        }

        return null;
    }

}
