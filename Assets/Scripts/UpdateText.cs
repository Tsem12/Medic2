using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    [SerializeField] TextMeshPro tmpPro;
    [SerializeField] CardDeckBuilder deck;

    public void UpdateTmp()
    {
        tmpPro.SetText(deck.deck.Count.ToString() + "/4");
    }
}
