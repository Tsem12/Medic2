using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] CardDeckBuilder deck;
    [SerializeField] CardBase card;
    bool on = false;

    private void Start()
    {
        deck.deck.Clear();
    }

    public void Switch()
    {
        if(deck.deck.Contains(card))
        {
            if(!on)
            {
                on = true;
                image.color = Color.white;
                transform.DOScale(transform.localScale * 1.15f, 0.2f);
            }
        }
        else
        {
            if(on)
            {
                on = false;
                image.color = Color.gray;
                transform.DOScale(transform.localScale / 1.15f, 0.2f);
            }
        }
    }
}
