using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ICharacter
{
    private bool _isPlaying;

    public virtual void StartTurn()
    {
        _isPlaying = true;
    }
    public virtual void EndTurn()
    {
        _isPlaying = false;
    }

    public abstract int GetSpeed();

    public virtual bool IsPlaying()
    {
        return _isPlaying;
    }
}
