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
    public float musicVolume;
    public float sfxVolume;
    public string Language;
    


    public GameData(){
        levelProgression = 0;
        levelUnlocked = new bool[4];
        levelUnlocked[0] = true;
        spellUnlocked = new bool[10];
        isHardDifficulty = false;
        musicVolume = 1;
        sfxVolume = 1;
        Language = "Fran�ais";
    }
    
    
}
