using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterObjects", menuName = "Characters/CharacterObjects")]
public class CharacterObjets : ScriptableObject
{
    public enum BossType
    {
        None,
        Orc,
        Cat,
        Kraqueen,
        Mage
    }
    public BossType bossType;
    public string inGameName;
    public Message[] messages;
    [Header("Icons")]
    public Sprite happyFace;
    public Sprite angryFace;
    public Sprite disguestedFace;
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