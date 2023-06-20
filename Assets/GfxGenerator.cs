using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GfxGenerator : MonoBehaviour
{

    private GameObject gfx;



    public void GenerateGfx(GameObject go)
    {
        if(gfx != null)
        {
            DestroyImmediate(gfx, true);
        }
        GameObject instance = Instantiate(go, transform);
        gfx = instance;
        GetComponentInParent<Character>().Animator = instance.GetComponentInChildren<Animator>();
    }
}
