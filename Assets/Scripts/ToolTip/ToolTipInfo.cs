using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipInfo : MonoBehaviour, IToolTip
{
    [SerializeField] string objectName;
    [SerializeField] string description;
    [SerializeField] Sprite image;
    [SerializeField] CardBase card;

    public void ShowToolTip(ToolTip tooltip)
    {
        if(card != null)
        {
            tooltip.ToolTipInfo(card.cardName, card.description, card.cardSprite);
        }
        else
        {
            tooltip.ToolTipInfo(objectName, description, image);
        }
    }

}
