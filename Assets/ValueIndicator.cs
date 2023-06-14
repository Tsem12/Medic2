using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _damageTexts;
    [SerializeField] private TextMeshProUGUI[] _healTexts;

    private void Start()
    {
        foreach(TextMeshProUGUI text in _damageTexts)
        {
            text.color = Color.red;
            Debug.Log(text.color);
        }
    }

}
