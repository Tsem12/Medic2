using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public void LoadCardSelector()
    {
        SceneManager.LoadSceneAsync("CardSelector");
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
