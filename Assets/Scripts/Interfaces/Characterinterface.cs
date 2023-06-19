using UnityEngine;

public interface ICharacter
{
    public int GetMaxHealth();
    public int GetCurrentHealth();
    public int GetSpeed();
    public int GetAgro();
    public int GetMaxHealthBar();
    public string GetName();
    public AttacksObject GetNextAttack();
    public void SetAttack();
    public bool IsPlaying();
    public bool IsDead();
    public void Kill();
    public void StartTurn();
    public void EndTurn();
    public void TakeDamage(AttacksObject attack, int additionalDamage = 0);
    public void SetTarget();
    public void SetCurrentHealth(int newValue);
    public void TrackSpecialAtkEvents(AttackEvent.SpecialAttacksTrigerMode trigerMode, int value);
    public Status GetStatus(Status.StatusEnum status);
    public void AddStatus(Status status);
    public void TryRemoveStatus(Status.StatusEnum status);
    public void GetAllStatus();
    public void SetBossAttackPreview(Sprite sprite);
    public void SetPartyMemberAttackPreview(Sprite sprite);
    public void SetIncommingAttack(AttacksObject atk, int index = 0);
    public void ClearIncommingAttack();
    public Sprite GetNextAttackSprite();
    public void CheckStatus();
    public void ClearIncomingAttacks();
    public void ClearAllStatus();
    public void UpdateBar();
    public ParticulesHandeler GetParticulHandeler();
    public MessageBehaviour GetMessageBehaviour();
}

