using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulesHandeler : MonoBehaviour
{
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

    [SerializeField] private SpriteRenderer _shield;
    [SerializeField] private SpriteRenderer _shieldEffect;
    private Color _shieldColor;
    [SerializeField] private SpriteRenderer _reflectShield;
    [SerializeField] private SpriteRenderer _reflectShieldEffect;
    private Color _reflectShieldColor;



    private void Start()
    {
        _shieldColor = _shield.color;
        _reflectShieldColor = _reflectShield.color;
    }
    [Button]
    public void TestShield() => ActiveEffect(Status.StatusEnum.Shielded);
    public void ActiveEffect(Status.StatusEnum status)
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
            case Status.StatusEnum.Disapeared:
                //Add particule
                break;
            case Status.StatusEnum.Taunting:
                _provoc.Play();
                break;
        }
    }

    [Button]
    public void TestRemoveShield() => DeactiveEffect(Status.StatusEnum.Shielded);
    public void DeactiveEffect(Status.StatusEnum status)
    {
        switch (status)
        {
            case Status.StatusEnum.Shielded:
                
                _shield.color = _shieldColor;
                _shield.DOColor(Vector4.zero, 0.5f).SetEase(Ease.OutCirc).OnComplete(() => _shield.gameObject.SetActive(false));
                _shieldEffect.DOColor(Vector4.zero, 0.5f).SetEase(Ease.OutCirc);
                break;
            case Status.StatusEnum.ShieldedWithReflect:
                _reflectShield.gameObject.SetActive(true);
                _reflectShield.color = new Vector4(0, 0, 0, 0);
                _reflectShield.DOColor(_reflectShieldColor, 0.5f).SetEase(Ease.InElastic).OnComplete(() => _shield.gameObject.SetActive(false));
                _reflectShieldEffect.DOColor(_reflectShieldColor, 0.5f).SetEase(Ease.InElastic);
                break;
        }
    }
}
