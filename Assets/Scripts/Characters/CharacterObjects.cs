using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterObjects", menuName = "Characters/CharacterObjects")]
public class CharacterObjets : ScriptableObject
{

    [Header("Stats")]
    [Space]

    public int maxHealth;
    [Range(1, 5)]
    public int numberOfHealthBar = 1;
    public int baseSpeed;
    public int baseAgroValue;

    [Header("Attacks paterns")]
    [Space]

    public AttacksPatern[] attacksPatern;
    public AttackEvent[] attacksEvent;




}