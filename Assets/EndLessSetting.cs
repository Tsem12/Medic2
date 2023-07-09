using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLessSetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;

    private void Awake()
    {
        UpdateScore();
    }
    private void UpdateScore()
    {
        GameData gameData = SaveSystem.Load();
        _score.text = gameData.bossStreak.ToString();
    }

    [Button]
    private void ResetScore()
    {
        GameData gameData = SaveSystem.Load();
        gameData.bossStreak = 0;
        SaveSystem.save(gameData);
        UpdateScore();
    }
}
