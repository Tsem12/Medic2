using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtPoint : MonoBehaviour
{
    [SerializeField] private GameObject _validHp;
    [SerializeField] private GameObject _invalidHp;

    public GameObject ValidHp { get => _validHp; set => _validHp = value; }
    public GameObject InvalidHp { get => _invalidHp; set => _invalidHp = value; }
}
