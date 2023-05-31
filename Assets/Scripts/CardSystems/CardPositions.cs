using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPositions : MonoBehaviour
{
    public RectTransform[] cardPos;

    [Range(0.0f, 0.5f)]
    [SerializeField] float offset;

    [Range(-5.0f, 5.0f)]
    [SerializeField] float posY;

    private void Start()
    {
        cardPos[0].position = new Vector3(0.5f + offset, posY, cardPos[0].position.z);
        cardPos[1].position = new Vector3(1.5f + (offset * 3), posY, cardPos[1].position.z);
        cardPos[2].position = new Vector3(-0.5f - offset, posY, cardPos[2].position.z);
        cardPos[3].position = new Vector3(-1.5f - (offset * 3), posY, cardPos[3].position.z);
    }

    private void OnValidate()
    {
        cardPos[0].position = new Vector3(0.5f + offset, posY, cardPos[0].position.z);
        cardPos[1].position = new Vector3(1.5f + (offset * 3), posY, cardPos[1].position.z);
        cardPos[2].position = new Vector3(-0.5f - offset, posY, cardPos[2].position.z);
        cardPos[3].position = new Vector3(-1.5f - (offset * 3), posY, cardPos[3].position.z);
    }
}
