using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreyButton : MonoBehaviour
{
    [SerializeField] Image button;
    [SerializeField] Image backButton;
    [SerializeField] CardDeckBuilder deck;

    public void UpdateColor()
    {
        if(deck.deck.Count < 4)
        {
            button.color = Color.grey;
            backButton.color = Color.grey;
        }
        else
        {
            button.color = Color.white;
            backButton.color = Color.white;
        }
    }
}
