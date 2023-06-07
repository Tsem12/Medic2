using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour,IInteractable
{
    public CardBase carBase;
    [SerializeField] AllReferences refs;
    [SerializeField] InputHandlerObject inputObject;
    [SerializeField] CardDeckBuilder deckBuilder;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer myRender;
    [SerializeField] SpriteRenderer usedRenderer;
    [SerializeField] float size = 0.5f;
    [SerializeField] TextMeshProUGUI tmpro;
    [SerializeField] int index;
    [SerializeField] ManaObject manaObject;

    private void Start()
    {
        carBase = deckBuilder.deck[index];
        carBase.manaObject.manaAddTurn += CheckIfInteractable;
        refs.fightManager.OnTurnEnd += EndInteractable;
        refs.fightManager.OnTurnBegin += EnableTurn;
        CheckIfInteractable();
        myRender.sprite = carBase.cardSprite;
        myRender.color = Color.white;
        usedRenderer.sprite = carBase.cardSprite;
        usedRenderer.color = Color.grey;
        tmpro.text = carBase.manaCost.ToString();
        carBase.manaObject.manaUpdate += CheckIfInteractable;
    }

    public bool ApplyEffect()
    {
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, size);
        col.enabled = true;
        if (collision != null && collision.gameObject.CompareTag("PartyMember"))
        {
            if (!collision.gameObject.GetComponent<PartyMember>().IsDead() && carBase.cardBehaviour == CardBehaviour.resurection)
            {
                return false;
            }
            Debug.Log("PlayCard");
            if(!carBase.ApplyEffectOfTheCard(collision.gameObject.GetComponent<Character>()))
            {
                return false;
            }
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
                col.enabled = false;
                myRender.enabled = false;
                usedRenderer.enabled = true;
            }
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
        if(carBase.manaCost <= carBase.manaObject.currentMana)
        {
            transform.tag = "Grabbable";
        }
        else
        {
            transform.tag = "Untagged";
        }
    }

    void EndInteractable()
    {
        transform.tag = "Untagged";
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
    }
}
