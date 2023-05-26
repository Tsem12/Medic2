using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour, ICharacter
{
    [SerializeField] private PartyMemberObjets _partyMemberObj;

    public int GetSpeed()
    {
        return _partyMemberObj.speed;
    }
}
