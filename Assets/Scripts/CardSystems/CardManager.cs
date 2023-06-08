using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] CardDeckBuilder deckBuilder;
    [SerializeField] List<CardBase> cardsBases;
    [SerializeField] List<Card> cardsHiden;
    [SerializeField] List<Card> cardsGame;

    private void Start()
    {
        List<CardBase> tempCardsBases = cardsBases;
        foreach (CardBase item in deckBuilder.deck)
        {
            if(cardsBases.Contains(item))
            {
                tempCardsBases.Remove(item);
            }
        }

        foreach (CardBase item in tempCardsBases)
        {
            AddToHidden(item);
        }

        foreach (CardBase item in deckBuilder.deck)
        {
            AddToPlayingCard(item);
        }
    }

    void AddToHidden(CardBase cardBase)
    {
        foreach (Card item in cardsHiden)
        {
            if(item.carBase == null)
            {
                item.carBase = cardBase;
                item.Init();
                return;
            }
        }
    }

    void AddToPlayingCard(CardBase cardBase)
    {
        foreach (Card item in cardsGame)
        {
            if (item.carBase == null)
            {
                item.carBase = cardBase;
                item.Init();
                return;
            }
        }
    }
}
