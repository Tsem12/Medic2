using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeckBuilder : ScriptableObject
{
    List<CardBase> deck = new List<CardBase>();

    public void AddCard(CardBase card)
    {
        if(deck.Count < 4)
        {
            deck.Add(card);
        }
    }


    public void SaveDeck()
    {
        if(deck.Count < 4)
        {
            Debug.Log("Can't save without enough card");
        }        
    }

    public void RemoveFromDeck(CardBase card) 
    { 
        if(deck.Contains(card))
        {
            deck.Remove(card);
        }
    }
}
