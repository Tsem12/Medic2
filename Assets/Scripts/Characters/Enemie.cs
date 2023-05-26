using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour, ICharacter
{
    [SerializeField] private EnemiesObjects _enemieObj;
    public int GetSpeed()
    {
        return _enemieObj.speed;
    }

}
