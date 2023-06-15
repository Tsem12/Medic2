using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetter : MonoBehaviour
{
    Character chara;
    [SerializeField] private ParticulesHandeler _particulesHandeler;
    private void Awake()
    {
        chara = GetComponentInParent<Character>();
    }
    private void Start()
    {
        if(_particulesHandeler != null)
        {
            chara.ParticuleHandler = _particulesHandeler;
        }
        chara.Animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        chara.Attack();
    }
}
