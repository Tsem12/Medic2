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
    }

    public enum Buff
    {
        Evade,
    }

    public enum DeBuff
    {
        Stun,
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

    public Sprite attackSprite;



    [Header("Stats")]

    [Range(0, 10)]
    public int atkDamage;
    public AttackEffects attackEffects;

    [HideIf("attackEffects", AttackEffects.BaseAttack)]
    [Range(1, 10)]
    public int effectTurnDuration;

    [ShowIf("attackEffects", AttackEffects.Dot)]
    public DotAttacks dotAttacks;

    [ShowIf("attackEffects", AttackEffects.Buff)]
    public Buff buff;

    [ShowIf("attackEffects", AttackEffects.DeBuff)]
    public DeBuff deBuff;

    [HideIf("attackEffects", AttackEffects.BaseAttack)]
    [Range(0f, 100f)]
    public float chanceToApplyEffect;

}
