using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "ScriptableObjects/Deck")]
public class CardDeckBuilder : ScriptableObject
{
    public List<CardBase> deck = new List<CardBase>();

    public void AddOrRemoveCard(CardBase card)
    {
        if (deck.Contains(card))
        {
            deck.Remove(card);
        }
        else
        {
            if (deck.Count < 4)
            {
                deck.Add(card);
            }
        }
    }


    public void SaveDeck()
    {
        if(deck.Count < 4)
        {
            Debug.Log("Can't save without enough card");
        }        
    }
}
