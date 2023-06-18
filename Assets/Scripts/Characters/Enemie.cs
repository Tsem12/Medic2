using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemie : Character
{
    [SerializeField] private Image _targetIcon;
    [SerializeField] private TextMeshProUGUI _bossName;

    [Header("Stats")]
    private int _damage;
    private int _speed;


    private void Start()
    {
        _refs.audioManager.Play(_refs.audioManager.StartMusic);
        _refs.audioManager.Pause(_refs.audioManager.StartMusic);
        AssignValues();
        _currentHealth = _maxHealth;
        //_refs.fightManager.TriggerEvent(AttackEvent.SpecialAttacksTrigerMode.AllieBuffed);

        _bossName.text = characterObj.inGameName;
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(_bossName);
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible) continue;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence.Join(animator.DOColorChar(i, Vector4.zero, 0));
        }
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible) continue;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence.Append(animator.DOColorChar(i, Color.black, 0));
            sequence.Append(animator.DOPunchCharScale(i, 5f, 1f / animator.textInfo.characterCount * 2).OnComplete(() => _refs.audioManager.Play("LeterFall")));
        }
        sequence.Append(_bossName.DOScale(10f, 0.3f).SetEase(Ease.InQuart).SetLoops(2, LoopType.Yoyo)).OnComplete(() =>
        {
            _refs.audioManager.Play("LeterBigFall");
            //for (int i = 0; i < animator.textInfo.characterCount; ++i)
            //{
            //    if (!animator.textInfo.characterInfo[i].isVisible) continue;
            //    Vector3 currCharOffset = animator.GetCharOffset(i);
            //    sequence.Join(animator.DOShakeCharOffset(i, 1f, 5f));
            //}
            sequence.Append(_bossName.DOFade(0f, 2f)).SetEase(Ease.Linear).SetDelay(0.1f);
            _refs.fightManager.StartTurn();
            _refs.audioManager.UnPause(_refs.audioManager.StartMusic);
            _refs.audioManager.FadeSound(_refs.audioManager.StartMusic);
        });

    }
    public override void AssignValues()
    {
        if(CharacterObj != null)
        {
            _maxHealth = CharacterObj.maxHealth;
            _speed = CharacterObj.baseSpeed;
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
        Status stun = GetStatus(global::Status.StatusEnum.Stunned);
        Status restrain = GetStatus(global::Status.StatusEnum.Restrained);
        Status disapear = GetStatus(global::Status.StatusEnum.Disapeared);
        Status sleep = GetStatus(global::Status.StatusEnum.Sleeped);

        if(stun != null || restrain != null || sleep != null)
            return;

        if (disapear != null && disapear.remainTurn > 1)
            return;

        List<ICharacter> chara =  new List<ICharacter>();
        _targets.Clear();
        _targetsAttacks.Clear();
        ICharacter target = null;
        AttacksObject atkobj = null;

        foreach(ICharacter c in _refs.fightManager.PartyMembers)
        {
            if (!c.IsDead() && c.GetStatus(global::Status.StatusEnum.Disapeared) == null)
            {
                chara.Add(c);
                c.ClearIncomingAttacks();
            }
        }

        if (chara.Count <= 0)
            return;

        chara.Sort(Compare);

        foreach(ICharacter c in chara)
        {
            c.ClearIncommingAttack();
            Status status = c.GetStatus(global::Status.StatusEnum.Taunting);

            if(c.IsDead())
                break;

            if(status != null)
            {
                for(int i = 0; i < Mathf.Min(_currentAtkClass.nrbOfTargets, _refs.fightManager.PartyMembersList.Count); i++)
                {
                    _targets.Add(c);
                    atkobj = _nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)];
                    _targetsAttacks.Add(atkobj);
                    c.SetIncommingAttack(atkobj, i);
                }
                return;
            }
        }

        int tempGlobalAgro = _refs.fightManager.GlobalAgro;

        for (int i = 0 ; i < Mathf.Min(_currentAtkClass.nrbOfTargets, _refs.fightManager.CharacterList.Count - 1); i++)
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
            atkobj = _nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)];
            _targetsAttacks.Add(atkobj);
            target.SetIncommingAttack(atkobj);
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
        return CharacterObj.numberOfHealthBar;
    }

    public override void ClearIncomingAttacks()
    {
        base.ClearIncomingAttacks();
    }

    public override void Attack()
    {
        base.Attack();
        _gfx.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
    }
}
