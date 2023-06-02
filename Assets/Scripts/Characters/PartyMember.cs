using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character, IHealable
{
    

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
        if( _characterObj != null)
        {
            _maxHealth = _characterObj.maxHealth;
            _damage = _characterObj.baseDamage;
            _speed = _characterObj.baseSpeed;
            _agroValue = _characterObj.baseAgroValue;
        }
        CheckObjectRefs();
    }
    public override int GetSpeed()
    {
        return _speed;
    }
    public override int GetAgro()
    {
        return _agroValue;
    }

    public CharacterObjets GetPartyMemberObj() 
    {
        return _characterObj;
    }

    protected override void Attack()
    {
        //if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} is attacking {_refs.fightManager.Enemie.gameObject.name}");

        AttacksObject atk = GetAttack();

        Status status = GetStatus(Status.StatusEnum.Strengthened);
        if(status != null)
        {
            _target.TakeDamage(atk , status.value);
        }
        else
        {
            _target.TakeDamage(atk);
        }
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

    public override Sprite GetIcone()
    {
        return _characterObj.icon;
    }

    public override int GetMaxHealthBar()
    {
        return 1;
    }
}
