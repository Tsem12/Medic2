using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaNumber : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] ManaObject manaObject;


    private void Start()
    {
        manaObject.manaUpdate += SetText;
    }

    private void OnDestroy()
    {
        manaObject.manaUpdate -= SetText;
    }

    void SetText()
    {
        textMeshProUGUI.text = ((int)manaObject.currentMana).ToString();
    }
}
