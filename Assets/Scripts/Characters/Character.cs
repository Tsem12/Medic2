using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Character : MonoBehaviour, ICharacter
{
    [SerializeField] protected AllReferences _refs;
    [SerializeField] protected Health _health;

    [SerializeField] protected CharacterObjets _characterObj;

    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;
    protected bool _isDead;

    private bool _isPlaying;

    private Coroutine _attackRoutine;
    private AttacksPatern _actualPatern;
    protected AttackEvent _latetsAttackEvent;

    public List<Status> _status = new List<Status>();
    private void OnValidate()
    {
        AssignValues();
    }

    #region Abstarct methods
    public abstract void SetCurrentHealth(int newValue);
    protected abstract void Attack();
    public abstract int GetSpeed();
    public abstract int GetAgro();
    public abstract void SetTarget();
    public abstract void AssignValues();
    public abstract Sprite GetIcone();
    public abstract int GetMaxHealthBar();
    #endregion
    public void CheckObjectRefs()
    {
        //Debug.Log(_characterObj);
       
    }
    public virtual void StartTurn()
    {
        _isPlaying = true;
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} turn started");
        _attackRoutine = StartCoroutine(AttackRoutine());
    }
    public virtual void EndTurn()
    {
        foreach (Status status in _status.ToList())
        {
            ApplyEndTurnStatut(status);
            status.remainTurn--;
            if(status.remainTurn <= 0)
            {
                _status.Remove(status);
            }
        }
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} finished his turn");
    }

    private void ApplyEndTurnStatut(Status statut)
    {
        switch (statut.status)
        {
            case Status.StatusEnum.Poisoned:
                _health.TakeDamage(statut.value);
                break;
            case Status.StatusEnum.Regenerating:
                _health.Heal(statut.value);
                break;
        }
    }


    public virtual bool IsPlaying()
    {
        return _isPlaying;
    }

    public Status GetStatus(Status.StatusEnum status)
    {
        foreach(Status s in _status.ToList())
        {
            if(s.status == status)
            {
                return s;
            }
        }
        return null;
    }

    private IEnumerator AttackRoutine()
    {
        Attack();
        yield return new WaitForSeconds(0.5f);
        _isPlaying = false;
        _attackRoutine = null;
    }

    public string GetName()
    {
        return gameObject.name;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void TakeDamage(AttacksObject attack, int additionalDamage = 0)
    {
        if (attack.atkDamage < 0)
            return;

        Status stun = GetStatus(Status.StatusEnum.Shielded);
        if (stun != null)
        {
            _status.Remove(stun);
            Debug.Log("AttackBloked");
            return;
        }

        _health.TakeDamage(attack.atkDamage + additionalDamage);
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void Kill()
    {
        _isDead = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Revive(int heal)
    {
        _isDead = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        _health.Heal(heal);
    }

    public bool DoesFulFillCondition(AttackClass atk)
    {
        switch (atk.condition)
        {
            case AttackClass.AttackConditions.None:
                return true;

            case AttackClass.AttackConditions.HpLowerThan:
                if ((float)_currentHealth / (float)_maxHealth <= (float)atk.percentageValue / 100f)
                {
                    Debug.Log($"{(float)_currentHealth / (float)_maxHealth} <= {(float)atk.percentageValue / 100f}");
                    return true;
                }
                else
                {
                    return false;
                }
            case AttackClass.AttackConditions.HpHiggerThan:
                if ((float)_currentHealth / (float)_maxHealth >= (float)atk.percentageValue / 100f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        throw new System.Exception("On dois pas arriver là");
    }

    public void TrackSpecialAtkEvents(AttackEvent.SpecialAttacksTrigerMode trigerMode)
    {
                         
        switch (trigerMode)
        {
            case AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar:

                foreach(AttackEvent atk in _characterObj.attacksEvent)
                {
                    if(atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar)
                    {
                        _latetsAttackEvent = atk;
                    }
                }
                break;
            case AttackEvent.SpecialAttacksTrigerMode.AllieBuffed:

                foreach (AttackEvent atk in _characterObj.attacksEvent)
                {
                    if (atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.AllieBuffed)
                    {
                        _latetsAttackEvent = atk;
                    }
                }
                break;
        }
    }
    public AttacksObject GetAttack()
    {
        foreach (AttacksPatern patern in _characterObj.attacksPatern)
        {
            if (patern.attacks.Length <= 0)
            {
                throw new System.Exception("Patern Attack in scriptable object cannot be empty NOOB GD");
            }

            foreach (AttackClass attack in patern.attacks)
            {
                if (attack.attack == null)
                {
                    throw new System.Exception("Attacks in paterns cannot be empty YOU FUCKING NOOB GD SKQCUQVYSCK");
                }
            }
        }


        if (_actualPatern == null || _actualPatern.attackQueue.Count <= 0)
        {
            _actualPatern = _characterObj.attacksPatern[Random.Range(0, _characterObj.attacksPatern.Count())];
            _actualPatern.FillQueue();
        }

        if (_latetsAttackEvent != null)
        {
            switch (_actualPatern.interuptMode)
            {
                case AttacksPatern.PaternInteruptMode.Interupt:

                    _actualPatern = null;
                    _actualPatern = _characterObj.attacksPatern[Random.Range(0, _characterObj.attacksPatern.Count())];
                    _actualPatern.FillQueue();

                    AttacksObject result = _latetsAttackEvent.attack.attack;
                    _latetsAttackEvent = null;
                    return result;

                case AttacksPatern.PaternInteruptMode.DontInteruptLastInQueue:
                    _actualPatern.attackQueue.Enqueue(_latetsAttackEvent.attack);
                    break;

                case AttacksPatern.PaternInteruptMode.DontInteruptFirstInQueue:
                    AttacksObject result2 = _latetsAttackEvent.attack.attack;
                    _latetsAttackEvent = null;
                    return result2;
            }

            _latetsAttackEvent = null;
        }

        AttackClass atk = _actualPatern.attackQueue.Dequeue();
        int nbrLoop = 0;
        while(!DoesFulFillCondition(atk))
        {
            if(nbrLoop > _characterObj.attacksPatern.Length)
            {
                throw new Exception("COMMENT TA REUSSI A FAIRE UNE INFINITE LOOP SALE MERDE");
            }

            if(_actualPatern.attackQueue.Count() <= 0)
            {
                _actualPatern = _characterObj.attacksPatern[Random.Range(0, _characterObj.attacksPatern.Count())];
                _actualPatern.FillQueue();
                atk = _actualPatern.attackQueue.Dequeue();
                nbrLoop++;
            }
            else
            {
                atk = _actualPatern.attackQueue.Dequeue();
            }
        }

        return atk.attack;
    }

    public void AddStatus(Status status)
    {
        Status s = GetStatus(status.status);
        if(s != null)
        {
            s.ResetStatus();
            return;
        }
        _status.Add(status);
    }

    [Button]
    public void TestShield() => AddStatus(new Status(Status.StatusEnum.Shielded, 1));
    [Button]
    public void TestStrenght() => AddStatus(new Status(Status.StatusEnum.Strengthened, 2));
    [Button]
    public void TestInitive()
    {
        AddStatus(new Status(Status.StatusEnum.Initiative, 2));
        _refs.fightManager.OrderCharacters();
    }
    [Button]
    public void TestPoisson() => AddStatus(new Status(Status.StatusEnum.Poisoned, 2, 1));
    [Button]
    public void TestHeal() => AddStatus(new Status(Status.StatusEnum.Regenerating, 2, 1));

}
