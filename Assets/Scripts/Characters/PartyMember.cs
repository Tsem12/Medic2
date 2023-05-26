using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    [SerializeField] private PartyMemberObjets _partyMemberObj;
    public override int GetSpeed()
    {
        return _partyMemberObj.speed;
    }

    public PartyMemberObjets GetPartyMemberObj() 
    {
        return _partyMemberObj;
    }
}
