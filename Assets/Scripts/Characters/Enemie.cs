using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemie : Character
{
    [SerializeField] private Image _targetIcon;


    [Header("Stats")]
    private int _damage;
    private int _speed;

    private void Start()
    {
        AssignValues();
        _currentHealth = _maxHealth;
        _refs.fightManager.TriggerEvent(AttackEvent.SpecialAttacksTrigerMode.AllieBuffed);

    }
    public override void AssignValues()
    {
        if(_characterObj != null)
        {
            _maxHealth = _characterObj.maxHealth;
            _speed = _characterObj.baseSpeed;
        }
    }
    public override int GetAgro()
    {
        Debug.LogError("Boss have no agro value");
        return 0;
    }

    public override int GetSpeed()
    {
        return _speed;
    }

    public override void SetTarget()
    {
        List<ICharacter> chara =  new List<ICharacter>();
        _targets.Clear();
        ICharacter target = null;

        foreach(ICharacter c in _refs.fightManager.PartyMembers)
        {
            if (!c.IsDead() && c.GetStatus(Status.StatusEnum.Disapeared) == null)
            {
                chara.Add(c);
            }
        }

        if (chara.Count <= 0)
            return;

        chara.Sort(Compare);

        foreach(ICharacter c in chara)
        {
            c.ClearIncommingAttack();
            Status status = c.GetStatus(Status.StatusEnum.Taunting);
            if(status != null)
            {
                for(int i = 0; i < _currentAtkClass.nrbOfTargets; i++)
                {
                    _targets.Add(c);
                    c.SetIncommingAttack(_nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)]);
                }
                return;
            }
        }

        int tempGlobalAgro = _refs.fightManager.GlobalAgro;

        for (int i = 0 ; i < _currentAtkClass.nrbOfTargets; i++)
        {
            int random = Random.Range(0, 101);
            float others = 0f;
            foreach(ICharacter c in chara)
            {
                float percentage = ((float)c.GetAgro() / (float)tempGlobalAgro) *100;
                //Debug.Log($"{percentage + others}, random {random} ");
                if(percentage + others >= random)
                {
                    target = c;
                    break;
                }
                else
                {
                    target = c;
                    others += percentage;
                }
            }

            tempGlobalAgro -= target.GetAgro();
            _targets.Add(target);
            target.SetIncommingAttack(_nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)]);
            chara.Remove(target);
        }

    }

    private int Compare(ICharacter x, ICharacter y)
    {
        if (x.GetAgro() == y.GetAgro()) return 0;
        if (x.GetAgro() == 0) return -1;
        if (x.GetAgro() == 0) return +1;

        return y.GetAgro() - x.GetAgro();
    }

    public override void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public override int GetMaxHealthBar()
    {
        return _characterObj.numberOfHealthBar;
    }
}
