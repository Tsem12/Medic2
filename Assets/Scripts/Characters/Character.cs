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
    private AttackEvent _latetsAttackEvent;

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
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} finished his turn");
    }


    public virtual bool IsPlaying()
    {
        return _isPlaying;
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

    public void TakeDamage(AttacksObject attack)
    {
        if (attack.atkDamage < 0)
            return;

        _health.TakeDamage(attack.atkDamage);
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
            //Debug.Log($"Patern {_actualPatern.paternName} loaded");
        }

        if(_latetsAttackEvent != null)
        {
            _latetsAttackEvent = null;

            if(_actualPatern.interuptMode == AttacksPatern.PaternInteruptMode.Interupt)
            {
                _actualPatern = null;
                return _latetsAttackEvent.attack.attack;
            }
            else if((_actualPatern.interuptMode == AttacksPatern.PaternInteruptMode.DontInterupt))
            {
                _actualPatern.attackQueue.Enqueue(_latetsAttackEvent.attack);
            }
        }

        AttackClass atk = _actualPatern.attackQueue.Dequeue(); 
        while(!DoesFulFillCondition(atk))
        {

            if(_actualPatern.attackQueue.Count() <= 0)
            {
                _actualPatern = _characterObj.attacksPatern[Random.Range(0, _characterObj.attacksPatern.Count())];
                _actualPatern.FillQueue();
                atk = _actualPatern.attackQueue.Dequeue();
                //Debug.Log($"Patern {_actualPatern.paternName} loaded");

            }
            else
            {
                atk = _actualPatern.attackQueue.Dequeue();
            }
        }

        return atk.attack;

    }
}
