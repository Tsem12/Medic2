using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] CardDeckBuilder deckBuilder;
    [SerializeField] List<CardBase> cardsBases = new List<CardBase>();
    [SerializeField] List<Card> cardsHiden = new List<Card>();
    List<Card> cardsGame = new List<Card>();
    [SerializeField] GameObject cardDeck;

    private void Start()
    {


        foreach (var item in cardDeck.GetComponentsInChildren<Card>())
        {
            cardsGame.Add(item);
        }

        foreach (var item in cardsBases)
        {
           // item.Load();
        }

        List<CardBase> removeLocked = new List<CardBase>();
        foreach (var item in cardsBases)
        {
            if(item.isUnlocked)
            {
                removeLocked.Add(item);
            }
        }

        Stack<CardBase> removeDeck = new Stack<CardBase>();
        foreach (var item in removeLocked)
        {
            if(!deckBuilder.deck.Contains(item))
            {
                removeDeck.Push(item);
            }
        }

        foreach (var item in cardsHiden)
        {
            if(removeDeck.Count > 0)
            {
                item.cardBase = removeDeck.Pop();
                item.Init();
            }
            else
            {
                item.NotInit();
            }
        }

        for (int i = 0; i < cardsGame.Count; i++)
        {
            cardsGame[i].cardBase = deckBuilder.deck[i];
            cardsGame[i].Init();
        }
    }

    [Button("TestShuffle")]
    public void ShuffleObject()
    {
        List<CardBase> current = new List<CardBase>();

        foreach (var item in cardsHiden)
        {
            if(item.gameObject.activeSelf)
            {
                current.Add(item.cardBase);
            }
        }

        foreach (var item in cardsGame)
        {
           current.Add(item.cardBase);
        }

        System.Random rand = new System.Random();
        var shuffled = current.OrderBy(_ => rand.Next()).ToList();

        Stack<CardBase> temp = new Stack<CardBase>();
        foreach (var item in shuffled)
        {
            temp.Push(item);
        }

        foreach (var item in cardsHiden)
        {
            if (item.gameObject.activeSelf)
            {
                item.cardBase = temp.Pop();
                item.UpdateCard();
            }
        }

        foreach (var item in cardsGame)
        {
            item.cardBase = temp.Pop();
            item.UpdateCard();
        }
    }

    public void SaveCards()
    {
        foreach (var item in cardsBases)
        {
            item.Save();
        }
    }
}
