using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _damageTexts;
    [SerializeField] private TextMeshProUGUI[] _healTexts;
    private Vector3 _initPosDamageText;
    private Vector3 _initPoshealText;

    private void Start()
    {
        _initPosDamageText = _damageTexts[0].rectTransform.localPosition;
        _initPoshealText = _healTexts[0].rectTransform.localPosition;
    }
    [Button]
    public void TestDamagetween() => TakeDamageTween(2);
    public void TakeDamageTween(int value)
    {
        if (value <= 0)
            return;
        foreach(TextMeshProUGUI text in _damageTexts)
        {
            if(text.gameObject.activeSelf == false)
            {
                text.gameObject.SetActive(true);
                text.text = value.ToString();
                text.rectTransform.DOJumpAnchorPos(text.rectTransform.localPosition + new Vector3(Random.Range(-500, 501), 0, 0), Random.Range(2000, 3000), 1, 1.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    text.gameObject.SetActive(false);
                    text.rectTransform.localPosition = _initPosDamageText;
                });
                break;
            }
        }
    }
    [Button]
    public void TestHealtween() => HealTween(2);
    public void HealTween(int value)
    {
        foreach (TextMeshProUGUI text in _healTexts)
        {
            if (value <= 0)
                return;
            if (text.gameObject.activeSelf == false)
            {
                text.gameObject.SetActive(true);
                text.text = value.ToString();
                text.rectTransform.localPosition = new Vector3(Random.Range(-500, 501), 0, 0);
                text.rectTransform.DOAnchorPosY(text.rectTransform.localPosition.y + Random.Range(1000, 1500), 1.5f).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    text.gameObject.SetActive(false);
                    text.rectTransform.localPosition = _initPoshealText;
                });
                break;
            }
        }
    }

}
