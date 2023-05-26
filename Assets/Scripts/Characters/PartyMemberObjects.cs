using UnityEngine;

[CreateAssetMenu(fileName = "PartyMemberObjects", menuName = "Characters/PartyMembers")]
public class PartyMemberObjets : ScriptableObject
{
    public int maxHealth;
    public int baseDamage;
    public int baseSpeed;
    public int baseAgroValue;
}