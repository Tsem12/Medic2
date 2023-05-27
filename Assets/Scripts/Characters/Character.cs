using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ICharacter
{
    [SerializeField] protected AllReferences _refs;

    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;

    private bool _isPlaying;

    private Coroutine _attackRoutine;


    private void OnValidate()
    {
        AssignValues();
    }

    #region Abstarct methods
    protected abstract void Attack();
    public abstract int GetSpeed();
    public abstract int GetAgro();
    public abstract void SetTarget();
    public abstract void AssignValues();
    #endregion
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


    public virtual bool IsPlaying()
    {
        return _isPlaying;
    }

    private IEnumerator AttackRoutine()
    {
        Attack();
        yield return new WaitForSeconds(0.5f);
        _isPlaying = false;
        _attackRoutine = null;
    }

    public string GetName()
    {
        return gameObject.name;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
