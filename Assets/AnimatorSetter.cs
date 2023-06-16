using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorSetter : MonoBehaviour
{
    Character chara;
    [SerializeField] private ParticulesHandeler _particulesHandeler;

    public UnityEvent AnimEvent;
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

    public void OnAnimEvent()
    {
        AnimEvent?.Invoke();
    }

}
