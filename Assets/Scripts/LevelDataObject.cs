using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLoader", menuName = "Data")]
public class LevelDataObject : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public CharacterObjets boss;
        public GameObject animator;
        public Sprite background;
    }

    public enum Difficulty
    {
        Easy,
        Hard,
        EndLess
    }

    public int currentSceneIndex { get; private set; }
    public LevelData[] levels;
    public Difficulty difficulty;

    public void ChooseScene(int index)
    {
        currentSceneIndex = index;
    }

    public void ChooseDifficulty(int difficultyIndex)
    {
        switch (difficultyIndex)
        {
            case 0:
                difficulty = Difficulty.Easy;
                break;
            case 1:
                difficulty = Difficulty.Hard;
                break;
            case 2:
                difficulty = Difficulty.EndLess;
                break;
        }
    }

}
