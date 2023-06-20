using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextMana : MonoBehaviour
{
    [SerializeField] CardBase card;
    [SerializeField] TMPro.TextMeshPro m_TextMeshPro;

    void Start()
    {
        m_TextMeshPro.text = card.manaCost.ToString();
    }
}
