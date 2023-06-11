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
        public RuntimeAnimatorController animator;
        public Sprite background;
    }
    public int currentSceneIndex { get; private set; }
    public LevelData[] levels;

    public void ChooseScene(int index)
    {
        currentSceneIndex = index;
    }

}
