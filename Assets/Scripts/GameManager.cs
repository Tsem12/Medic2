using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ICharacter character;

    private GAMESTATE _gameState;

    public GAMESTATE GameState { get => _gameState; set => _gameState = value; }

    public GameData gameData;

    public enum GAMESTATE
    {
        Menu,
        Playing,
        Paused
    }


    private void OnEnable()
    {
        SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
    }

    private static void SceneManagerOnSceneUnloaded(Scene scene)
    {
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
    }

    private void Awake()
    {
        gameData = SaveSystem.Load();
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
