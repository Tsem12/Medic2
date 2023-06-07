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
        gameData = SaveSysteme.Load();
    }

    public void ChangeLanguage(string Language)
    {
        gameData.Language = Language;
        LocalizationManager.Language = gameData.Language;
        SaveSysteme.save(gameData);
        Debug.Log("language changed");
    }
}
