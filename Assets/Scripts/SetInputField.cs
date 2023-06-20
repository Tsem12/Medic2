using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SetInputField : MonoBehaviour
{
    CardBase card;
    [SerializeField] TextMeshProUGUI tmpPro;
    [SerializeField] TMP_InputField input;
    [SerializeField] TextMeshProUGUI textButton;

    public void Init(CardBase cardBase)
    {
        card = cardBase;
        input.text = cardBase.manaCost.ToString();
        tmpPro.text = cardBase.name;
    }

    public void Set()
    {
        card.manaCost = int.Parse(input.text);
    }

    public void EnableSwitch()
    {

    }
}
