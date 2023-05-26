using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ICharacter
{
    private bool _isPlaying;

    private Coroutine _attackRoutine;

    public virtual void StartTurn()
    {
        _isPlaying = true;
        Debug.Log($"{gameObject.name} is attacking");
        _attackRoutine = StartCoroutine(AttackRoutine());
    }
    public virtual void EndTurn()
    {
        Debug.Log($"{gameObject.name} finished his turn");
    }

    public abstract int GetSpeed();
    public abstract int GetAgro();

    public virtual bool IsPlaying()
    {
        return _isPlaying;
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _isPlaying = false;
        _attackRoutine = null;
    }

}
