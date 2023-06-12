using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReferenceSetter;

public class DataSetter : MonoBehaviour
{
    public enum DataType
    {
        BossObject,
        BossAnimator,
        Background
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

    private void ConnectValues(DataType item)
    {
        switch (item)
        {
            case DataType.BossObject:
                GetComponent<Character>().CharacterObj = _levelData.levels[_levelData.currentSceneIndex].boss;
                break;
            case DataType.BossAnimator:
                GetComponent<Animator>().runtimeAnimatorController = _levelData.levels[_levelData.currentSceneIndex].animator;
                break;
            case DataType.Background:
                GetComponent<SpriteRenderer>().sprite = _levelData.levels[_levelData.currentSceneIndex].background;
                break;
        }
    }
}
