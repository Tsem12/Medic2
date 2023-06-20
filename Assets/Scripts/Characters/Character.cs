using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
    [Header("Refs")]
    [SerializeField] protected CharacterObjets characterObj;
    [SerializeField] private MessageBehaviour message;
    [SerializeField] protected AllReferences _refs;
    [SerializeField] private Health health;
    [SerializeField] protected StatusBarManager _statusBar;
    [SerializeField] private ParticulesHandeler _particuleHandler;
    [SerializeField] protected Transform _gfx;
    [SerializeField] private Transform characterGfx;
    [SerializeField] protected PartyMemberEnum charaType;
    [SerializeField] private Animator _animator;

    [Header("Health")]
    protected int _maxHealth;
    protected int _currentHealth;
    protected bool _isDead;

    [Header("Attacks")]
    private Coroutine _attackRoutine;
    protected AttacksPatern _actualPatern;
    protected AttackEvent _latetsAttackEvent;
    protected List<AttacksObject> _nextPossibleAttacks;
    protected List<AttacksObject> _incomingAttacks = new List<AttacksObject>();
    protected List<AttacksObject> _targetsAttacks =  new List<AttacksObject>();
    protected AttacksObject _nextAttack;
    protected AttackClass _currentAtkClass;

    [Header("Status")]
    private Status.StatusEnum _statusToApply;
    public List<Status> Status { get => _statusList; set => _statusList = value; }
    public CharacterObjets CharacterObj { get => characterObj; set => characterObj = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public ParticulesHandeler ParticuleHandler { get => _particuleHandler; set => _particuleHandler = value; }
    public Transform CharacterGfx { get => characterGfx; set => characterGfx = value; }
    public MessageBehaviour Message { get => message; set => message = value; }
    public Health Health { get => health; set => health = value; }

    private List<Status> _statusList = new List<Status>();


    private bool _isPlaying;
    protected List<ICharacter> _targets =  new List<ICharacter>();


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
    public ParticulesHandeler GetParticulHandeler()
    {
        return ParticuleHandler;
    }
    public virtual void StartTurn()
    {
        Status stunned = GetStatus(global::Status.StatusEnum.Stunned);
        Status restrained = GetStatus(global::Status.StatusEnum.Restrained);
        Status sleep = GetStatus(global::Status.StatusEnum.Sleeped);
        Status disapear = GetStatus(global::Status.StatusEnum.Disapeared);

        List<Status> list = new List<Status>();
        if(stunned != null ) list.Add(stunned);
        if(restrained != null ) list.Add(restrained);
        if(sleep != null ) list.Add(sleep);


        if(disapear != null && disapear.remainTurn <= 1)
        {
            TryRemoveStatus(global::Status.StatusEnum.Disapeared);
            disapear = null;
        }

        if (stunned != null || restrained != null || sleep != null || disapear != null)
        {
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"{gameObject.name} can't attack");

            if(list.Count >= 1)
            {
                switch (list[Random.Range(0, list.Count)].status)
                {
                    case global::Status.StatusEnum.Sleeped:
                        if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                            Message.DisplayMessage(global::Message.MessageType.Sleeped, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                        break;
                    case global::Status.StatusEnum.Restrained:
                        if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                            Message.DisplayMessage(global::Message.MessageType.Restrained, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                        break;
                    case global::Status.StatusEnum.Stunned:
                        if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                            Message.DisplayMessage(global::Message.MessageType.Stunned, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                        break;

                }
            }
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

    public MessageBehaviour GetMessageBehaviour()
    {
        return Message;
    }

    public virtual void EndTurn()
    {
        if (IsDead())
            return;
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} finished his turn");
        foreach(Status status in Status.ToList())
        {
            if(status != null)
            {
                ParticuleHandler?.ActiveEffect(status.status);
            }
        }
        CheckStatus();
        UpdateBar();
    }

    public void CheckStatus()
    {
        foreach (Status status in Status.ToList())
        {
            switch(status.status)
            {
                case global::Status.StatusEnum.Stunned: 
                    _refs.audioManager.Play("StatusStunned");
                    break;

                case global::Status.StatusEnum.Restrained:
                    _refs.audioManager.Play("StatusRestrained");
                    break;

                case global::Status.StatusEnum.Sleeped:
                    _refs.audioManager.Play("StatusSleep");
                    break;
                case global::Status.StatusEnum.Strengthened:
                    _refs.audioManager.Play("StatusStrengthened");
                    break;
            }

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
            case global::Status.StatusEnum.Poisoned:
                _refs.audioManager.Play("StatusPoison");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Poisoned, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                Health.TakeDamage(statut.value);
                break;
            case global::Status.StatusEnum.Fired:

                if(Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue +1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Fired, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);

                _refs.audioManager.Play("StatusBurning");
                Health.TakeDamage(statut.value);
                break;
            case global::Status.StatusEnum.Restrained:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Restrained, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);

                _refs.audioManager.Play("StatusRestrained");
                Health.TakeDamage(statut.value);
                break;
            case global::Status.StatusEnum.Regenerating:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Heal, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                _refs.audioManager.Play("StatusRegen");
                Health.Heal(statut.value, true);
                break;
        }
    }


    public virtual bool IsPlaying()
    {
        return _isPlaying;
    }

    public CharacterObjets getCharaObj()
    {
        return CharacterObj;
    }
    public Status GetStatus(Status.StatusEnum status)
    {
        foreach(Status s in Status.ToList())
        {
            if(s.status == status)
            {
                return s;
            }
        }
        return null;
    }
    public void ClearAllStatus()
    {
        foreach(Status status in Status.ToList())
        {
            TryRemoveStatus(status.status);
        }
    }
    public virtual void Attack()
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

        Status strengthned = GetStatus(global::Status.StatusEnum.Strengthened);
        Status fatigue = GetStatus(global::Status.StatusEnum.Fatigue);

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

        if(_currentAtkClass.selfStatus != global::Status.StatusEnum.None)
        {
            AddStatus(GetStatus(_currentAtkClass.selfStatus, _currentAtkClass.selfStatusDuration, 1, 1, 1));
        }

        int index = 0;
        foreach(ICharacter target in _targets.ToList())
        {

            //Debug.Log($" attack : {_targetsAttacks[index]}, patern {_actualPatern.paternName}");
            Status disapear = target.GetStatus(global::Status.StatusEnum.Disapeared);
            Status shield = target.GetStatus(global::Status.StatusEnum.Shielded);
            Status s = target.GetStatus(global::Status.StatusEnum.ShieldedWithReflect);
            if(disapear != null || shield != null)
            {
                Debug.Log("AttackBloked");
                if (target.GetMessageBehaviour() != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                {
                    target.GetMessageBehaviour().DisplayMessage(global::Message.MessageType.Shielded, target.getCharaObj(), _refs.fightManager.Enemie.CharacterObj.bossType);
                }
                continue;
            }
            if (s != null)
            {
                TakeDamage(_targetsAttacks[index], additionalDamage);
                AddStatus(_targetsAttacks[index].GetStatus());
            }
            else
            {
                target.TakeDamage(_targetsAttacks[index], additionalDamage);
                target.AddStatus(_targetsAttacks[index].GetStatus());
                if (_targetsAttacks[index].isLifeSteal)
                {
                    Health.Heal(Mathf.Max(_targetsAttacks[index].atkDamage + additionalDamage, 0), charaType != PartyMemberEnum.Boss);
                }
                if (_targetsAttacks[index].isShuffle)
                {
                    _refs.cardManager.ShuffleObject();
                    _refs.fightManager.ShuffleParticles.Play();
                }
            }
            index++;

        }

    }

    public virtual void SetIncommingAttack(AttacksObject atk, int index = 0)
    {
        if (!IsDead())
        {
            _incomingAttacks.Add(atk);
        }
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
        _animator?.SetInteger("AttackIndex", _targetsAttacks[0].attackAnimIndex);
        _animator?.SetTrigger("TriggerAtk");
        _refs.audioManager.Play(_targetsAttacks[0].attackSound);
        //if(charaType == PartyMemberEnum.Boss)
        //{
        //    yield return new WaitForSeconds(1.5f);
        //}
        //yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        yield return new WaitForSeconds(_targetsAttacks[0].animDuration);
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

        Health.TakeDamage(Mathf.Max(attack.atkDamage + additionalDamage, 0));
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
        ClearAllStatus();
        _refs.fightManager.CharacterList.Remove(GetComponent<ICharacter>());
        if (Message != null)
            Message.DisplayMessage(global::Message.MessageType.Die, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
        _isDead = true;
        CharacterGfx.gameObject.SetActive(false);
        _particuleHandler.ActiveEffect(ParticulesHandeler.CardEffect.Die);
        _particuleHandler.StopAllParticles();
    }

    public void Revive(float heal)
    {
        if (!_isDead)
            return;

        if(charaType != PartyMemberEnum.Boss)
        {
            _refs.fightManager.PartyMembersList.Add(GetComponent<ICharacter>());
        }

        _isDead = false;
        _refs.fightManager.CharacterList.Add(GetComponent<ICharacter>());
        if (Message != null)
            Message.DisplayMessage(global::Message.MessageType.Revive, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
        SetTarget();
        _refs.fightManager.OrderCharacters();
        SetPartyMemberAttackPreview(GetNextAttackSprite());
        Health.Heal((int) (heal / 100f * _maxHealth), true);
        CharacterGfx.DOShakeScale(0.25f, 0.05f).SetEase(Ease.InFlash).SetDelay(0.1f).OnPlay(() => CharacterGfx.gameObject.SetActive(true));
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
                //Debug.Log($"{_health.CurrentHealthBarAmount} >= {atk.value}");
                if (CharacterObj.numberOfHealthBar - Health.CurrentHealthBarAmount >= atk.value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case AttackClass.AttackConditions.HpBarNotLost:
                //Debug.Log(_characterObj.numberOfHealthBar - _health.CurrentHealthBarAmount >= atk.value);
                if (CharacterObj.numberOfHealthBar - Health.CurrentHealthBarAmount > atk.value)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            case AttackClass.AttackConditions.HpBarEqual:
                if (Health.CurrentHealthBarAmount == atk.value)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case AttackClass.AttackConditions.CanAttack:
                Status stunned = GetStatus(global::Status.StatusEnum.Stunned);
                Status restrained = GetStatus(global::Status.StatusEnum.Restrained);
                Status sleep = GetStatus(global::Status.StatusEnum.Sleeped);
                Status disapear = GetStatus(global::Status.StatusEnum.Disapeared);

                if (stunned != null || restrained != null || sleep != null || disapear != null)
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

    public void TrackSpecialAtkEvents(AttackEvent.SpecialAttacksTrigerMode trigerMode, int value)
    {
                         
        switch (trigerMode)
        {
            case AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar:

                foreach(AttackEvent atk in CharacterObj.attacksEvent)
                {
                    if(atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar) //&& DoesFulFillCondition(atk.attack)
                    {
                        if(atk.HpOccurMode == AttackEvent.HealthBarOccurMode.AlwaysTrigger)
                        {
                            _latetsAttackEvent = atk;
                        }
                        else if(atk.HpOccurMode == AttackEvent.HealthBarOccurMode.TriggerOnce && value == atk.numberHealthBarLeft)
                        {
                            _latetsAttackEvent = atk;
                        }
                    }
                }
                break;
            case AttackEvent.SpecialAttacksTrigerMode.AllieBuffed:

                foreach (AttackEvent atk in CharacterObj.attacksEvent)
                {
                    if (atk.trigerMode == AttackEvent.SpecialAttacksTrigerMode.AllieBuffed) //&& DoesFulFillCondition(atk.attack)
                    {
                        _latetsAttackEvent = atk;
                    }
                }
                break;
        }
    }
    public List<AttacksObject> GetAttack()
    {
        foreach (AttacksPatern patern in CharacterObj.attacksPatern)
        {
            if (patern.attacks.Length <= 0)
            {
                throw new System.Exception("Patern Attack in scriptable object cannot be empty NOOB GD");
            }

            foreach (AttackClass attack in patern.attacks)
            {
                foreach(AttacksObject atkObj in attack.attack)
                {
                    if (attack.attack == null)
                    { 
                        throw new System.Exception("Y'a une couille dans le paté (ou une gd issue) mais y'a un élément vide dans la liste d'attaque");
                    }
                }
            }
        }


        if (_actualPatern == null || _actualPatern.attackQueue.Count <= 0)
        {
            _actualPatern = CharacterObj.attacksPatern[Random.Range(0, CharacterObj.attacksPatern.Count())];
            _actualPatern.FillQueue();
        }

        if (_latetsAttackEvent != null)
        {
            switch (_actualPatern.interuptMode)
            {
                case AttacksPatern.PaternInteruptMode.Interupt:

                    _actualPatern = null;
                    _actualPatern = _latetsAttackEvent.attack;
                    _actualPatern.FillQueue();
                    AttackClass atcClass = _actualPatern.attackQueue.Dequeue();
                    List<AttacksObject> result = new List<AttacksObject>();
                    foreach (AttacksObject atc in atcClass.attack)
                    {
                        result.Add(atc);
                    }
                    //List<AttacksObject> result = new List<AttacksObject>();
                    //foreach(AttacksObject atc in _latetsAttackEvent.attack.attack)
                    //{
                    //    result.Add(atc);
                    //}
                    _currentAtkClass = atcClass;
                    _statusToApply = atcClass.selfStatus;
                    _latetsAttackEvent = null;
                    return result;
                case AttacksPatern.PaternInteruptMode.DontInteruptLastInQueue:
                    foreach(AttackClass atckObj in _latetsAttackEvent.attack.attacks)
                    {
                        _actualPatern.attackQueue.Enqueue(atckObj);
                    }
                    break;

                case AttacksPatern.PaternInteruptMode.DontInteruptFirstInQueue:
                    List<AttackClass> copyQueue = new List<AttackClass>();
                    List<AttacksObject> result2 = new List<AttacksObject>();
                    while(_actualPatern.attackQueue.Count <= 0)
                    {
                        copyQueue.Add(_actualPatern.attackQueue.Dequeue());
                    }

                    foreach(AttackClass atkClass in _latetsAttackEvent.attack.attacks)
                    {
                        _actualPatern.attackQueue.Enqueue(atkClass);
                    }
                    foreach(AttackClass lastQueue in copyQueue)
                    {
                        _actualPatern.attackQueue.Enqueue(lastQueue);
                    }
                    AttackClass attack = _actualPatern.attackQueue.Dequeue();
                    foreach (AttacksObject atc in attack.attack)
                    {
                        result2.Add(atc);
                    }
                    _statusToApply = attack.selfStatus;
                    _currentAtkClass = attack;
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
                if(nbrLoop > 10000)
                {
                    throw new Exception("COMMENT TA REUSSI A FAIRE UNE INFINITE LOOP SALE MERDE");
                }

                if(_actualPatern.attackQueue.Count() <= 0)
                {
                    _actualPatern = CharacterObj.attacksPatern[Random.Range(0, CharacterObj.attacksPatern.Count())];
                    _actualPatern.FillQueue();
                    _statusToApply = atk.selfStatus;
                    atk = _actualPatern.attackQueue.Dequeue();
                    nbrLoop++;
                }
                else
                {
                    _statusToApply = atk.selfStatus;
                    atk = _actualPatern.attackQueue.Dequeue();
                }
            }

            _currentAtkClass = atk;
            foreach (AttacksObject atc in atk.ConditionalAttack)
            {
                result3.Add(atc);
            }
            _statusToApply = atk.selfStatus;
            return result3;
        }
        else if (atk.attackConditionsMode == AttackClass.ConditionMode.UseBaseAttackWithoutCondition && atk.condition != AttackClass.AttackConditions.None)
        {
            if (atk.condition == AttackClass.AttackConditions.Random)
            {
                _currentAtkClass = atk;
                foreach (AttacksObject atc in atk.attack)
                {
                    result3.Add(atc);
                }
                foreach (AttacksObject atc in atk.ConditionalAttack)
                {
                    result3.Add(atc);
                }
                _statusToApply = atk.selfStatus;
                return result3;
            }

            if (DoesFulFillCondition(atk))
            {
                _currentAtkClass = atk;
                foreach (AttacksObject atc in atk.ConditionalAttack)
                {
                    result3.Add(atc);
                }
                _statusToApply = atk.selfStatus;
                return result3;
            }
            else
            {
                _currentAtkClass = atk;
                foreach (AttacksObject atc in atk.attack)
                {
                    result3.Add(atc);
                }
                _statusToApply = atk.selfStatus;
                return result3;
            }
        }

        _currentAtkClass = atk;
        foreach (AttacksObject atc in atk.attack)
        {
            result3.Add(atc);
        }
        return result3;
    }

    public void TryRemoveStatus(Status.StatusEnum status)
    {
        Status statu = GetStatus(status);
        if(statu != null)
        {
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"The status {status} has been removed from {gameObject.name}");

            switch (status)
            {
                case global::Status.StatusEnum.Shielded:
                    _refs.audioManager.Play("ShieldBreak");
                    ParticuleHandler.DeactiveShield(status);
                    break;
                case global::Status.StatusEnum.Disapeared:

                    if (_particuleHandler.Disapear != null && !IsDead())
                    {
                        _particuleHandler.ActiveEffect(ParticulesHandeler.CardEffect.Disapear);
                        transform.DOShakeScale(1).SetEase(Ease.InOutFlash).SetDelay(0.75f).OnPlay(() => CharacterGfx.gameObject.SetActive(true));
                    }
                    break;
                case global::Status.StatusEnum.ShieldedWithReflect:
                    _refs.audioManager.Play("StatusShieldBreak");
                    ParticuleHandler.DeactiveShield(status);
                    break;
                case global::Status.StatusEnum.Taunting:
                    break;
            }

            Status.Remove(statu);
            _statusBar.UpdateBar();
            return;
        }

    }

    public void AddStatus(Status status)
    {
        if (status == null || IsDead())
            return;

        switch (status.status)
        {
            case global::Status.StatusEnum.Shielded:
                _refs.audioManager.Play("StatusShielded");
                if(Message!=null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)  
                    Message.DisplayMessage(global::Message.MessageType.Shielded, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Fatigue:
                _refs.audioManager.Play("StatusFatigue");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Fatigue, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Fired:
                _refs.audioManager.Play("StatusBurning");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Fired, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Poisoned:
                _refs.audioManager.Play("StatusPoison");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Poisoned, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Regenerating:
                _refs.audioManager.Play("StatusRegen");
                //_message.DisplayMessage(Message.MessageType.Regenerating, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Sleeped:
                _refs.audioManager.Play("StatusSleep");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Sleeped, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Strengthened:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Strengthened, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Initiative:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Initiative, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Restrained:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Restrained, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Stunned:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Stunned, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.Disapeared:
                _refs.audioManager.Play("StatusShielded");
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Disapeared, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
            case global::Status.StatusEnum.ShieldedWithReflect:
                if (Message != null && Random.Range(0, _refs.fightManager.ChanceToTriggerAfxDialogue + 1) == 0)
                    Message.DisplayMessage(global::Message.MessageType.Shielded, CharacterObj, _refs.fightManager.Enemie.characterObj.bossType);
                break;
     
        }

        Status s = GetStatus(status.status);
        if(s != null)
        {
            switch (s.status)
            {
                case global::Status.StatusEnum.Fatigue:
                    if (_refs.fightManager.EnableDebug)
                        Debug.Log($"the status {status.status} of {gameObject.name} has been applied twice it has turned into {(global::Status.StatusEnum.Sleeped)}");
                    AddStatus(new Status(global::Status.StatusEnum.Sleeped, true));
                    _statusBar.UpdateBar();
                    TryRemoveStatus(global::Status.StatusEnum.Fatigue);
                    TryRemoveStatus(global::Status.StatusEnum.Stunned);
                    break;

                case global::Status.StatusEnum.Stunned:
                    _refs.audioManager.Play("StatusStunned");
                    if (_refs.fightManager.EnableDebug)
                        Debug.Log($"{gameObject.name} already got the status: {status.status} it has been reseted");
                    TryRemoveStatus(global::Status.StatusEnum.Sleeped);
                    break;
            }
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"{gameObject.name} already got the status: {status.status} it has been reseted");
            s.ResetStatus();
            _statusBar.UpdateBar();
            return;
        }
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} get the status: {status.status}");

        if(status.status == global::Status.StatusEnum.Disapeared)
        {
            if(_particuleHandler.Disapear != null)
            {
                transform.DOShakeScale(0.5f).SetEase(Ease.InOutFlash).OnComplete(() => CharacterGfx.gameObject.SetActive(false));
                _particuleHandler.ActiveEffect(ParticulesHandeler.CardEffect.Disapear);
            }
        }
        Status.Add(status);
        _statusBar.UpdateBar();
    }

    public void UpdateBar()
    {
        _statusBar.UpdateBar();
    }

    [Button]
    public void TestShield() => AddStatus(new Status(global::Status.StatusEnum.Shielded, 1));
    [Button]
    public void TestStrenght() => AddStatus(new Status(global::Status.StatusEnum.Strengthened, 2, 1));
    [Button]
    public void TestInitive()
    {
        AddStatus(new Status(global::Status.StatusEnum.Initiative, 2));
        _refs.fightManager.OrderCharacters();
    }
    [Button]
    public void TestPoisson() => AddStatus(new Status(global::Status.StatusEnum.Poisoned, 2, 1));
    [Button]
    public void TestHeal() => AddStatus(new Status(global::Status.StatusEnum.Regenerating, 2, 1));
    [Button]
    public void TestStun() => AddStatus(new Status(global::Status.StatusEnum.Stunned, 2));
    [Button]
    public void TestSleep() => AddStatus(new Status(global::Status.StatusEnum.Sleeped, true));
    [Button]
    public void TestFatigue() => AddStatus(new Status(global::Status.StatusEnum.Fatigue, true, 1));
    [Button]
    public void TestRestrained() => AddStatus(new Status(global::Status.StatusEnum.Restrained, 2, 1));
    [Button]
    public void TestFired() => AddStatus(new Status(global::Status.StatusEnum.Fired, 2, 2));
    [Button]
    public void TestTaunt() 
    {
        AddStatus(new Status(global::Status.StatusEnum.Taunting, 2));
        _refs.fightManager.ResetTargets();

    } 
    [Button]
    public void TestReflectShield() => AddStatus(new Status(global::Status.StatusEnum.ShieldedWithReflect, 2));
    [Button]
    public void TestDisapear() => AddStatus(new Status(global::Status.StatusEnum.Disapeared, 2));
    [Button]
    public void GetAllStatus()
    {
        foreach(Status s in Status)
        {
            Debug.Log(s.status);
        }
    }
    [Button]
    public void ClearStatus()
    {
        ClearAllStatus();
    }
    [Button]
    public void TestRevive() => Revive(GetMaxHealth()/2);

    public Status GetStatus(Status.StatusEnum statu, int effectTurnDuration, int dotValuePerTurn, int buffValue, int deBuffValue)
    {
        switch (statu)
        {
            case global::Status.StatusEnum.Poisoned:
                return new Status(global::Status.StatusEnum.Poisoned, effectTurnDuration, dotValuePerTurn);
            case global::Status.StatusEnum.Restrained:
                return new Status(global::Status.StatusEnum.Restrained, effectTurnDuration, dotValuePerTurn);
            case global::Status.StatusEnum.Fired:
                return new Status(global::Status.StatusEnum.Fired, effectTurnDuration, dotValuePerTurn);

            case global::Status.StatusEnum.Strengthened:
                return new Status(global::Status.StatusEnum.Strengthened, effectTurnDuration, buffValue);
            case global::Status.StatusEnum.Initiative:
                return new Status(global::Status.StatusEnum.Initiative, effectTurnDuration);
            case global::Status.StatusEnum.Regenerating:
                return new Status(global::Status.StatusEnum.Regenerating, effectTurnDuration, buffValue);
            case global::Status.StatusEnum.Shielded:
                return new Status(global::Status.StatusEnum.Shielded, effectTurnDuration);
            case global::Status.StatusEnum.ShieldedWithReflect:
                return new Status(global::Status.StatusEnum.ShieldedWithReflect, effectTurnDuration);
            case global::Status.StatusEnum.Taunting:
                return new Status(global::Status.StatusEnum.Taunting, effectTurnDuration);

            case global::Status.StatusEnum.Stunned:
                return new Status(global::Status.StatusEnum.Stunned, effectTurnDuration);
            case global::Status.StatusEnum.Fatigue:
                return new Status(global::Status.StatusEnum.Fatigue, true, deBuffValue);
            case global::Status.StatusEnum.Sleeped:
                return new Status(global::Status.StatusEnum.Sleeped, true);
            case global::Status.StatusEnum.Disapeared:
                return new Status(global::Status.StatusEnum.Disapeared, effectTurnDuration);

        }

        return null;
    }

}
