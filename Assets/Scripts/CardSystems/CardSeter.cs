using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeter : MonoBehaviour
{
    [SerializeField] CardDeckBuilder deck;
    [SerializeField] Card[] cards;
    void Start()
    {
        cards[0].carBase = deck.deck[0];
        cards[1].carBase = deck.deck[1];
        cards[2].carBase = deck.deck[2];
        cards[3].carBase = deck.deck[3];
    }
}
