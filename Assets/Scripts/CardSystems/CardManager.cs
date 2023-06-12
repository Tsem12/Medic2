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

    private void Start()
    {
        List<CardBase> removeLocked = new List<CardBase>();
        foreach (var item in cardsBases)
        {
            if(item.isUnlocked)
            {
                removeLocked.Add(item);
            }
        }

        List<CardBase> removeDeck = new List<CardBase>();
        foreach (var item in removeLocked)
        {
            if(!deckBuilder.deck.Contains(item))
            {
                removeDeck.Add(item);
            }
        }

        for (int i = 0; i < cardsHiden.Count; i++)
        {
            if(i > removeDeck.Count - 1)
            {
                cardsHiden[i].NotInit();
            }
            else
            {
                cardsHiden[i].carBase = removeDeck[i];
                cardsHiden[i].Init();
            }
        }

        for (int i = 0; i < cardsGame.Count; i++)
        {
            cardsGame[i].carBase = deckBuilder.deck[i];
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
                current.Add(item.carBase);
            }
        }

        foreach (var item in cardsGame)
        {
           current.Add(item.carBase);
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
                item.carBase = temp.Pop();
                item.UpdateCard();
            }
        }

        foreach (var item in cardsGame)
        {
            item.carBase = temp.Pop();
            item.UpdateCard();
        }
    }
}
