using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelProgression;

    public bool[] levelUnlocked;
    public bool[] spellUnlocked;

    public bool isHardDifficulty;
    public bool musicVolume;
    public bool sfxVolume;
    public string Language;
    public int index;
    public int bossStreak;
    


    public GameData(){
        levelProgression = 0;
        levelUnlocked = new bool[4];
        levelUnlocked[0] = true;
        spellUnlocked = new bool[10];
        isHardDifficulty = false;
        musicVolume = true;
        sfxVolume = true;
        Language = "Français";
        index = 0;
        bossStreak = 0;
    }
    
    
}
