using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipInfo : MonoBehaviour, IToolTip
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite image;
    public void ShowToolTip(Transform canva)
    {
        if (canva.gameObject != null)
        {
            canva.gameObject.GetComponent<ToolTip>().ToolTipInfo(name, description, image);
        }
    }
}
