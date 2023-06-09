using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtPoint : MonoBehaviour
{
    [SerializeField] private Image _validHp;
    [SerializeField] private Image _InValidHp;

    [SerializeField] private Sprite[] _colors;

    public Image ValidHp { get => _validHp; set => _validHp = value; }
    public Sprite[] Colors { get => _colors; set => _colors = value; }
    public Image InValidHp { get => _InValidHp; set => _InValidHp = value; }
}
