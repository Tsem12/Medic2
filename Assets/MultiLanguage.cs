using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;

public class MultiLanguage : MonoBehaviour
{
    public GameData gameData;
    public void Awake()
    {
        gameData = SaveSystem.Load();
        LocalizationManager.Read();
        Debug.Log("langage is : " + gameData.Language);
        LocalizationManager.Language = "Français";
        SaveSystem.save(gameData);
    }
}
