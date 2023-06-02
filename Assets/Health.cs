using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;

    private ICharacter _character;
    private int _currentHealthBarAmount;

    [Header("layout group")]
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private HorizontalLayoutGroup _layerGroup;
    [SerializeField] private List<HealtPoint> _healthPoints = new List<HealtPoint>();


    private void Start()
    {
        _healthPoints.Clear();
        _character = GetComponent<Character>();

        _currentHealthBarAmount = _character.GetMaxHealthBar();

        for (int i = 0; i < _character.GetMaxHealth(); i++)
        {
            GameObject obj = Instantiate(_healthPoint, _layerGroup.transform);
            HealtPoint hp = obj.GetComponent<HealtPoint>();
            _healthPoints.Add(hp);
        }

        if( _healthPoints.Count > 1)
        {
            _layerGroup.padding.left += -(((int)_healthPoints[0].GetComponent<RectTransform>().rect.width * _healthPoints.Count) + ((int)_layerGroup.spacing * _healthPoints.Count - 1)) / 2;
        }
    }

    public void TakeDamage(int value)
    {
        int newHealth = _character.GetCurrentHealth() - value;
        if(newHealth <= 0)
        {
            _currentHealthBarAmount -= 1;
            if(_currentHealthBarAmount <= 0)
            {
                if (_refs.fightManager.EnableDebug)
                    Debug.Log($"{gameObject.name} have been killed");

                foreach(HealtPoint hp in _healthPoints)
                {
                    hp.ValidHp.sprite = hp.Colors[hp.Colors.Length - 1];
                }
                _character.SetCurrentHealth(0);
                _character.Kill();
                return;
            }
            else
            {
                _character.SetCurrentHealth(_character.GetMaxHealth() + newHealth);
                _refs.fightManager.TriggerEvent(AttackEvent.SpecialAttacksTrigerMode.LooseHealthBar);
                foreach (HealtPoint hp in _healthPoints)
                {
                    if(_currentHealthBarAmount <= 1)
                    {
                        hp.ValidHp.sprite = hp.Colors[hp.Colors.Length - 1];
                    }
                    else
                    {
                        hp.ValidHp.sprite = hp.Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount + 1];
                    }
                }
                for(int i = 0; i < _character.GetCurrentHealth(); i++)
                {

                    _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];

                }
                return;
            }
        }

        for(int i = newHealth; i < newHealth + value; i++)
        {

            if (_currentHealthBarAmount <= 1)
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_healthPoints[i].Colors.Length - 1];
            }
            else
            {
                _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount + 1];
            }

        }
        _character.SetCurrentHealth(newHealth);
    }
    [Button]
    public void TestHeal() => Heal(2);
    internal void Heal(int value)
    {
        Debug.Log("Tryheal");
        if (_character.IsDead())
        {
            Debug.LogError($"{gameObject.name} is dead he cannot be healed");
            return;
        }

        int newHealth = _character.GetCurrentHealth() + value;
        if (newHealth > _character.GetMaxHealth())
        {
            newHealth = _character.GetMaxHealth();
            if(_currentHealthBarAmount < _character.GetMaxHealthBar())
            {
                _currentHealthBarAmount += 1;
                for(int i = 0; i < value ; i++)
                {
                    _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
                }

            }
            else
            {
                foreach(HealtPoint hp in _healthPoints)
                {
                    hp.ValidHp.sprite = hp.Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
                }
            }
            _character.SetCurrentHealth(_character.GetMaxHealth());
            return;
        }

        for (int i = _character.GetCurrentHealth(); i < newHealth; i++)
        {
            _healthPoints[i].ValidHp.sprite = _healthPoints[i].Colors[_character.GetMaxHealthBar() - _currentHealthBarAmount];
        }
        if (_refs.fightManager.EnableDebug)
            Debug.Log($"{gameObject.name} have been healed");
        _character.SetCurrentHealth(newHealth);
    }

    //private void OnValidate()
    //{
    //    _character = GetComponent<ICharacter>();
    //    _sliderRectTransform = _sliderImage.rectTransform;


    //    if( _character == null )
    //        return;

    //    int health = _character.GetMaxHealth();

    //    _sliderImage.pixelsPerUnitMultiplier = _heartSize + (_hearthSizeWithSpace * Mathf.Max(health - 1, 0));

    //    if (health == 2)
    //        _sliderRectTransform.offsetMin = new Vector2(0, _bottomBaseValue);
    //    else if (health == 3)
    //        _sliderRectTransform.offsetMin = new Vector2(0, _bottomBaseValue + _bottomScaleValue);
    //    else if (health > 3)
    //        _sliderRectTransform.offsetMin = new Vector2(0, _bottomBaseValue + _bottomScaleValue + GetScaledBotValue(health));

    //}

    //private float GetScaledBotValue(int HearthsCount)
    //{
    //    float value = _bottomScaleValue;
    //    float result = 0;
    //    for(int i = 0; i < HearthsCount - 3; i++)
    //    {
    //        result += value /= 2;
    //    }

    //    return result;
    //}
}
