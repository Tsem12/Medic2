using Assets.SimpleLocalization;
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
    [SerializeField] LocalizedText ltNameOne;
    [SerializeField] LocalizedText ltDescOne;
    [SerializeField] UnityEngine.UI.Image imageOne;

    [SerializeField] GameObject two;
    [SerializeField] TextMeshProUGUI toolTipNameTwo;
    [SerializeField] TextMeshProUGUI descriptionTwo;
    [SerializeField] LocalizedText ltNameTwo;
    [SerializeField] LocalizedText ltDescTwo;
    [SerializeField] UnityEngine.UI.Image imageTwo;

    [SerializeField] GameObject three;
    [SerializeField] TextMeshProUGUI toolTipNameThree;
    [SerializeField] TextMeshProUGUI descriptionThree;
    [SerializeField] LocalizedText ltNameThree;
    [SerializeField] LocalizedText ltDescThree;
    [SerializeField] UnityEngine.UI.Image imageThree;

    public void ToolTipInfo(string name, string desc, Sprite toolTipImage)
    {
        one.SetActive(true);
        two.SetActive(false);
        three.SetActive(false);
        ltNameOne.LocalizationKey = name;
        ltDescOne.LocalizationKey = desc;
        ltNameOne.Localize();
        ltDescOne.Localize();
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
            Debug.Log($"{infoList[0].name},{infoList[0].desc}, ");
            one.SetActive(true);
            ltNameOne.LocalizationKey = infoList[0].name;
            ltDescOne.LocalizationKey = infoList[0].desc;
            ltNameOne.Localize();
            ltDescOne.Localize();
            imageOne.sprite = infoList[0].toolTipImage;
            imageOne.color = Color.white;
        }

        if (infoList.Count >= 2)
        {
            two.SetActive(true);
            ltNameTwo.LocalizationKey = infoList[1].name;
            ltDescTwo.LocalizationKey = infoList[1].desc;
            ltNameTwo.Localize();
            ltDescTwo.Localize();
            imageTwo.sprite = infoList[1].toolTipImage;
            imageTwo.color = Color.white;
        }

        if (infoList.Count >= 3)
        {
            three.SetActive(true);
            ltNameThree.LocalizationKey = infoList[2].name;
            ltDescThree.LocalizationKey = infoList[2].desc;
            ltNameThree.Localize();
            ltDescThree.Localize();
            imageThree.sprite = infoList[2].toolTipImage;
            imageThree.color = Color.white;
        }
    }
}