using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ReferenceSetter;

public class DataSetter : MonoBehaviour
{
    public enum DataType
    {
        BossObject,
        BossAnimator,
        Background,
        AudioManager
    }

    [SerializeField] private LevelDataObject _levelData;
    [SerializeField] private DataType[] _dataTypes;

    private void Awake()
    {
        foreach (DataType item in _dataTypes)
        {
            ConnectValues(item);
        }
    }

    public void Reconnectvalue()
    {
        foreach(DataType item in _dataTypes)
        {
            ConnectValues(item);
        }
    }

    private void ConnectValues(DataType item)
    {
        switch (item)
        {
            case DataType.BossObject:
                GetComponent<Character>().CharacterObj = _levelData.levels[_levelData.currentSceneIndex].boss;
                break;
            case DataType.BossAnimator:
                GetComponent<GfxGenerator>().GenerateGfx(_levelData.levels[_levelData.currentSceneIndex].animator);
                break;
            case DataType.Background:
                GetComponent<Image>().sprite = _levelData.levels[_levelData.currentSceneIndex].background;
                break;
            case DataType.AudioManager:
                GetComponent<AudioManager>().StartMusic = _levelData.levels[_levelData.currentSceneIndex].themeName;
                break;
        }
    }
}
