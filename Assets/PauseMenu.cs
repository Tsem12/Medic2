using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject optionMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject LevelMenu;
    [SerializeField] GameObject accountWindow;
    [SerializeField] GameObject HowToPlayWindow;
    private bool isPaused = false;
    private bool isHowToPlayToggled = false;
    private bool isLevelMenuToggled = false;

    public void Pause()
    {
        audioManager.Play("ButtonPress1");

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
        audioManager.Play("ButtonPress1");
        optionMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void Home()
    {
        audioManager.Play("ButtonPress1");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        audioManager.Play("ButtonPress1");
        Home();
    }

    public void Retry()
    {
        audioManager.Play("ButtonPress1");
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        Debug.Log("next level");
    }

    public void ToggleHowToPlay()
    {
        audioManager.Play("ButtonPress1");

        if (isHowToPlayToggled)
        {
            HowToPlayWindow.SetActive(false);
            Time.timeScale = 1f;
            isHowToPlayToggled = false;
        }
        else
        {
            HowToPlayWindow.SetActive(true);
            optionMenu.SetActive(false);
            Time.timeScale = 0f;
            isHowToPlayToggled = true;

        }
    }




    public void ToggleLevelMenu()
    {
        audioManager.Play("ButtonPress1");

        if (isLevelMenuToggled)
        {
            LevelMenu.SetActive(false);
            MainMenu.SetActive(true);
            isLevelMenuToggled = false;
        }
        else
        {
            LevelMenu.SetActive(true);
            MainMenu.SetActive(false);
            isLevelMenuToggled = true;
        }
    }
}
