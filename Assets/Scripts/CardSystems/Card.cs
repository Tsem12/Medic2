using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour,IInteractable
{
    public CardBase carBase;
    [SerializeField] AllReferences refs;
    [SerializeField] InputHandlerObject inputObject;
    [HideInInspector] public bool wasPlayed = false;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer myRender;
    [SerializeField] SpriteRenderer usedRenderer;
    [SerializeField] float size = 0.5f;
    [SerializeField] TextMeshProUGUI tmpro;

    private void Start()
    {
        carBase.manaObject.manaAddTurn += CheckIfInteractable;
        refs.fightManager.OnTurnEnd += EndInteractable;
        refs.fightManager.OnTurnBegin += EnableTurn;
        CheckIfInteractable();
        myRender.sprite = carBase.cardSprite;
        myRender.color = Color.white;
        usedRenderer.sprite = carBase.cardSprite;
        usedRenderer.color = Color.grey;
        tmpro.text = carBase.manaCost.ToString();
    }

    public bool ApplyEffect()
    {
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, size);
        col.enabled = true;
        if (collision != null && collision.gameObject.CompareTag("PartyMember"))
        {
            if(!collision.gameObject.GetComponent<PartyMember>().IsDead() && carBase.cardBehaviour == CardBehaviour.resurection)
            {
                return false;
            }
            carBase.ApplyEffectOfTheCard(collision.GetComponent<Character>());
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
        if(!handlerObject.isChaningCards && !wasPlayed)
        {
            if(ApplyEffect())
            {
                col.enabled = false;
                myRender.enabled = false;
                usedRenderer.enabled = true;
                wasPlayed = true;
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
        if(wasPlayed)
        {
            col.enabled = true;
            myRender.enabled = true;
            usedRenderer.enabled = false;
            wasPlayed = false;
        }
    }
}
