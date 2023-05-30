using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetInputField : MonoBehaviour
{
    CardBase cardBase;
    [SerializeField] TextMeshProUGUI tmpPro;
    [SerializeField] TMP_InputField input;

    public void Init(CardBase carbase)
    {
        cardBase = carbase;
        tmpPro.text = carbase.name;
    }

    public void Set()
    {
        cardBase.manaCost = int.Parse(input.text);
    }
}
