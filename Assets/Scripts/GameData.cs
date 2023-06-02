using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelProgression;

    public bool[] levelUnlocked;
    public bool[] spellUnlocked;

    public int difficulty;
    public float musicVolume;
    public float sfxVolume;



    public GameData(){
        levelProgression = 0;
        levelUnlocked = new bool[4];
        levelUnlocked[0] = true;
        spellUnlocked = new bool[13];
        spellUnlocked[0] = true;
        spellUnlocked[1] = true;
        spellUnlocked[2] = true;
        spellUnlocked[3] = true;
        difficulty = 0;
        musicVolume = 1;
        sfxVolume = 1;
    }
    
    
}
