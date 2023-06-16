using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField] GameObject Window_0;
    [SerializeField] GameObject Window_1;
    [SerializeField] GameObject Window_2;
    [SerializeField] GameObject Window_3;
    [SerializeField] GameObject Window_4;
    [SerializeField] GameObject Window_5;

    private int index = 0;


    public void Left()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = 0;
        }
        Debug.Log(index);
        UpdateWindow();
    }
    public void Rigt()
    {
        if (index < 5)
        {
            index++;
        }
        else
        {
            index = 5;
        }
        Debug.Log(index);
        UpdateWindow();


    }

    public void WipeWindows()
    {
        Window_0.SetActive(false);
        Window_1.SetActive(false);
        Window_2.SetActive(false);
        Window_3.SetActive(false);
        Window_4.SetActive(false);
        Window_5.SetActive(false);
    }

    public void UpdateWindow()
    {
        switch(index)
        {
            case 0:
                WipeWindows();
                Window_0.SetActive(true);
                break;

            case 1:
                WipeWindows();
                Window_1.SetActive(true);
                break;

            case 2:
                WipeWindows();
                Window_2.SetActive(true);
                break;

            case 3:
                WipeWindows();
                Window_3.SetActive(true);
                break;

            case 4:
                WipeWindows();
                Window_4.SetActive(true);
                break;

            case 5:
                WipeWindows();
                Window_5.SetActive(true);
                break;
        }
    }
}
