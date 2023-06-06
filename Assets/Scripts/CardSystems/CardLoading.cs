using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLoading : MonoBehaviour
{
    [SerializeField] CardBase[] cards;
    [SerializeField] AllReferences refs;

    private void Start()
    {
        foreach (CardBase card in cards)
        {
            card.isUnlocked = refs.gameManager.gameData.spellUnlocked[(int)card.cardBehaviour];
        }
    }
}
