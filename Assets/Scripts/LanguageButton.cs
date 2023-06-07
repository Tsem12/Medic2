using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization;

public class LanguageButton : MonoBehaviour
{
    public Button[] Buttons;
    public GameData gameData;

    public void Awake()
    {
        gameData = SaveSystem.Load();
    }

    public void ChangeLanguage(string Language)
    {
        gameData.Language = Language;
        LocalizationManager.Language = gameData.Language;
        SaveSystem.save(gameData);
        Debug.Log("language changed");
    }
}
