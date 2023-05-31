using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : Character
{
    [SerializeField] private EnemiesObjects _enemieObj;

    private ICharacter _target;

    [Header("Stats")]
    private int _damage;
    private int _speed;

    private void Start()
    {
        AssignValues();
        _currentHealth = _maxHealth;

    }
    public override void AssignValues()
    {
        if(_enemieObj != null)
        {
            _maxHealth = _enemieObj.baseHealth;
            _damage = _enemieObj.baseDamage;
            _speed = _enemieObj.baseSpeed;
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
        ICharacter target = null;

        foreach(ICharacter c in _refs.fightManager.PartyMembers)
        {
            if (!c.IsDead())
            {
                chara.Add(c);
            }
        }

        chara.Sort(Compare);

        int random = Random.Range(0, 101);
        float others = 0f;
        foreach(ICharacter c in chara)
        {
            float percentage = ((float)c.GetAgro() / (float)_refs.fightManager.GlobalAgro) *100;
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

        _target = target;
    }
    protected override void Attack()
    {
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} is attacking {_target.GetName()}");
        _target.TakeDamage(_enemieObj.baseDamage);
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
}
