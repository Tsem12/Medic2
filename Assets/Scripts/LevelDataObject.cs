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
        public string themeName;
    }

    public enum Difficulty
    {
        Easy,
        Hard,
        EndLess
    }

    public int currentSceneIndex { get;  set; }
    public LevelData[] levels;
    public Difficulty difficulty;
    public Queue<int> indexQueue = new Queue<int>();

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

    public void FillQueue()
    {
        indexQueue.Clear();
        int easyLevelRandom = Random.Range(0, 2);
        int HardLevelRandom = Random.Range(0, 2);
        
        indexQueue.Enqueue(easyLevelRandom == 0 ? 0 : 1);
        indexQueue.Enqueue(HardLevelRandom == 0 ? 2 : 3);
        indexQueue.Enqueue(easyLevelRandom == 0 ? 1 : 0);
        indexQueue.Enqueue(HardLevelRandom == 0 ? 3 : 2);
    }

    public void DequeueIndex()
    {
        if(indexQueue.Count <= 0)
        {
            FillQueue();
        }
        currentSceneIndex = indexQueue.Dequeue();
    }

}
