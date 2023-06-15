using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour
{
    [SerializeField] ManaObject manaEventHandler;
    [SerializeField] Image manaSlider;
    [SerializeField] float manaPerSeconds = 2;
    [SerializeField] AnimationCurve curve;
    Coroutine updateRoutine;
    float mana;

    private void Start()
    {
        manaEventHandler.currentMana = 0f;
        mana = manaEventHandler.currentMana;
        StartUpdate();
    }

    private void OnEnable()
    {
        manaEventHandler.manaUpdate += StartUpdate;
    }

    private void OnDestroy()
    {
        manaEventHandler.manaUpdate -= StartUpdate;
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
                manaSlider.fillAmount = mana / manaEventHandler.maxMana;
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
                manaSlider.fillAmount = mana / manaEventHandler.maxMana;
                yield return null;
            }
        }
        
        manaSlider.fillAmount = manaEventHandler.currentMana / manaEventHandler.maxMana;
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