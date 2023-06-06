using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour
{
    [SerializeField] ManaObject manaEventHandler;
    [SerializeField] Slider manaSlider;
    [SerializeField] float manaPerSeconds = 2;
    [SerializeField] AnimationCurve curve;
    Coroutine updateRoutine;
    float mana;

    private void Start()
    {
        mana = manaEventHandler.currentMana;
    }

    private void OnEnable()
    {
        manaEventHandler.manaUpdate += StartUpdate;
    }

    private void OnDisable()
    {
        manaEventHandler.manaUpdate -= StartUpdate;
    }

    IEnumerator UpdateMana()
    {
        
        if(mana <= manaEventHandler.currentMana)
        {
            float i = 0f;
            while (mana <= manaEventHandler.currentMana)
            {
                mana += curve.Evaluate(i) * manaPerSeconds;
                i += Time.deltaTime;
                manaSlider.value = mana / manaEventHandler.maxMana;
                yield return null;
            }
        }
        else
        {
            float i = 0f;
            while (mana >= manaEventHandler.currentMana)
            {
                mana -= curve.Evaluate(i) * manaPerSeconds;
                i += Time.deltaTime;
                manaSlider.value = mana / manaEventHandler.maxMana;
                yield return null;
            }
        }
        
        manaSlider.value = manaEventHandler.currentMana / manaEventHandler.maxMana;
        updateRoutine = null;
    }

    void StartUpdate()
    {
        if(updateRoutine == null)
        {
            updateRoutine = StartCoroutine(UpdateMana());
        }
    }
}