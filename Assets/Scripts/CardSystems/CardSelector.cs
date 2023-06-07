using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] CardDeckBuilder deck;
    [SerializeField] CardBase card;

    private void Start()
    {
        deck.deck.Clear();
    }

    public void Switch()
    {
        if(deck.deck.Contains(card))
        {
            image.color = Color.gray;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
