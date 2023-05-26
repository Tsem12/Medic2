using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PartyMemberObjects", menuName = "Characters/PartyMembers")]
public class PartyMemberObjets : ScriptableObject
{
    public int speed;
    public int agroValue;
}