using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemiesObjects", menuName = "Characters/Enemies")]
public class EnemiesObjects : ScriptableObject
{
    [Header("Health")]
    public int baseHealth;
    [Range(1, 3)]
    public int numberOfHealthBar;

    public int baseSpeed;
    public int baseDamage;

    public Sprite icon;
}
