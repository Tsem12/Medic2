using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject accountWindow;
    private bool isPaused = false;

    public void Pause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
        optionMenu.SetActive(true);
        Time.timeScale = 0f;
            isPaused = true;

        }

    }

    public void Resume()
    {
        optionMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Home();
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        Debug.Log("next level");
    }
}
