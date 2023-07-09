using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization;
using NaughtyAttributes;
using System;

public class MultiLanguage : MonoBehaviour
{
    private GameData gameData;

    public GameData GameData { get => gameData; set => gameData = value; }

    public void Awake()
    {
        GameData = SaveSystem.Load();
        FIXTHELANGUAGE();
        Debug.Log($"langage is :  + {GameData.Language}, {GameData.index}");
        LocalizationManager.Read();
        LocalizationManager.Language = GameData.Language;
        SaveSystem.save(GameData);
    }

    private void FIXTHELANGUAGE()
    {
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
    private void DisplayLanguauge() => Debug.Log(GameData.Language);
}
