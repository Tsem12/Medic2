using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyMemberObjects", menuName = "Characters/PartyMembers")]
public class CharacterObjets : ScriptableObject
{
    [System.Serializable]
    public class AttacksPatern
    {
        public AttacksObject[] attack;
    }
    public enum SpecialAttacksTrigerMode
    {
        Percentage,
        HpAboveAmout,
        HpBelowAmout,
    }

    [Header("Stats")]
    [Space]

    public int maxHealth;
    [Range(1, 3)]
    public int numberOfHealthBar = 1;
    public int baseDamage;
    public int baseSpeed;
    public int baseAgroValue;
    public Sprite icon;

    [Header("Spécial Attacks")]
    [Space]

    public AttacksPatern[] specialAttacks;

    public SpecialAttacksTrigerMode specialAttacksTrigerMode;

    [ShowIf("specialAttacksTrigerMode", SpecialAttacksTrigerMode.Percentage)]
    [Range(0f, 100f)]
    public float chanceToSpecialAttack;

    [ShowIf("specialAttacksTrigerMode", SpecialAttacksTrigerMode.HpAboveAmout)]
    public int maxHpToSpecialAttacks;

    [ShowIf("specialAttacksTrigerMode", SpecialAttacksTrigerMode.HpBelowAmout)]
    public int minHpToSpecialAttacks;


    //[Button]
    //public void TestGetAttacks() => Debug.Log($"{GetAttack(testHp).attackName}");
    //[SerializeField] private int testHp;

    //public AttacksObject GetAttack(int currentHp)
    //{
    //    switch (specialAttacksTrigerMode)
    //    {
    //        case SpecialAttacksTrigerMode.Percentage:

    //            int random = Random.Range(0, 101);
    //            if (random <= chanceToSpecialAttack)
    //            {
    //                return specialAttacks[Random.Range(0, specialAttacks.Length)];
    //            }
    //            else
    //            {
    //                return baseAttack;
    //            }

    //        case SpecialAttacksTrigerMode.HpAboveAmout:

    //            if (currentHp >= maxHpToSpecialAttacks)
    //            {
    //                return specialAttacks[Random.Range(0, specialAttacks.Length)];
    //            }
    //            else
    //            {
    //                return baseAttack;
    //            }
    //        case SpecialAttacksTrigerMode.HpBelowAmout:

    //            if (currentHp <= minHpToSpecialAttacks)
    //            {
    //                return specialAttacks[Random.Range(0, specialAttacks.Length)];
    //            }
    //            else
    //            {
    //                return baseAttack;
    //            }
    //    }
    //    return null;
    //}
}