using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Character : MonoBehaviour, ICharacter
{
    public enum PartyMemberEnum
    {
        Boss,
        Berserker,
        Paladin,
        Archer
    }
    [SerializeField] private PartyMemberEnum charaType;
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
    protected List<AttacksObject> _nextPossibleAttacks;
    protected AttacksObject _nextAttack;
    protected List<AttacksObject> _incomingAttacks = new List<AttacksObject>();
    protected AttackClass _currentAtkClass;

    protected List<ICharacter> _targets =  new List<ICharacter>();

    public List<Status> _status = new List<Status>();
    private void OnValidate()
    {
        AssignValues();
    }

    #region Abstarct methods
    public abstract void SetCurrentHealth(int newValue);
    public abstract int GetSpeed();
    public abstract int GetAgro();
    public abstract void SetTarget();
    public abstract void AssignValues();
    public virtual void ClearIncomingAttacks() { }
    public abstract int GetMaxHealthBar();
    public virtual void SetBossAttackPreview(Sprite sprite) { }
    public virtual void SetPartyMemberAttackPreview(Sprite sprite) { }
    public virtual Sprite GetNextAttackSprite() { return null; }

    #endregion
    public void CheckObjectRefs()
    {
        //Debug.Log(_characterObj);
       
    }
    public virtual void StartTurn()
    {
        Status stunned = GetStatus(Status.StatusEnum.Stunned);
        Status restrained = GetStatus(Status.StatusEnum.Restrained);
        Status sleep = GetStatus(Status.StatusEnum.Sleeped);
        Status disapear = GetStatus(Status.StatusEnum.Disapeared);

        if (stunned != null || restrained != null || sleep != null || disapear != null)
        {
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"{gameObject.name} can't attack");
            return;
        }

        _isPlaying = true;
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} turn started");
        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    public void SetAttack()
    {
        _nextPossibleAttacks = GetAttack();
    }

    public virtual void EndTurn()
    {
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} finished his turn");
    }

    public void CheckStatus()
    {
        foreach (Status status in _status.ToList())
        {
            ApplyEndTurnStatut(status);
            if (!status.isInfinite)
            {
                status.remainTurn--;
                if (status.remainTurn <= 0)
                {
                    TryRemoveStatus(status.status);
                }
            }
        }
    }

    private void ApplyEndTurnStatut(Status statut)
    {
        switch (statut.status)
        {
            case Status.StatusEnum.Poisoned:
                _health.TakeDamage(statut.value);
                break;
            case Status.StatusEnum.Fired:
                _health.TakeDamage(statut.value);
                break;
            case Status.StatusEnum.Restrained:
                _health.TakeDamage(statut.value);
                break;
            case Status.StatusEnum.Regenerating:
                _health.Heal(statut.value, true);
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

    protected void Attack()
    {
        if (_refs.fightManager.EnableDebug)
        {
            if(_targets.Count == 1)
                Debug.Log($"{gameObject.name} is attacking {_targets[0].GetName()}");
            else if(_targets.Count == 2)
                Debug.Log($"{gameObject.name} is attacking {_targets[0].GetName()} and {_targets[1].GetName()}");
            else
                Debug.Log($"{gameObject.name} is attacking {_targets[0].GetName()},  {_targets[1].GetName()} and {_targets[2].GetName()}");
        }


        int additionalDamage = 0;

        Status strengthned = GetStatus(Status.StatusEnum.Strengthened);
        Status fatigue = GetStatus(Status.StatusEnum.Fatigue);

        if(charaType == PartyMemberEnum.Berserker)
        {
            additionalDamage += _maxHealth - _currentHealth;
        }

        if (strengthned != null)
        {
            additionalDamage += strengthned.value;
        }
        if (fatigue != null)
        {
            additionalDamage -= fatigue.value;
        }

        foreach(ICharacter target in _targets)
        {

            Status s = target.GetStatus(Status.StatusEnum.ShieldedWithReflect);
            AttacksObject atk = _nextPossibleAttacks[Random.Range(0,_nextPossibleAttacks.Count)];
            if (s != null)
            {
                AddStatus(atk.GetStatus());
                TakeDamage(atk, additionalDamage);
            }
            else
            {
                target.AddStatus(atk.GetStatus());
                target.TakeDamage(atk, additionalDamage);
            }

        }

    }

    public virtual void SetIncommingAttack(AttacksObject atk, int index = 0)
    {
        _incomingAttacks.Add(atk);
    }

    public void ClearIncommingAttack()
    {
        if(_incomingAttacks != null)
        {
            _incomingAttacks.Clear();
        }
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

        Status shield = GetStatus(Status.StatusEnum.Shielded);
        if (shield != null)
        {
            Debug.Log("AttackBloked");
            return;
        }

        _health.TakeDamage(Mathf.Max(attack.atkDamage + additionalDamage, 0));
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public AttacksObject GetNextAttack()
    {
        return _nextAttack;
    }

    public void Kill()
    {
        _status.Clear();
        _isDead = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

    public void Revive(float heal)
    {
        if (!_isDead)
            return;

        _isDead = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        _refs.fightManager.PartyMembersList.Add(GetComponent<ICharacter>());
        _health.Heal((int) (heal / 100f * _maxHealth), true);
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
                    //Debug.Log($"{(float)_currentHealth / (float)_maxHealth} <= {(float)atk.percentageValue / 100f}");
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

            case AttackClass.AttackConditions.HpBarLost:
                Debug.Log(_characterObj.numberOfHealthBar - _health.CurrentHealthBarAmount >= atk.value);
                if(_characterObj.numberOfHealthBar - _health.CurrentHealthBarAmount >= atk.value)
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
                    if(atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar && DoesFulFillCondition(atk.attack))
                    {
                        _latetsAttackEvent = atk;
                    }
                }
                break;
            case AttackEvent.SpecialAttacksTrigerMode.AllieBuffed:

                foreach (AttackEvent atk in _characterObj.attacksEvent)
                {
                    if (atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.AllieBuffed && DoesFulFillCondition(atk.attack))
                    {
                        _latetsAttackEvent = atk;
                    }
                }
                break;
        }
    }
    public List<AttacksObject> GetAttack()
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

                    List<AttacksObject> result = new List<AttacksObject>();
                    result.Add(_latetsAttackEvent.attack.attack);
                    _latetsAttackEvent = null;
                    return result;

                case AttacksPatern.PaternInteruptMode.DontInteruptLastInQueue:
                    _actualPatern.attackQueue.Enqueue(_latetsAttackEvent.attack);
                    break;

                case AttacksPatern.PaternInteruptMode.DontInteruptFirstInQueue:
                    List<AttacksObject> result2 = new List<AttacksObject>();
                    result2.Add(_latetsAttackEvent.attack.attack);
                    _latetsAttackEvent = null;
                    return result2;
            }

            _latetsAttackEvent = null;
        }

        List<AttacksObject> result3 = new List<AttacksObject>();
        AttackClass atk = _actualPatern.attackQueue.Dequeue();
        int nbrLoop = 0;

        if(atk.attackConditionsMode == AttackClass.ConditionMode.DontAttackWithoutCondition)
        {
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
        }
        else if (atk.attackConditionsMode == AttackClass.ConditionMode.UseBaseAttackWithoutCondition && atk.condition != AttackClass.AttackConditions.None)
        {
            if (atk.condition == AttackClass.AttackConditions.Random)
            {
                _currentAtkClass = atk;
                result3.Add(atk.attack);
                result3.Add(atk.ConditionalAttack);
                return result3;
            }

            if (DoesFulFillCondition(atk))
            {
                _currentAtkClass = atk;
                result3.Add(atk.ConditionalAttack);
                return result3;
            }
            else
            {
                _currentAtkClass = atk;
                result3.Add(atk.attack);
                return result3;
            }
        }

        _currentAtkClass = atk;
        result3.Add(atk.attack);
        return result3;
    }

    public void TryRemoveStatus(Status.StatusEnum status)
    {
        Status statu = GetStatus(status);
        if(statu != null)
        {
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"The status {status} has been removed from {gameObject.name}");

            if(status == Status.StatusEnum.Disapeared)
            {
                _refs.fightManager.CharacterList.Add(GetComponent<ICharacter>());

                transform.DOShakeScale(0.25f).SetEase(Ease.InOutFlash).OnComplete(() => GetComponent<SpriteRenderer>().enabled = true);
            }

            _status.Remove(statu);
            return;
        }

    }

    public void AddStatus(Status status)
    {
        if (status == null)
            return;

        Status s = GetStatus(status.status);
        if(s != null)
        {
            switch (s.status)
            {
                case Status.StatusEnum.Fatigue:

                    if (_refs.fightManager.EnableDebug)
                        Debug.Log($"the status {status.status} of {gameObject.name} has been applied twice it has turned into {Status.StatusEnum.Sleeped}");
                    _status.Add(new Status(Status.StatusEnum.Sleeped, true));
                    TryRemoveStatus(Status.StatusEnum.Fatigue);
                    TryRemoveStatus(Status.StatusEnum.Stunned);
                    break;

                case Status.StatusEnum.Stunned:
                    if (_refs.fightManager.EnableDebug)
                        Debug.Log($"{gameObject.name} already got the status: {status.status} it has been reseted");
                    TryRemoveStatus(Status.StatusEnum.Sleeped);
                    break;

                default:
                    if (_refs.fightManager.EnableDebug)
                        Debug.Log($"{gameObject.name} already got the status: {status.status} it has been reseted");
                    s.ResetStatus();
                    break;
            }
            return;
        }
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} get the status: {status.status}");

        if(status.status == Status.StatusEnum.Disapeared)
        {
            _refs.fightManager.CharacterList.Remove(GetComponent<ICharacter>());
            transform.DOShakeScale(0.25f).SetEase(Ease.InOutFlash).OnComplete(() => GetComponent<SpriteRenderer>().enabled = false);
        }
        _status.Add(status);
        if (status.status == Status.StatusEnum.Taunting)
        {
            _refs.fightManager.ResetTargets();
        }
    }

    [Button]
    public void TestShield() => AddStatus(new Status(Status.StatusEnum.Shielded, 1));
    [Button]
    public void TestStrenght() => AddStatus(new Status(Status.StatusEnum.Strengthened, 2, 1));
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
    [Button]
    public void TestStun() => AddStatus(new Status(Status.StatusEnum.Stunned, 2));
    [Button]
    public void TestSleep() => AddStatus(new Status(Status.StatusEnum.Sleeped, true));
    [Button]
    public void TestFatigue() => AddStatus(new Status(Status.StatusEnum.Fatigue, 2, 1));
    [Button]
    public void TestRestrained() => AddStatus(new Status(Status.StatusEnum.Restrained, 2, 1));
    [Button]
    public void TestFired() => AddStatus(new Status(Status.StatusEnum.Fired, 2, 2));
    [Button]
    public void TestTaunt() 
    {
        AddStatus(new Status(Status.StatusEnum.Taunting, 2));
    } 
    [Button]
    public void TestReflectShield() => AddStatus(new Status(Status.StatusEnum.ShieldedWithReflect, 2));
    [Button]
    public void TestDisapear() => AddStatus(new Status(Status.StatusEnum.Disapeared, 2));
    [Button]
    public void GetAllStatus()
    {
        foreach(Status s in _status)
        {
            Debug.Log(s.status);
        }
    }
    [Button]
    public void TestRevive() => Revive(GetMaxHealth()/2);

}
