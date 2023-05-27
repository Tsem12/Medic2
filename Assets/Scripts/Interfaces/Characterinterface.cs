public interface ICharacter
{
    public int GetMaxHealth();
    public int GetCurrentHealth();
    public int GetSpeed();
    public int GetAgro();
    public void StartTurn();
    public bool IsPlaying();
    public void EndTurn();
    public void SetTarget();
    public string GetName();
    //public void SetTarget(ICharacter target);
}

