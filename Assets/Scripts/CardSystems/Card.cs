using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour,IInteractable
{
    [SerializeField] CardBase carBase;
    [HideInInspector] public bool wasPlayed = false;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer rend;

    public bool ApplyEffect()
    {
        Debug.Log("Test");
        Collider2D col = Physics2D.OverlapCircle(transform.position, 1f);
        if (col != null && col.gameObject.CompareTag("PartyMember"))
        {
            carBase.ApplyEffectOfTheCard(col.GetComponent<PartyMember>().GetPartyMemberObj());
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
                rend.enabled = false;
                wasPlayed = true;
            }
            ResetPos();
        }
        else
        {

        }
    }
}
