using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] GameObject easyPannel;
    [SerializeField] GameObject hardPannel;
    public Button[] Buttons;
    public GameData gameData;
    private void Awake()
    {
        gameData = SaveSystem.Load();
        SwitchDifficulty();
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level "+levelId;
        SceneManager.LoadScene(levelName);
    }

    public void SwitchDifficulty()
    {
        if (gameData.isHardDifficulty)
        {
            hardPannel.SetActive(false);
            easyPannel.SetActive(true);
        }
        else
        {
            easyPannel.SetActive(false);
            hardPannel.SetActive(true);
        }
        gameData.isHardDifficulty = !gameData.isHardDifficulty;
        SaveSystem.save(gameData);
    }
    
}
