using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetter : MonoBehaviour
{
    private void Start()
    {
        GetComponentInParent<Character>().Animator = GetComponent<Animator>();
    }
}
