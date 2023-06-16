using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class Health : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;
    [SerializeField] private ValueIndicator _valueIndicator;

    private Character _character;
    private Tweener _tweener;
    private int _currentHealthBarAmount;

    [SerializeField] private Transform _gfx;
    [Header("layout group")]
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private GameObject _healthBarCount;
    [SerializeField] private HorizontalLayoutGroup _layerGroupHealthpoint;
    [SerializeField] private HorizontalLayoutGroup _layerGroupHealthBar;
    [SerializeField] private List<HealtPoint> _healthPoints = new List<HealtPoint>();
    [SerializeField] private List<GameObject> _healthBars = new List<GameObject>();

    public int CurrentHealthBarAmount { get => _currentHealthBarAmount; }

    private void Awake()
    {
        _healthPoints.Clear();
        _character = GetComponent<Character>();
        _currentHealthBarAmount = _character.GetMaxHealthBar();
        
    }
    private IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _character.GetMaxHealth(); i++)
        {
            GameObject obj = Instantiate(_healthPoint, _layerGroupHealthpoint.transform);
            HealtPoint hp = obj.GetComponent<HealtPoint>();
            _healthPoints.Add(hp);
        }

        if (_layerGroupHealthBar != null)
        {
            for (int i = 0; i < _character.GetMaxHealthBar(); i++)
            {
                GameObject obj = Instantiate(_healthBarCount, _layerGroupHealthBar.transform);
                _healthBars.Add(obj);
            }

        }


    }


    public void TakeDamage(int value)
    {

        Status status = _character.GetStatus(Status.StatusEnum.Sleeped);
        if (status != null)
        {
            _character.TryRemoveStatus(Status.StatusEnum.Sleeped);
            _character.AddStatus(new Status(Status.StatusEnum.Stunned, 2));
            Status d = _character.GetStatus(Status.StatusEnum.Stunned);
        }
        TweenTakeDamage(0.1f);
        _valueIndicator.TakeDamageTween(value);
        int newHealth = _character.GetCurrentHealth() - value;
        if (newHealth <= 0)
        {
            _currentHealthBarAmount -= 1;
            if (_currentHealthBarAmount <= 0)
            {
                if (_refs.fightManager.EnableDebug)
                    Debug.Log($"{gameObject.name} have been killed");

                foreach (HealtPoint hp in _healthPoints)
                {
                    hp.ValidHp.sprite = hp.Colors[hp.Colors.Length - 1];
                }
                _character.SetCurrentHealth(0);
                _character.Kill();
                return;
            }
            else
            {
                _healthBars[_currentHealthBarAmount].SetActive(false);
                _character.SetCurrentHealth(_character.GetMaxHealth() + newHealth);
                _refs.fightManager.TriggerEvent(AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar, _currentHealthBarAmount);
                foreach (HealtPoint hp in _healthPoints)
                {
                    if (_currentHealthBarAmount <= 1)
                    {
                        hp.ValidHp.sprite = hp.Colors[hp.Colors.Length - 1];
                    }
                    else
                    {
                        hp.ValidHp.sprite = hp.Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount + 1];
                    }
                }
                for (int i = 0; i < _character.GetCurrentHealth(); i++)
                {
                    _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
                }
                for(int i = _character.GetCurrentHealth(); i < _character.GetMaxHealth(); i++)
                {
                    TakeHealthBarDamageTweener(i);
                }
                return;
            }
        }

        for (int i = newHealth; i < newHealth + value; i++)
        {
            if (_currentHealthBarAmount <= 1)
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_healthPoints[i].Colors.Length - 1];
                TakeLastBarHealthBarDamageTweener(i);
            }
            else
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount + 1];
                TakeHealthBarDamageTweener(i);
            }

        }
        _character.SetCurrentHealth(newHealth);
    }

    private void TweenTakeDamage(float value)
    {
        if (_tweener == null)
        {
            _tweener = _gfx.DOShakePosition(0.5f, value).SetEase(Ease.InFlash).OnComplete(() => _tweener = null);
        }
    }

    private void TakeHealthBarDamageTweener(int i)
    {
        Image img = _healthPoints[i].ValidHp.GetComponent<Image>();
        Color baseColor = img.color;
        Tweener scale = _healthPoints[i].ValidHp.rectTransform.DOScale(Vector3.one * 1.5f, 0.25f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(img.DOColor(Color.red, 0.25f));
        sequence.Join(_healthPoints[i].ValidHp.rectTransform.DOShakeAnchorPos(0.5f, 25f, 10, 90).SetEase(Ease.OutFlash));
        sequence.Join(scale.OnComplete(() => _healthPoints[i].ValidHp.rectTransform.DOScale(Vector3.one, 0.25f)));
        sequence.Append(img.DOColor(baseColor, 0.25f));
    }

    private void TakeLastBarHealthBarDamageTweener(int i)
    {
        Tweener scale = _healthPoints[i].InValidHp.rectTransform.DOScale(Vector3.one * 1.5f, 0.25f);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_healthPoints[i].InValidHp.rectTransform.DOShakeAnchorPos(0.5f, 25f, 10, 90).SetEase(Ease.OutFlash));
        sequence.Join(scale.OnComplete(() => _healthPoints[i].InValidHp.rectTransform.DOScale(Vector3.one, 0.25f)));
    }

    [Button]
    public void TestHeal() => Heal(2);
    internal void Heal(int value, bool IsPartyMember = false)
    {
        _refs.audioManager.Play("StatusRegen");
        if (_character.IsDead())
        {
            Debug.LogError($"{gameObject.name} is dead he cannot be healed");
            return;
        }
        _valueIndicator.HealTween(value);
        int newHealth = _character.GetCurrentHealth() + value;
        if (newHealth > _character.GetMaxHealth())
        {
            newHealth = _character.GetMaxHealth();
            if (_currentHealthBarAmount < _character.GetMaxHealthBar())
            {
                _currentHealthBarAmount += 1;
                for (int i = 0; i < value; i++)
                {
                    _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
                }

            }
            else
            {
                StartCoroutine(HealTweenRoutine());
                //foreach (HealtPoint hp in _healthPoints)
                //{
                //    sequence1.Append(hp.GetComponent<RectTransform>().DOMoveY(0.4f, 0.175f).SetEase(Ease.OutFlash).SetLoops(2, LoopType.Yoyo));
                //    hp.ValidHp.sprite = hp.Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
                //}
            }
            _character.SetCurrentHealth(_character.GetMaxHealth());
            return;
        }
        List<HealtPoint> list = new List<HealtPoint>();
        for (int i = _character.GetCurrentHealth(); i < newHealth; i++)
        {
            list.Add(_healthPoints[i]);
            if (IsPartyMember)
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[0];
            }
            else
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
            }
        }
        StartCoroutine(HealTweenRoutine(list));
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} have been healed");

        _character.SetCurrentHealth(newHealth);

    }


    IEnumerator HealTweenRoutine(List<HealtPoint> list)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (HealtPoint hp in list)
        {
            sequence.Append(hp.ValidHp.rectTransform.DOAnchorPosY(hp.ValidHp.rectTransform.localPosition.y + 300, 0.175f).SetEase(Ease.OutFlash).SetLoops(2, LoopType.Yoyo));
        }
        yield return new WaitUntil(() => !sequence.IsPlaying());
    }

    IEnumerator HealTweenRoutine()
    {
        Sequence sequence1 = DOTween.Sequence();
        for (int i = _character.GetCurrentHealth(); i < _character.GetMaxHealth(); i++)
        {
            _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
            sequence1.Append(_healthPoints[i].ValidHp.rectTransform.DOAnchorPosY(_healthPoints[i].ValidHp.rectTransform.localPosition.y + 300, 0.175f).SetEase(Ease.OutFlash).SetLoops(2, LoopType.Yoyo));
        }
        yield return new WaitUntil(() => !sequence1.IsPlaying());
    }
}
