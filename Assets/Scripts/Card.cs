using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] CardBase carBase;
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
}
