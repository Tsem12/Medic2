using UnityEngine;

[System.Serializable]
public class Message
{
    public enum MessageType
    {
        Strengthened,
        Initiative,
        Regenerating,
        Shielded,
        Fatigue,
        Poisoned,
        Fired,
        Sleeped,
        Restrained,
        Stunned,
        Disapeared,
        ShieldedWithReflect,
        Taunting,
        Die,
        Revive,
        Afk,
        Heal
    }
    public string name;
    public MessageType messageType;
    public MessageBody[] messages;
}

[System.Serializable]
public class MessageBody
{
    public enum Expression
    {
        Happy,
        Angry
    }
    public CharacterObjets.BossType bossType;
    public Expression expression;
    [TextArea]
    public string message;
}