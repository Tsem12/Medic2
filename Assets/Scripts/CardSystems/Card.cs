using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour,IInteractable
{
    [SerializeField] CardBase carBase;
    [SerializeField] AllReferences refs;
    [HideInInspector] public bool wasPlayed = false;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer myRender;
    [SerializeField] SpriteRenderer usedRenderer;
    [SerializeField] float size = 0.5f;

    private void Start()
    {
        carBase.manaObject.manaAddTurn += CheckIfInteractable;
        CheckIfInteractable();
    }

    public bool ApplyEffect()
    {
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, size);
        col.enabled = true;
        if (collision != null && collision.gameObject.CompareTag("PartyMember"))
        {
            carBase.ApplyEffectOfTheCard(collision.GetComponent<IHealable>());  
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
        else
        {

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
}
