using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBar : MonoBehaviour
{
    //placeHolder Vars
    private bool notDefinitiveIsPlayerDead = false;



    //Definitive Vars
    [SerializeField] int maxMana = 10;
    [SerializeField] Slider manaSlider;
    [SerializeField] Slider preManaSlider;
    [SerializeField] float manaRecuperationCooldown = 2;
    private int minMana = 0;
    private int currentMana;


    private int currentPreMana;
    private int maxPreMana;
    private int minPreMana;

    private void Awake()
    {
        
        currentMana = minMana;
        maxPreMana = maxMana * 10;
        minPreMana = minMana;
        StartCoroutine(startCoroutines());

    }

    private void Update()
    {
        

        //visual values
        manaSlider.maxValue = maxMana;
        manaSlider.minValue = minMana;
        manaSlider.value = currentMana;

        preManaSlider.maxValue = maxPreMana;
        preManaSlider.minValue = minPreMana;
        preManaSlider.value = currentPreMana;


        if (currentMana < minMana)
        {currentMana = minMana;}
        if (currentMana > maxMana)
        {currentMana = maxMana;}

        if (currentPreMana > maxPreMana)
        { currentPreMana = maxPreMana; }

    }


    void AddMana(int manaAmount)
    {
        if ((manaAmount<=0)||(manaAmount>maxMana)||(notDefinitiveIsPlayerDead))
        {
            Debug.LogWarning("Impossible to Add Mana you made a mistake");
        }
        else
        {
            int newMana = currentMana + manaAmount;
            currentMana = Mathf.Min(newMana, maxMana);
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
            int newMana = currentPreMana + manaAmount;
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
            int newMana = currentMana - manaAmount;
            currentMana = Mathf.Max(newMana, minMana);
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
}

