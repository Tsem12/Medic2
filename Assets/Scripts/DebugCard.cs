using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugCard : MonoBehaviour
{
    [SerializeField] CardBase[] cards;
    [SerializeField] GameObject inputFieldPrefab;
    [SerializeField] GameObject template;
    int index = 0;

    public void ShowDebug()
    {
        template.GetComponent<SetInputField>().Init(cards[0]);
        template.transform.SetParent(transform);
        template.transform.localPosition = Vector3.zero;
    }

    public void Right()
    {

    }

    public void Left()
    {

    }
}
