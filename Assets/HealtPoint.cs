using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtPoint : MonoBehaviour
{
    [SerializeField] private Image _validHp;

    [SerializeField] private Sprite[] _colors;

    public Image ValidHp { get => _validHp; set => _validHp = value; }
    public Sprite[] Colors { get => _colors; set => _colors = value; }
}
