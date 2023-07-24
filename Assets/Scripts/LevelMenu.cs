using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    [SerializeField] GameObject easyPannel;
    [SerializeField] GameObject hardPannel;
    [SerializeField] GameObject hardButton;
    [SerializeField] GameObject easyButton;
    public Button[] Buttons;
    private GameData gameData;
    private void Awake()
    {
        gameData = SaveSystem.Load();
        gameData.isHardDifficulty = false;
        SwitchDifficulty();
        SaveSystem.save(gameData);
    }
    public void OpenLevel()
    {
        audioManager.Play("ButtonPress1");
        string levelName = "Game";
        SceneManager.LoadSceneAsync(levelName);
    }

    public void SwitchDifficulty()
    {
        audioManager.Play("ButtonPress1");

        if (!gameData.isHardDifficulty)
        {
            hardPannel.SetActive(false);
            easyPannel.SetActive(true);
            hardButton.SetActive(false);
            easyButton.SetActive(true);
        }
        else
        {
            easyPannel.SetActive(false);
            hardPannel.SetActive(true);
            easyButton.SetActive(false);
            hardButton.SetActive(true);
        }
        gameData = SaveSystem.Load();
        gameData.isHardDifficulty = !gameData.isHardDifficulty;
        SaveSystem.save(gameData);
        
    }
    
}
