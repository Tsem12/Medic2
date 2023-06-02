using UnityEngine;

public interface ICharacter
{
    public int GetMaxHealth();
    public int GetCurrentHealth();
    public int GetSpeed();
    public int GetAgro();
    public int GetMaxHealthBar();
    public string GetName();
    public Sprite GetIcone();
    public bool IsPlaying();
    public bool IsDead();
    public void Kill();
    public void StartTurn();
    public void EndTurn();
    public void TakeDamage(int damage);
    public void SetTarget();
    public void SetCurrentHealth(int newValue);
}

