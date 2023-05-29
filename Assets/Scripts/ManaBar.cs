using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour
{
    //placeHolder Vars
    private bool notDefinitiveIsPlayerDead = false;



    //Definitive Vars
    [SerializeField] Slider manaSlider;
    [SerializeField] Slider preManaSlider;
    [SerializeField] float maxMana = 10;
    [SerializeField] float manaRecuperationCooldown = 2;
    [SerializeField] float smoothnessFactor = 5f;



    private float minMana = 0;
    private float currentMana;
    private float currentPreMana;
    private float maxPreMana;
    private float minPreMana;

    private void Awake()
    {
        
        currentMana = minMana;
        maxPreMana = maxMana * 10;
        minPreMana = minMana;
        StartCoroutine(startCoroutines());

    }

    private void Update()
    {
        CheckSliderValues();

        if(currentMana ==7)
        {
           
            RemoveMana(7);
        }

        Debug.Log(currentMana);
        Debug.Log(currentPreMana);
    }


    void AddMana(float manaAmount)
    {
        if ((manaAmount<=0)||(manaAmount>maxMana)||(notDefinitiveIsPlayerDead))
        {
            Debug.LogWarning("Impossible to Add Mana you made a mistake");
        }
        else
        {
            float newMana = currentMana + manaAmount;
            StartCoroutine(IncreaseManaSmoothly(newMana));
        } 
    }

    void PreAddMana(int manaAmount)
    {
        if ((manaAmount <= 0) || (manaAmount > maxPreMana) || (notDefinitiveIsPlayerDead))
        {
            Debug.LogWarning("Impossible to Add Mana you made a mistake");
        }
        else
        {
            float newMana = currentPreMana + manaAmount;
            currentPreMana = newMana;
        }
    }

    void RemoveMana(int manaAmount)
    {
        if ((manaAmount <= 0)||(manaAmount > maxMana) || (notDefinitiveIsPlayerDead))
        {
            Debug.LogWarning("Impossible to Remove Mana you made a mistake");
        }
        else
        {
            float newMana = currentMana - manaAmount;
            currentMana = Mathf.Max(newMana, minMana);
            currentPreMana = currentMana / 10 + 10;
        }
    }

    void ReduceMaxMana(int newMax)
    {
        if (newMax <= 0)
        {
            Debug.LogWarning("new manaMax Value is incorrect");
        }
        else
        {
            maxMana = newMax;
        }
    }

    void CheckSliderValues()
    {
        //visual values
        manaSlider.maxValue = maxMana;
        manaSlider.minValue = minMana;
        manaSlider.value = currentMana;

        preManaSlider.maxValue = maxPreMana;
        preManaSlider.minValue = minPreMana;
        preManaSlider.value = currentPreMana;


        if (currentMana < minMana)
        { currentMana = minMana; }
        if (currentMana > maxMana)
        { currentMana = maxMana; }

        if (currentPreMana > maxPreMana)
        { currentPreMana = maxPreMana; }

    }

    IEnumerator ManaRefill()
    {
        AddMana(1);
        yield return new WaitForSeconds(manaRecuperationCooldown);
        StartCoroutine(ManaRefill());
    }

    IEnumerator PreManaRefill()
    {
        PreAddMana(1);
        yield return new WaitForSeconds(manaRecuperationCooldown/10);
        StartCoroutine(PreManaRefill());
    }

    IEnumerator startCoroutines()
    {
        StartCoroutine(PreManaRefill());
        yield return new WaitForSeconds(manaRecuperationCooldown);
        StartCoroutine(ManaRefill());
    }

    IEnumerator IncreaseManaSmoothly(float targetMana)
    {
        while ((currentMana < targetMana))
        {
            currentMana = Mathf.MoveTowards(currentMana, targetMana, Time.deltaTime * smoothnessFactor);
            yield return null;
        }
        currentMana = Mathf.Min(currentMana, maxMana);
    }
}

