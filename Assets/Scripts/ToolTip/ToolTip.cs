using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI toolTipName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] UnityEngine.UI.Image image;

    public void ToolTipInfo(string name, string desc, Sprite toolTipImage)
    {
        toolTipName.text = name;
        description.text = desc;
        image.sprite = toolTipImage;
        image.color = Color.white;
    }
}
