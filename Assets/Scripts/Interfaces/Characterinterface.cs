public interface ICharacter
{
    public int GetMaxHealth();
    public int GetCurrentHealth();
    public void SetCurrentHealth(int newValue);
    public int GetSpeed();
    public int GetAgro();
    public void StartTurn();
    public bool IsPlaying();
    public bool IsDead();
    public void Kill();
    public void EndTurn();
    public void SetTarget();
    public string GetName();
    public void TakeDamage(int damage);
}

