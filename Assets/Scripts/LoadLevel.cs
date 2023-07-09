using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public void LoadCardSelector()
    {
        DOTween.KillAll();
        SceneManager.LoadSceneAsync("CardSelector");
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
        DOTween.KillAll();
    }

    public void LoadGame()
    {
        DOTween.KillAll();
        SceneManager.LoadSceneAsync("Game");
    }
}
