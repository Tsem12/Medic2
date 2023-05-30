using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemiesObjects", menuName = "Characters/Enemies")]
public class EnemiesObjects : ScriptableObject
{
    public int baseSpeed;
    public int baseDamage;
    public int baseHealth;

    public Sprite icon;
}
