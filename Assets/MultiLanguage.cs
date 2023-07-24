using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using NaughtyAttributes;
using System;

public class MultiLanguage : MonoBehaviour
{
    private GameData gameData;

    

    public void Awake()
    {
        gameData = SaveSystem.Load();
        Debug.Log($"langage is :  + {gameData.Language}, {gameData.index}");
        FIXTHELANGUAGE();
        LocalizationManager.Read();
        LocalizationManager.Language = gameData.Language;
        SaveSystem.save(gameData);
    }

    private void FIXTHELANGUAGE()
    {
        gameData = SaveSystem.Load();
        switch (gameData.index)
        {
            case 0:
                gameData.Language = "Français";
                break;

            case 1:
                gameData.Language = "English";
                break;

            case 2:
                gameData.Language = "Allemand";
                break;

            case 3:
                gameData.Language = "Espagnol";
                break;

            case 4:
                gameData.Language = "Chinois";
                break;

            case 5:
                gameData.Language = "Japonais";
                break;

            case 6:
                gameData.Language = "Portuguais";
                break;

            case 7:
                gameData.Language = "Russe";
                break;
        }
        SaveSystem.save(gameData);
    }

    [Button]
    private void DisplayLanguauge() => Debug.Log(gameData.Language);
}
