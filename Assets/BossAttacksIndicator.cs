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
        _arrow.DOMoveY(0.5f, 0.25f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);

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
        _group.padding.left = Mathf.Abs(index - 2) * 228;    
        _slots[index].gameObject.SetActive(true);
        _slots[index].sprite = sprite;
        ResetPos();
    }
}
