using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreyButton : MonoBehaviour
{
    [SerializeField] Image button;
    [SerializeField] Image backButton;
    [SerializeField] UnityEngine.UI.Button backButtonButton;
    [SerializeField] CardDeckBuilder deck;

    public void UpdateColor()
    {
        if(deck.deck.Count < 4)
        {
            button.color = Color.grey;
            backButton.color = Color.grey;
            backButtonButton.interactable = false;
        }
        else
        {
            backButtonButton.interactable = true;
            button.color = Color.white;
            backButton.color = Color.white;
        }
    }
}
