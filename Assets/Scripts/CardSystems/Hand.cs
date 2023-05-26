using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject[] bagCards;
    [SerializeField] CardPositions pos;
    [SerializeField] CardHandlerObject cardObject;


    void HandUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            bagCards[i].transform.position = pos.cardPos[i].position;
        }
    }
}
