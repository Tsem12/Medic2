using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour,IInteractable
{
    [SerializeField] CardBase carBase;
    [HideInInspector] public bool wasPlayed = false;
    [SerializeField] CardHandlerObject handlerObject;
    [SerializeField] BoxCollider2D col;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] SpriteRenderer usedRenderer;

    public bool ApplyEffect()
    {
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
                Debug.Log("Test");
                col.enabled = false;
                renderer.enabled = false;
                usedRenderer.enabled = true;
                wasPlayed = true;
            }
            ResetPos();
        }
        else
        {

        }
    }
}
