using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelection : MonoBehaviour
{
    [SerializeField] GameObject Français;
    [SerializeField] GameObject English;
    [SerializeField] GameObject Allemand;
    [SerializeField] GameObject Espagne;
    [SerializeField] GameObject Chine;
    [SerializeField] GameObject Japon;
    [SerializeField] GameObject Portugal;
    [SerializeField] GameObject Russe;
    private int index = 0;
    [SerializeField] LanguageButton languageButton;
    GameData gameData;
    public void Awake()
    {
        gameData = SaveSystem.Load();
        index = gameData.index;
        UpdateWindow();
    }

    public void Left()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = 7;
        }
        gameData.index = index;
        UpdateWindow();
        SaveSystem.save(gameData);
    }



    public void Rigt()
    {
        if (index < 7)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        gameData.index = index;
        UpdateWindow();
        SaveSystem.save(gameData);
    }

    public void WipeWindows()
    {
        Français.SetActive(false);
        English.SetActive(false);
        Espagne.SetActive(false);
        Allemand.SetActive(false);
        Chine.SetActive(false);
        Japon.SetActive(false);
        Portugal.SetActive(false);
        Russe.SetActive(false);
    }
    void UpdateWindow()
    {
        switch (index)
        {
            case 0:
                WipeWindows();
                Français.SetActive(true);
                languageButton.ChangeLanguage("Français");
                break;

            case 1:
                WipeWindows();
                English.SetActive(true);
                languageButton.ChangeLanguage("English");

                break;

            case 2:
                WipeWindows();
                Allemand.SetActive(true);
                languageButton.ChangeLanguage("Allemand");
                break;

            case 3:
                WipeWindows();
                Espagne.SetActive(true);
                languageButton.ChangeLanguage("Espagnol");
                break;

            case 4:
                WipeWindows();
                Chine.SetActive(true);
                languageButton.ChangeLanguage("Chinois");

                break;

            case 5:
                WipeWindows();
                Japon.SetActive(true);
                languageButton.ChangeLanguage("Japonais");
                break;

            case 6:
                WipeWindows();
                Portugal.SetActive(true);
                languageButton.ChangeLanguage("Portuguais");
                break;

            case 7:
                WipeWindows();
                Russe.SetActive(true);
                languageButton.ChangeLanguage("Russe");
                break;
        }
        SaveSystem.save(gameData);
    }
}
