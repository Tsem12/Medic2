using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    [SerializeField] private PartyMemberObjets _partyMemberObj;
    private ICharacter _target;
    public override int GetSpeed()
    {
        return _partyMemberObj.speed;
    }
    public override int GetAgro()
    {
        return _partyMemberObj.agroValue;
    }

    public PartyMemberObjets GetPartyMemberObj() 
    {
        return _partyMemberObj;
    }

    protected override void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking {_refs.fightManager.Enemie.gameObject.name}");
    }

    public override void SetTarget()
    {
        _target = _refs.fightManager.Enemie.GetComponent<ICharacter>();
    }

}
