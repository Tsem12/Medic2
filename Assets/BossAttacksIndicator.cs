using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAttacksIndicator : MonoBehaviour
{
    [SerializeField] private Image[] _slots;
    private Vector3 initPos;
    private RectTransform _transform;
    private HorizontalLayoutGroup _group;
    private Vector3 _initArrowPos;
    public Image[] Slots { get => _slots; set => _slots = value; }

    [SerializeField] private RectTransform _arrow;
    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _group = GetComponent<HorizontalLayoutGroup>();
        initPos = _transform.localPosition;
        _initArrowPos = _arrow.localPosition;
        Sequence sequence = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo);
        sequence.Append(_arrow.DOAnchorPosY(7000f, 0.5f).SetEase(Ease.OutFlash));
        sequence.Join(_arrow.DOScale(Vector3.one *1.2f, 0.1f).SetEase(Ease.OutBounce));

    }

    public void ResetPos()
    {
        _transform.localPosition = initPos;
    }

    public void Clear()
    {
        foreach (Image go in _slots)
        {
            go.gameObject.SetActive(false);
            _arrow.gameObject.SetActive(false);
        }
    }

    public void SetSprite(Sprite sprite, int index)
    {
        _arrow.gameObject.SetActive(true);  
        _slots[index].gameObject.SetActive(true);
        _slots[index].sprite = sprite;
        ResetPos();
    }
}
