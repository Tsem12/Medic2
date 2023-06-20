using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] CardDeckBuilder deck;
    [SerializeField] CardBase card;
    [SerializeField] TextMeshPro text;

    bool on = false;

    private void Start()
    {
        deck.deck.Clear();
        text.text = card.manaCost.ToString();
    }

    public void Switch()
    {
        if(deck.deck.Contains(card))
        {
            if(!on)
            {
                on = true;
                image.color = Color.white;
                text.color = Color.white;
                transform.DOScale(transform.localScale * 1.15f, 0.2f);
            }
        }
        else
        {
            if(on)
            {
                on = false;
                image.color = Color.gray;
                text.color = Color.gray;
                transform.DOScale(transform.localScale / 1.15f, 0.2f);
            }
        }
    }
}
