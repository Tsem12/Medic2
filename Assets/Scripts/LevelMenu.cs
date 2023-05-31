using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] Buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i =0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        for(int i = 0; i < unlockedLevel; i++)
        {
            Buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level "+levelId;
        SceneManager.LoadScene(levelName);
    }
}
