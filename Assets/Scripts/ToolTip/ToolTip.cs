using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    //Flemme je fais de la merde si je veut ok
    [SerializeField] GameObject one;
    [SerializeField] TextMeshProUGUI toolTipNameOne;
    [SerializeField] TextMeshProUGUI descriptionOne;
    [SerializeField] UnityEngine.UI.Image imageOne;

    [SerializeField] GameObject two;
    [SerializeField] TextMeshProUGUI toolTipNameTwo;
    [SerializeField] TextMeshProUGUI descriptionTwo;
    [SerializeField] UnityEngine.UI.Image imageTwo;

    [SerializeField] GameObject three;
    [SerializeField] TextMeshProUGUI toolTipNameThree;
    [SerializeField] TextMeshProUGUI descriptionThree;
    [SerializeField] UnityEngine.UI.Image imageThree;

    public void ToolTipInfo(string name, string desc, Sprite toolTipImage)
    {
        one.SetActive(true);
        two.SetActive(false);
        three.SetActive(false);
        toolTipNameOne.text = name;
        descriptionOne.text = desc;
        imageOne.sprite = toolTipImage;
        imageOne.color = Color.white;
    }

    public void ToolTipMultiInfo(List<(string name, string desc, Sprite toolTipImage)> infoList)
    {
        one.SetActive(false);
        two.SetActive(false);
        three.SetActive(false);

        Debug.Log(infoList);
        if (infoList.Count >= 1)
        {
            one.SetActive(true);
            toolTipNameOne.text = infoList[0].name;
            descriptionOne.text = infoList[0].desc;
            imageOne.sprite = infoList[0].toolTipImage;
            imageOne.color = Color.white;
        }

        if (infoList.Count >= 2)
        {
            two.SetActive(true);
            toolTipNameTwo.text = infoList[1].name;
            descriptionTwo.text = infoList[1].desc;
            imageTwo.sprite = infoList[1].toolTipImage;
            imageTwo.color = Color.white;
        }

        if (infoList.Count >= 3)
        {
            three.SetActive(true);
            toolTipNameThree.text = infoList[2].name;
            descriptionThree.text = infoList[2].desc;
            imageThree.sprite = infoList[2].toolTipImage;
            imageThree.color = Color.white;
        }
    }
}