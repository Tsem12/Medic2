using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : Character, IHealable
{
    [Header("Stats")]
    private int _damage;
    private int _speed;
    private int _agroValue;

    [SerializeField] private Image _bossAttackImage;
    [SerializeField] private Image _nextAttackImage;


    private void Start()
    {
        AssignValues();
        _currentHealth = _maxHealth;
        _refs.fightManager.OnTurnEnd += () => _bossAttackImage.gameObject.SetActive(false);
        _refs.fightManager.OnTurnEnd += () => _nextAttackImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _refs.fightManager.OnTurnEnd -= () => _bossAttackImage.gameObject.SetActive(false);
        _refs.fightManager.OnTurnEnd -= () => _nextAttackImage.gameObject.SetActive(false);
    }

    public override void AssignValues()
    {
        if( _characterObj != null)
        {
            _maxHealth = _characterObj.maxHealth;
            _speed = _characterObj.baseSpeed;
            _agroValue = _characterObj.baseAgroValue;
        }
        CheckObjectRefs();
    }
    public override void SetIncommingAttack(AttacksObject atk)
    {
        base.SetIncommingAttack(atk);
        _bossAttackImage.gameObject.SetActive(true);
        _bossAttackImage.sprite = atk.GetAttackSprite(_refs.fightManager);

    }
    public override Sprite GetNextAttackSprite()
    {
         return _nextPossibleAttacks[0].GetAttackSprite(_refs.fightManager);
    }

    public override void SetBossAttackPreview(Sprite sprite)
    {

    }

    public override void SetPartyMemberAttackPreview(Sprite sprite)
    {
        Status stunned = GetStatus(Status.StatusEnum.Stunned);
        Status restrained = GetStatus(Status.StatusEnum.Restrained);
        Status sleep = GetStatus(Status.StatusEnum.Sleeped);

        if (stunned != null || restrained != null || sleep != null || IsDead())
            return;

        _nextAttackImage.gameObject.SetActive(true);
        _nextAttackImage.sprite = sprite;
    }

    public override void EndTurn()
    {
        base.EndTurn();
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

    public override void SetTarget()
    {
        _targets.Clear();
        if(_refs.fightManager.Enemie.GetComponent<ICharacter>().GetStatus(Status.StatusEnum.Disapeared) == null)
            _targets.Add(_refs.fightManager.Enemie.GetComponent<ICharacter>());
    }

    public override void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void Heal(int value)
    {
        _health.Heal(value, true);
    }


    public override int GetMaxHealthBar()
    {
        return 1;
    }
}
