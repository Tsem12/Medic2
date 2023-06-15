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
    bool effectWasApplied = false;

    public void Init()
    {
        if(cardBase != null)
        {
            CheckIfInteractable();
            UpdateCard();
            cardBase.manaObject.manaAddTurn += CheckIfInteractable;
            refs.fightManager.OnTurnEnd += EndInteractable;
            refs.fightManager.OnTurnBegin += EnableTurn;
            cardBase.manaObject.manaUpdate += CheckIfInteractable;
            handlerObject.switchCard += SwitchUpdate;
        }
        else
        {
            EndInteractable();
            myRender.sprite = lockedSprite;
            myRender.color = Color.white;
            usedRenderer.sprite = lockedSprite;
            usedRenderer.color = Color.white;
            tmpro.SetText("");
        }
    }

    private void OnDestroy()
    {
        cardBase.manaObject.manaAddTurn -= CheckIfInteractable;
        refs.fightManager.OnTurnEnd -= EndInteractable;
        refs.fightManager.OnTurnBegin -= EnableTurn;
        cardBase.manaObject.manaUpdate -= CheckIfInteractable;
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
            if(!cardBase.ApplyEffectOfTheCard(collision.gameObject.GetComponent<Character>()))
            {
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
                HideCard();
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
        if(cardBase.manaCost <= cardBase.manaObject.currentMana)
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
        col.enabled = true;
        myRender.enabled = true;
        usedRenderer.enabled = false;
        effectWasApplied = false;
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
            if(!other.effectWasApplied)
            {
                ExChangeCard(other);
                other.HideCard();
                HideCard();
            }
        }
    }

    void SwitchUpdate()
    {
        if(handlerObject.isChaningCards)
        {
            if(!effectWasApplied)
            {
                transform.tag = "Grabbable";
            }
        }
        else
        {
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

    public void HideCard()
    {
        col.enabled = false;
        myRender.enabled = false;
        usedRenderer.enabled = true;
    }

    public void ShowToolTip(ToolTip tooltip)
    {
        tooltip.ToolTipInfo(cardBase.cardName, cardBase.description, cardBase.cardSprite);
    }
}
