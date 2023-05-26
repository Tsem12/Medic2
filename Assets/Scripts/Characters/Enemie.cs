using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : Character
{
    [SerializeField] private EnemiesObjects _enemieObj;
    public override int GetSpeed()
    {
        return _enemieObj.speed;
    }
}
