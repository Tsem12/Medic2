using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour, IInteractable , IToolTip
{
    public CardBase cardBase;
    [SerializeField] AllReferences refs;
    [SerializeField] InputHandlerObject inputObject;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer myRender;
    [SerializeField] SpriteRenderer usedRenderer;
    [SerializeField] float size = 0.5f;
    [SerializeField] TextMeshPro tmpro;
    [SerializeField] ManaObject manaObject;
    [SerializeField] Sprite lockedSprite;
    public bool isPlayingCard;
    bool effectWasApplied = false;
    bool isHidden = false;
    public bool wasSwitched = false;


    public void Init()
    {
        if(isPlayingCard)
        {
            manaObject.manaAddTurn += CheckIfInteractable;
            refs.fightManager.OnTurnEnd += DisableTurn;
            manaObject.manaUpdate += CheckIfInteractable;
            manaObject.manaUpdate += ManaUpdate;
        }
        else
        {
            HideCard();
        }
        refs.fightManager.OnTurnBegin += EnableTurn;
        handlerObject.switchCard += SwitchUpdate;
        CheckIfInteractable();
        UpdateCard();
    }

    private void OnDestroy()
    {
        if(isPlayingCard)
        {
            manaObject.manaAddTurn -= CheckIfInteractable;
            refs.fightManager.OnTurnEnd -= DisableTurn;
            manaObject.manaUpdate -= CheckIfInteractable;
            manaObject.manaUpdate -= ManaUpdate;
        }
        refs.fightManager.OnTurnBegin -= EnableTurn;
        handlerObject.switchCard -= SwitchUpdate;
    }

    public void NotInit()
    {
        gameObject.SetActive(false);
    }

    public bool ApplyEffect()
    {
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, size);
        col.enabled = true;
        if (collision != null && collision.gameObject.CompareTag("PartyMember"))
        {
            if (!collision.gameObject.GetComponent<PartyMember>().IsDead() && cardBase.cardBehaviour == CardBehaviour.resurection)
            {
                return false;
            }
            if (cardBase.ApplyEffectOfTheCard(collision.gameObject.GetComponent<Character>()) == false)
            {
                refs.audioManager.Play("FailedCast");
                return false;
            }
            effectWasApplied = true;
            return true;
        }
        return false;
    }

    void ResetPos()
    {
        transform.localPosition = Vector3.zero;
    }

    public void Interact()
    {
        if(!handlerObject.isChaningCards)
        {
            if(ApplyEffect())
            {
                DisableCard();
            }
            ResetPos();
        }
        else
        {
            SwitchCard();
            ResetPos();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, size);
    }

    void CheckIfInteractable()
    {
        if(cardBase.manaCost <= cardBase.manaObject.currentMana && !effectWasApplied)
        {
            transform.tag = "Grabbable";
        }
        else
        {
            transform.tag = "ToolTip";
        }
    }

    void EndInteractable()
    {
        transform.tag = "ToolTip";
    }

    public void Cancel()
    {
        ResetPos();
    }

    void EnableTurn()
    {
        effectWasApplied = false;
        wasSwitched = false;
        CheckIfInteractable();
        if (isPlayingCard)
        {
            if (cardBase.manaCost <= cardBase.manaObject.currentMana)
            {
                ShowCard();
            }
            else
            {
                DisableCard();
            }
        }
        else
        {
            if(isHidden)
            {
                isHidden = false;
            }
        }
    }

    void SwitchCard()
    {
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, size);
        col.enabled = true;
        Card other = null;
        if (collision != null)
        {
            other = collision.gameObject.GetComponent<Card>();
        }
        if (other != null)
        {
            ExChangeCard(other);
            other.DisableCard();
            DisableCard();
            wasSwitched = true;
            other.wasSwitched = true;
        }
    }

    void SwitchUpdate()
    {
        if(handlerObject.isChaningCards)
        {
            transform.tag = "Grabbable";
            if (!isPlayingCard)
            {
                if(isHidden)
                {
                    DisableCard();
                }
                else
                {
                    ShowCard();
                }
            }
            if (!wasSwitched)
            {
                col.enabled = true;
            }
        }
        else
        {
            if(!isPlayingCard)
            {
                HideCard();
            }
            CheckIfInteractable();
        }
    }

    public void ExChangeCard(Card card)
    {
        CardBase myCard = cardBase;
        cardBase = card.cardBase;
        card.cardBase = myCard;

        UpdateCard();
        card.UpdateCard();
    }

    public void UpdateCard()
    {
        if(cardBase != null)
        {
            myRender.sprite = cardBase.cardSprite;
            myRender.color = Color.white;
            usedRenderer.sprite = cardBase.cardSpriteGrey;
            usedRenderer.color = Color.white;
            tmpro.SetText(cardBase.manaCost.ToString());
        }
        else
        {
            EndInteractable();
            myRender.sprite = lockedSprite;
            myRender.color = Color.white;
            usedRenderer.sprite = cardBase.cardSpriteGrey;
            usedRenderer.color = Color.white;
            tmpro.SetText("");
        }
    }

    void ShowCard()
    {
        col.enabled = true;
        myRender.enabled = true;
        usedRenderer.enabled = false;
        tmpro.enabled = true;
    }

    void HideCard()
    {
        col.enabled = false;
        myRender.enabled = false;
        usedRenderer.enabled = false;
        tmpro.enabled = false;
    }

    public void DisableCard()
    {
        col.enabled = false;
        myRender.enabled = false;
        usedRenderer.enabled = true;
        tmpro.enabled = true;
        isHidden = true;
    }

    public void ShowToolTip(ToolTip tooltip)
    {
        tooltip.ToolTipInfo(cardBase.cardName, cardBase.description, cardBase.cardSprite);
    }

    void DisableTurn()
    {
        HideCard();
        myRender.enabled = true;
        tmpro.enabled = true;
    }

    void ManaUpdate()
    {
        if(!effectWasApplied)
        {
            CheckIfInteractable();
        }
        if (isPlayingCard)
        {
            if (cardBase.manaCost <= cardBase.manaObject.currentMana && !effectWasApplied)
            {
                ShowCard();
            }
            else
            {
                DisableCard();
            }
        }
        else
        {
            if (isHidden)
            {
                isHidden = false;
            }
        }
    }
}
