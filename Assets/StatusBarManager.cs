using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    [SerializeField] private Image[] _statusImages;
    [SerializeField] private Image[] _statusTimerImages;
    [SerializeField] private Sprite[] _timerImages;
    [SerializeField] private AllReferences _refs;
    [SerializeField] private Character _chara;

    private Vector3 _initSize;
    private Dictionary<Status.StatusEnum, int> _statusDictionary = new Dictionary<Status.StatusEnum, int>();

    private void Start()
    {
        _initSize = _statusImages[0].rectTransform.localScale;
    }
    public void UpdateBar()
    {
        int index = 0;
        foreach(Status status in _chara.Status)
        {
            if(index > _chara.Status.Count)
                break;

            _statusImages[index].gameObject.SetActive(true);
            _statusImages[index].rectTransform.localScale = _initSize;
            if (_statusImages[index].sprite != GetAttackSprite(_refs.fightManager, status.status))
            {
                _statusImages[index].sprite = GetAttackSprite(_refs.fightManager, status.status);
                _statusImages[index].rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.InBounce).SetLoops(2, LoopType.Yoyo);
            }
            if (status.isInfinite)
            {
                _statusTimerImages[index].sprite = GetTimerSprite(status.remainTurn, true);
            }
            else
            {
                _statusTimerImages[index].sprite = GetTimerSprite(status.remainTurn);
                TimerTween(index);
            }
            index++;
        }



        //foreach(Image img in _statusImages)
        //{
        //    if (img.IsActive())
        //    {
        //        img.rectTransform.DOScale(0f, 0.25f).SetEase(Ease.InBounce).OnComplete(() => img.gameObject.SetActive(false));
        //    }
        //}

        for(int i = index; i < _statusImages.Length - 1; i++)
        {
            if(_statusImages[i].gameObject.activeSelf == true)
            {
                RemoveTween(_statusImages[i].rectTransform);
            }
        }
    }

    private void TimerTween(int index)
    {
        Sequence timerSequence = DOTween.Sequence();
        timerSequence.Append(_statusTimerImages[index].rectTransform.DOShakeRotation(0.5f).SetEase(Ease.OutBounce));
        timerSequence.Join(_statusTimerImages[index].rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).SetLoops(2, LoopType.Yoyo));
    }

    private void RemoveTween(RectTransform rect)
    {
        rect.DOScale(0.1f, 0.25f).SetEase(Ease.InBounce).OnComplete(() => rect.gameObject.SetActive(false));
    }

    public Sprite GetTimerSprite(int num, bool isInfitite = false)
    {
        if (isInfitite)
        {
            return _timerImages[0];
        }
        switch (num)
        {
            case 1:
                return _timerImages[1];
            case 2:
                return _timerImages[2];
            case 3:
                return _timerImages[3];
            case 4:
                return _timerImages[4];

        }
        return _timerImages[0];
    }
    public Sprite GetAttackSprite(FightManager fm, Status.StatusEnum status)
    {
        switch (status)
        {
            case Status.StatusEnum.Strengthened:
                return fm.Strengthened;
            case Status.StatusEnum.Initiative:
                return fm.Initiative;
            case Status.StatusEnum.Regenerating:
                return fm.Regenerating;
            case Status.StatusEnum.Shielded:
                return fm.Shielded;
            case Status.StatusEnum.Fatigue:
                return fm.Fatigue;
            case Status.StatusEnum.Poisoned:
                return fm.Poisoned;
            case Status.StatusEnum.Fired:
                return fm.Fire;
            case Status.StatusEnum.Sleeped:
                return fm.Sleeped;
            case Status.StatusEnum.Restrained:
                return fm.Restrained;
            case Status.StatusEnum.Stunned:
                return fm.Stunned;
            case Status.StatusEnum.Disapeared:
                return fm.Disapear;
            case Status.StatusEnum.ShieldedWithReflect:
                return fm.ReflectShield;
            case Status.StatusEnum.Taunting:
                return fm.Taunt;
        }
        return null;
    }
}
