using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SetInputField : MonoBehaviour
{
    CardBase cardBase;
    [SerializeField] TextMeshProUGUI tmpPro;
    [SerializeField] TMP_InputField input;
    [SerializeField] TextMeshProUGUI textButton;

    public void Init(CardBase carbase)
    {
        cardBase = carbase;
        input.text = cardBase.manaCost.ToString();
        tmpPro.text = carbase.name;
    }

    public void Set()
    {
        cardBase.manaCost = int.Parse(input.text);
    }

    public void EnableSwitch()
    {

    }
}
