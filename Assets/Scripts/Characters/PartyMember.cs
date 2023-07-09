using DG.Tweening;
using NaughtyAttributes;
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
    [SerializeField] private BossAttacksIndicator _bossIndicator;
    [SerializeField] private RectTransform _arrowForBoss;


    private void Start()
    {
        AssignValues();
        _currentHealth = _maxHealth;
        _refs.fightManager.OnTurnEnd += () => _bossIndicator.Clear();
        _refs.fightManager.OnTurnEnd += () => _arrowForBoss.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _refs.fightManager.OnTurnEnd -= () => _bossIndicator.Clear();
        _refs.fightManager.OnTurnEnd -= () => _arrowForBoss.gameObject.SetActive(false);
        DOTween.KillAll();
    }

    private void Awake()
    {
        //_arrowForBoss.DOLocalMoveY(0.05f, 0.25f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
        Sequence sequence = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo);
        sequence.Append(_arrowForBoss.DOAnchorPosY(270, 0.5f).SetEase(Ease.OutFlash));
        sequence.Join(_arrowForBoss.DOScale(Vector3.one * .5f, 0.15f).SetEase(Ease.OutBounce));
    }

    public override void AssignValues()
    {
        if( CharacterObj != null)
        {
            _maxHealth = CharacterObj.maxHealth;
            _speed = CharacterObj.baseSpeed;
            _agroValue = CharacterObj.baseAgroValue;
        }
        CheckObjectRefs();
    }
    public override void ClearIncomingAttacks()
    {
        _bossIndicator.Clear();
    }
    public override void SetIncommingAttack(AttacksObject atk, int index = 0)
    {
        base.SetIncommingAttack(atk);
        _bossIndicator.SetSprite(atk.GetAttackSprite(_refs.fightManager), index);

    }
    public override Sprite GetNextAttackSprite()
    {
         return _targetsAttacks[0].GetAttackSprite(_refs.fightManager);
    }

    public override void SetBossAttackPreview(Sprite sprite)
    {

    }

    public override void SetPartyMemberAttackPreview(Sprite sprite)
    {
        Status stunned = GetStatus(global::Status.StatusEnum.Stunned);
        Status restrained = GetStatus(global::Status.StatusEnum.Restrained);
        Status sleep = GetStatus(global::Status.StatusEnum.Sleeped);

        if (stunned != null || restrained != null || sleep != null || IsDead())
            return;

        _arrowForBoss.gameObject.SetActive(true);
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
        return CharacterObj;
    }

    public override void SetTarget()
    {
        _targets.Clear();
        _targetsAttacks.Clear();
        _targets.Add(_refs.fightManager.Enemie.GetComponent<ICharacter>());
        AttacksObject atk = _nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)];
        _targetsAttacks.Add(atk);
        _refs.fightManager.Enemie.SetIncommingAttack(atk);

    }

    public override void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void Heal(int value)
    {
        Health.Heal(value, true);
    }


    public override int GetMaxHealthBar()
    {
        return 1;
    }

    public override void Attack()
    {
        base.Attack();
        switch (charaType)
        {
            case PartyMemberEnum.Berserker:
                Sequence sequence = DOTween.Sequence().SetLoops(2, LoopType.Yoyo);
                sequence.Append(_gfx.transform.DOScale(Vector3.one * .75f, 0.2f).SetEase(Ease.Flash));
                sequence.Join(_gfx.transform.DOMoveX(transform.position.x + 1.25f, 0.2f).SetEase(Ease.Flash));
                break;
            case PartyMemberEnum.Paladin:
                _gfx.transform.DOScale(Vector3.one * .75f, 0.2f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
                break;
            case PartyMemberEnum.Archer:
                _gfx.transform.DOScale(Vector3.one * 1.75f, 0.2f).SetEase(Ease.InBounce).SetLoops(2, LoopType.Yoyo);
                break;
        }
    }

    [Button]
    private void TestAllMessages() => StartCoroutine(TestAllMessageRoutine());
    private IEnumerator TestAllMessageRoutine()
    {
                Debug.Log("qssdsqd");
        foreach(Message m in characterObj.messages)
        {
            foreach(MessageBody mb in m.messages)
            {
                message.StartCoroutine(message.MessageRoutine(mb));
                yield return new WaitForSeconds(2f);
            }
        }
    }
}
