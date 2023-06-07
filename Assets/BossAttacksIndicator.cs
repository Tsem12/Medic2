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
    public Image[] Slots { get => _slots; set => _slots = value; }

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _group = GetComponent<HorizontalLayoutGroup>();
        initPos = _transform.localPosition;
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
        }
    }

    public void SetSprite(Sprite sprite, int index)
    {
        _group.padding.left = Mathf.Abs(index - 2) * 228;    
        _slots[index].gameObject.SetActive(true);
        _slots[index].sprite = sprite;
        ResetPos();
    }
}
