using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacks", menuName = "Characters/Attacks")]
public class AttacksObject : ScriptableObject
{
    public enum AttackEffects
    {
        Poison
    }



    [Header("Info")]

    public string attackName;
    [TextArea]
    public string description;


    [Header("Stats")]

    public int atkDamage;
    public AttackEffects attackEffects;

}
