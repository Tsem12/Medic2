using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceSettersManager : MonoBehaviour
{
    public static List<ReferenceSetter> refs = new List<ReferenceSetter>();

    public static void ReconnectAll()
    {
        foreach(ReferenceSetter r in refs)
        {
            r.ReconnectAllValues();
        }
    }



}
