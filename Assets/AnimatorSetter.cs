using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetter : MonoBehaviour
{
    Character chara;
    private void Awake()
    {
        chara = GetComponentInParent<Character>();
    }
    private void Start()
    {
        chara.Animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        chara.Attack();
    }
}
