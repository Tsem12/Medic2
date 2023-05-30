using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCard : MonoBehaviour
{
    [SerializeField] CardBase[] cards;
    [SerializeField] GameObject inputFieldPrefab;
    List<GameObject> cardsDebug = new List<GameObject>();

    public void ShowDebug()
    {
        foreach (CardBase item in cards)
        {
            GameObject obj = Instantiate(inputFieldPrefab);
            obj.GetComponent<SetInputField>().Init(item);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            cardsDebug.Add(obj);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject item in cardsDebug)
        {
            Destroy(item);
        }
        cardsDebug.Clear();
    }
}
