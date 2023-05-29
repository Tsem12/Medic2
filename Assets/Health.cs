using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;

    private ICharacter _character;

    [Header("Slider refs")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderImage;

    [SerializeField] private float _hearthSizeWithSpace;
    [SerializeField] private float _heartSize;

    [SerializeField] private float _bottomBaseValue;
    [SerializeField] private float _bottomScaleValue;

    private RectTransform _sliderRectTransform;


    [Header("layout group")]
    [SerializeField] private GameObject _healthPoint;
    [SerializeField] private HorizontalLayoutGroup _layerGroup;
    [SerializeField] private List<HealtPoint> _healthPoints = new List<HealtPoint>();


    private void Start()
    {
        _healthPoints.Clear();
        _character = GetComponent<Character>();
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
            if (_refs.fightManager.EnableDebug)
                Debug.Log($"{gameObject.name} have been killed");

            foreach(HealtPoint hp in _healthPoints)
            {
                hp.ValidHp.SetActive(false);
                hp.InvalidHp.SetActive(true);
            }
            _character.SetCurrentHealth(0);
            _character.Kill();
            return;
        }

        for(int i = newHealth; i < newHealth + value; i++)
        {
            _healthPoints[i].ValidHp.SetActive(false);
            _healthPoints[i].InvalidHp.SetActive(true);
        }
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
