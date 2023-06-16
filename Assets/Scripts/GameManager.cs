using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ICharacter character;

    private GAMESTATE _gameState;
    [SerializeField] private RectTransform _optionMenu;
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
        _optionMenuinitLocation = _optionMenu.localPosition;
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToggleMenu()
    {
        _optionMenu.gameObject.SetActive(!_optionMenu.gameObject.activeSelf);
    }

    public void ToggleMenu(bool state)
    {
        if(state == _optionMenu.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            _optionMenu.gameObject.SetActive(state);
        }
    }

}
