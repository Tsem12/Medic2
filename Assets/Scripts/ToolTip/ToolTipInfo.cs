using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipInfo : MonoBehaviour, IToolTip
{
    [SerializeField] string objectName;
    [SerializeField] string description;
    [SerializeField] Sprite image;

    public void ShowToolTip(ToolTip tooltip)
    {
        tooltip.ToolTipInfo(objectName, description, image);
    }
}
