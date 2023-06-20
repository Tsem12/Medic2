using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ICharacter character;

    private GAMESTATE _gameState;
    [SerializeField] private LevelDataObject _levelData;
    [SerializeField] private RectTransform _pauseMenu;
    [SerializeField] private RectTransform _winMenu;
    [SerializeField] private RectTransform _looseMenu;


    [SerializeField] private RectTransform _nextLevelButton;
    private Vector3 _optionMenuinitLocation;

    public GAMESTATE GameState { get => _gameState; set => _gameState = value; }

    public GameData gameData;

    public enum GAMESTATE
    {
        Menu,
        Playing,
        Paused
    }


    private void Awake()
    {
        gameData = SaveSystem.Load();
        _optionMenuinitLocation = _pauseMenu.localPosition;
        if(_levelData.currentSceneIndex == _levelData.levels.Length - 1)
        {
            _nextLevelButton.gameObject.SetActive(false);
        }
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void TogglePause()
    {
        _pauseMenu.gameObject.SetActive(!_pauseMenu.gameObject.activeSelf);
    }

    public void SetNextLevel()
    {
        _levelData.currentSceneIndex++;
    }

    public void ToggleWinLooseMenu(bool isWin)
    {
        _pauseMenu.gameObject.SetActive(false);
        if(isWin)
        {
            _winMenu.gameObject.SetActive(true);
        }
        else
        {
            _looseMenu.gameObject.SetActive(true);
        }
    }

}
