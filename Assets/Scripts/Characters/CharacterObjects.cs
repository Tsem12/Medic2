using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterObjects", menuName = "Characters/CharacterObjects")]
public class CharacterObjets : ScriptableObject
{

    [Header("Stats")]
    [Space]

    public int maxHealth;
    [Range(1, 3)]
    public int numberOfHealthBar = 1;
    public int baseDamage;
    public int baseSpeed;
    public int baseAgroValue;
    public Sprite icon;

    [Header("Attacks paterns")]
    [Space]

    public AttacksPatern[] attacksPatern;
    public AttackEvent[] attacksEvent;




}