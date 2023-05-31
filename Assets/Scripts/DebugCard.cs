using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugCard : MonoBehaviour
{
    [SerializeField] CardBase[] cards;
    [SerializeField] GameObject template;
    int index = 0;

    public void ShowDebug()
    {
        template.GetComponent<SetInputField>().Init(cards[index]);
    }

    public void Right()
    {
        index++;
        if(index >= cards.Length - 1)
        {
            index = cards.Length - 1;
        }
    }

    public void Left()
    {
        index--;
        if (index < 0)
        {
            index = 0;
        }
    }
}
