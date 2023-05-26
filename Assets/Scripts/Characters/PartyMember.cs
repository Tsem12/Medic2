using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    [SerializeField] private PartyMemberObjets _partyMemberObj;

    [Header("Stats")]
    private int _maxHealth;
    private int _damage;
    private int _speed;
    private int _agroValue;

    private ICharacter _target;

    private void Start()
    {
        AssignValues();
    }
    private void AssignValues()
    {
        _maxHealth = _partyMemberObj.maxHealth;
        _damage = _partyMemberObj.baseDamage;
        _speed = _partyMemberObj.baseSpeed;
        _agroValue = _partyMemberObj.baseAgroValue;
    }
    public override int GetSpeed()
    {
        return _speed;
    }
    public override int GetAgro()
    {
        return _agroValue;
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
