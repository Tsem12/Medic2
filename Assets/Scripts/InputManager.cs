using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;
    void Start()
    {
        Debug.Log(_refs.camera.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
