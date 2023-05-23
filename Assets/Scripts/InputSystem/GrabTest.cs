using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTest : MonoBehaviour, IGrabbable
{
    public IEnumerator Grab(Vector2 pos)
    {
        while (true)
        {
            transform.position = pos;
            yield return null;
        }
    }
}
