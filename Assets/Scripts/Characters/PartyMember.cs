using DG.Tweening;
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
    }

    private void Awake()
    {
        _arrowForBoss.DOLocalMoveY(0.05f, 0.25f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
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
        _targetsAttacks.Add(_nextPossibleAttacks[Random.Range(0, _nextPossibleAttacks.Count)]);

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

    public override void Attack()
    {
        base.Attack();
        _gfx.transform.DOScale(Vector3.one * .5f,0.5f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
    }
}
