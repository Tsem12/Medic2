using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour, IGrabbable
{
    public IEnumerator Grab(Vector2 pos)
    {
        while (true)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10f;
            yield return null;
        }
    }
}
