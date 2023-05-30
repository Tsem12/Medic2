using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character, IHealable
{
    [SerializeField] private PartyMemberObjets _partyMemberObj;

    [Header("Stats")]
    private int _damage;
    private int _speed;
    private int _agroValue;

    private ICharacter _target;

    private void Start()
    {
        AssignValues();
        _currentHealth = _maxHealth;
    }
    public override void AssignValues()
    {
        if( _partyMemberObj != null)
        {
            _maxHealth = _partyMemberObj.maxHealth;
            _damage = _partyMemberObj.baseDamage;
            _speed = _partyMemberObj.baseSpeed;
            _agroValue = _partyMemberObj.baseAgroValue;
        }
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
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} is attacking {_refs.fightManager.Enemie.gameObject.name}");
        _target.TakeDamage(_partyMemberObj.baseDamage);
    }

    public override void SetTarget()
    {
        _target = _refs.fightManager.Enemie.GetComponent<ICharacter>();
    }

    public override void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void Heal(int value)
    {
        _health.Heal(value);
    }
}
