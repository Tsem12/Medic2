using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulesHandeler : MonoBehaviour
{
    public enum CardEffect
    {
        Ressurect,
        Panacea,
        Heal,
        Disapear,
        Die

    }

    [SerializeField] private ParticleSystem _buff;
    [SerializeField] private ParticleSystem _sleep;
    [SerializeField] private ParticleSystem _fatigue;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _grab;
    [SerializeField] private ParticleSystem _poison;
    [SerializeField] private ParticleSystem _provoc;
    [SerializeField] private ParticleSystem _stun;

    [SerializeField] private ParticleSystem[] _panacea;
    [SerializeField] private ParticleSystem[] _res;
    [SerializeField] private ParticleSystem _heal;
    [SerializeField] private ParticleSystem _disapear;
    [SerializeField] private ParticleSystem _die;

    [SerializeField] private SpriteRenderer _shield;
    [SerializeField] private SpriteRenderer _shieldEffect;
    private Color _shieldColor;
    [SerializeField] private SpriteRenderer _reflectShield;
    [SerializeField] private SpriteRenderer _reflectShieldEffect;
    private Color _reflectShieldColor;

    [SerializeField] private AnimationCurve _breakShield;

    public ParticleSystem Disapear { get => _disapear; set => _disapear = value; }

    private void Start()
    {
        _shieldColor = _shield.color;
        _reflectShieldColor = _reflectShield.color;
    }

    public void StopAllParticles()
    {
        _buff.Stop();
        _sleep.Stop();
        _fatigue.Stop();
        _fire.Stop();
        _grab.Stop();
        _poison.Stop();
        _provoc.Stop();
        _stun.Stop();
        _heal.Stop();
        if(_disapear != null)
        {
            _disapear.Stop();
        }
        foreach(ParticleSystem p in _panacea)
        {
            p.Stop();
        }
        foreach (ParticleSystem p in _res)
        {
            p.Stop();
        }
        _shield.gameObject.SetActive(false);
        _shieldEffect.gameObject.SetActive(false);
        _reflectShield.gameObject.SetActive(false);
        _shieldEffect.gameObject.SetActive(false);

    }

    public void ActiveShield(Status.StatusEnum status)
    {
        switch (status)
        {
            case Status.StatusEnum.Shielded:
                _shield.gameObject.SetActive(true);
                _shield.color = new Vector4(0, 0, 0, 0);
                _shield.DOColor(_shieldColor, 0.5f).SetEase(Ease.OutCirc);
                _shieldEffect.DOColor(Color.white, 0.5f).SetEase(Ease.OutCirc);
                break;
            case Status.StatusEnum.ShieldedWithReflect:
                _reflectShield.gameObject.SetActive(true);
                _reflectShield.color = new Vector4(0, 0, 0, 0);
                _reflectShield.DOColor(_reflectShieldColor, 0.5f).SetEase(Ease.OutCirc);
                _reflectShieldEffect.DOColor(Color.white, 0.5f).SetEase(Ease.OutCirc);
                break;
        }
    }

    public void ActiveEffect(Status.StatusEnum status)
    {
        Debug.Log("dsdffsdfs");
        switch (status)
        {
            case Status.StatusEnum.Strengthened:
                _buff.Play();
                break;
            case Status.StatusEnum.Regenerating:
                _heal.Play();
                break;
            case Status.StatusEnum.Fatigue:
                _fatigue.Play();
                break;
            case Status.StatusEnum.Poisoned:
                _poison.Play();
                break;
            case Status.StatusEnum.Fired:
                _fire.Play();
                break;
            case Status.StatusEnum.Sleeped:
                _sleep.Play();
                break;
            case Status.StatusEnum.Restrained:
                _grab.Play();
                break;
            case Status.StatusEnum.Stunned:
                _stun.Play();
                break;
            case Status.StatusEnum.Taunting:
                _provoc.Play();
                break;
        }
    }

    public void ActiveEffect(CardEffect status)
    {
        switch (status)
        {
            case CardEffect.Ressurect:
                foreach (ParticleSystem particule in _res)
                {
                    particule.Play();
                }
                break;
            case CardEffect.Panacea:
                foreach (ParticleSystem particule in _panacea)
                {
                    particule.Play();
                }
                break;
            case CardEffect.Heal:
                _heal.Play();
                break;
            case CardEffect.Die:
                _die.Play();
                break;
            case CardEffect.Disapear:
                Disapear.Play();
                break;
        }
    }

    public void DeactiveShield(Status.StatusEnum status)
    {
        switch (status)
        {
            case Status.StatusEnum.Shielded:
                
                //_shield.color = _shieldColor;
                _shield.DOColor(Vector4.zero, 0.5f).SetEase(_breakShield).OnComplete(() => _shield.gameObject.SetActive(false));
                _shieldEffect.DOColor(Vector4.zero, 0.5f).SetEase(_breakShield).OnComplete(() => _shieldEffect.gameObject.SetActive(false));
                break;
            case Status.StatusEnum.ShieldedWithReflect:
                //_reflectShield.color = _reflectShieldColor;
                _reflectShield.DOColor(Vector4.zero, 0.5f).SetEase(_breakShield).OnComplete(() => _shield.gameObject.SetActive(false));
                _reflectShieldEffect.DOColor(Vector4.zero, 0.5f).SetEase(_breakShield).OnComplete(() => _shieldEffect.gameObject.SetActive(false));
                break;
        }
    }
}
