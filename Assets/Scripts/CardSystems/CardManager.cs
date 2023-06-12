using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class CardManager : MonoBehaviour
{
    [SerializeField] CardDeckBuilder deckBuilder;
    [SerializeField] List<CardBase> cardsBases;
    [SerializeField] List<Card> cardsHiden;
    [SerializeField] List<Card> cardsGame;
    int indexGame = 0;
    int indexHidden = 0;

    private void Start()
    {
        List<CardBase> tempCardsBases = new List<CardBase>();
        foreach (var item in cardsBases)
        {
            tempCardsBases.Add(item);
        }

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
        if (cardsHiden[indexHidden] != null)
        {
            if (cardsHiden[indexHidden].carBase == null)
            {
                if (cardBase.isUnlocked)
                {
                    cardsHiden[indexHidden].carBase = cardBase;
                }
                cardsHiden[indexHidden].Init();
            }
            else
            {
                if (cardBase.isUnlocked)
                {
                    cardsGame[indexGame].carBase = cardBase;
                }
                cardsHiden[indexHidden].UpdateCard();
            }
        }
        indexHidden++;
    }

    void AddToPlayingCard(CardBase cardBase)
    {
        if (cardsGame[indexGame] != null)
        {
            if (cardsGame[indexGame].carBase == null)
            {
                if (cardBase.isUnlocked)
                {
                    cardsGame[indexGame].carBase = cardBase;
                }
                cardsGame[indexGame].Init();
            }
            else
            {
                if (cardBase.isUnlocked)
                {
                    cardsGame[indexGame].carBase = cardBase;
                }
                cardsGame[indexGame].UpdateCard();
            }
        }
        indexGame++;
    }

    [Button("TestShuffle")]
    public void ShuffleObject()
    {
        indexGame = 0;
        indexHidden = 0;

        System.Random rand = new System.Random();
        var shuffled = cardsBases.OrderBy(_ => rand.Next()).ToList();

        foreach (CardBase item in shuffled)
        {
            if(item.isUnlocked && indexGame < cardsGame.Count)
            {
                AddToPlayingCard(item);
            }
            else if(indexHidden < cardsHiden.Count)
            {
                AddToHidden(item);
            }
        }
    }
}
