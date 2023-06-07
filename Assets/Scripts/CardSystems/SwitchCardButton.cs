using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchCardButton : MonoBehaviour
{
    [SerializeField] CardHandlerObject cardHandler;
    [SerializeField] CardDeckBuilder deck;
    [SerializeField] AllReferences refs;
    [SerializeField] Collider2D[] colToHide;
    [SerializeField] List<GameObject> cardsToHide;
    [SerializeField] List<CardBase> everyBase;

    private void Awake()
    {
        cardHandler.isChaningCards = false;
    }
    private void Start()
    {
        foreach (var item in everyBase)
        {
            if(deck.deck.Contains(item))
            {
                everyBase.Remove(item);
            }
        }
        for (int i = 0; i < everyBase.Count; i++)
        {
            cardsToHide[i].GetComponent<Card>().carBase = everyBase[i];
        }

        cardHandler.SwitchUpdate();
        refs.fightManager.OnTurnEnd += Hide;
    }

    public void Show()
    {
        foreach (var item in colToHide)
        {
            item.enabled = false;
        }
        foreach (var item in cardsToHide)
        {
            item.SetActive(true);
        }
        
        cardHandler.isChaningCards = true;
        cardHandler.SwitchUpdate();
    }

    public void Hide()
    {
        foreach (var item in colToHide)
        {
            item.enabled = true;
        }
        foreach (var item in cardsToHide)
        {
            item.SetActive(false);
        }
        cardHandler.isChaningCards = false;
        cardHandler.SwitchUpdate();
    }
}
