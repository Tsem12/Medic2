using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchCardButton : MonoBehaviour
{
    [SerializeField] CardHandlerObject cardHandler;
    [SerializeField] AllReferences refs;
    [SerializeField] CapsuleCollider2D[] colToHide;
    [SerializeField] GameObject cardsToHide;
    bool wasHit = false;

    private void Awake()
    {
        cardHandler.isChaningCards = false;
    }

    private void Start()
    {
        refs.fightManager.OnTurnEnd += Hide;
        refs.fightManager.OnTurnBegin += ShowButton;
        refs.fightManager.OnTurnEnd += HideButton;
    }

    private void OnDestroy()
    {
        refs.fightManager.OnTurnEnd -= Hide;
        refs.fightManager.OnTurnBegin -= ShowButton;
        refs.fightManager.OnTurnEnd -= HideButton;
    }

    public void Show()
    {
        foreach (var item in colToHide)
        {
            item.enabled = false;
        }
        cardsToHide.SetActive(true);


        cardHandler.isChaningCards = true;
        cardHandler.SwitchUpdate();
    }

    public void Hide()
    {
        if(wasHit)
        {
            wasHit = false;
        }
        foreach (var item in colToHide)
        {
            item.enabled = true;
        }
        cardsToHide.SetActive(false);
        cardHandler.isChaningCards = false;
        cardHandler.SwitchUpdate();
    }

    public void OnClick()
    {
        wasHit = !wasHit;

        if(wasHit)
        {
            refs.audioManager.Play("ButtonPress1");
            Show();
        }
        else
        {
            refs.audioManager.Play("ButtonPress2");
            Hide();
        }
    }

    void HideButton()
    {
        transform.gameObject.SetActive(false);
    }

    void ShowButton()
    {
        transform.gameObject.SetActive(true);
    }
}
