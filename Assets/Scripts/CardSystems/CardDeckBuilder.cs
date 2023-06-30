using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    public void SaveDeck(string sceneName)
    {
        if(deck.Count == 4)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }        
    }
}
