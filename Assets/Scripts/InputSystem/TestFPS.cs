using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestFPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        text.text = (1.0f / Time.deltaTime).ToString();
    }
}
