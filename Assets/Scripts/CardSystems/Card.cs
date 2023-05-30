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
        col.enabled = false;
        Collider2D collision = Physics2D.OverlapCircle(transform.position, 1f);
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
