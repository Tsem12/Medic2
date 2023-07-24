using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization;

public class LanguageButton : MonoBehaviour
{
    public Button[] Buttons;
    private GameData gameData;

    public void Awake()
    {
        gameData = SaveSystem.Load();
        Debug.Log(gameData.Language);
        LocalizationManager.Language = gameData.Language;
    }

    public void ChangeLanguage(string Language)
    {
        gameData = SaveSystem.Load();
        gameData.Language = Language;
        Debug.Log(gameData.Language);
        LocalizationManager.Language = gameData.Language;
        SaveSystem.save(gameData);
        Debug.Log($"language changed to {Language} was ");
    }
}
