using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GfxGenerator : MonoBehaviour
{

    private GameObject gfx;



    public void GenerateGfx(GameObject go)
    {
        Instantiate(go, transform);
        gfx = go;
        GetComponentInParent<Character>().Animator = go.GetComponentInChildren<Animator>();
    }
}
