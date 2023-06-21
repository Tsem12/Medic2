using UnityEngine;

[System.Serializable]
public class Message
{
    public enum MessageType
    {
        Strengthened,
        Initiative,
        Shielded,
        Fatigue,
        Poisoned,
        Fired,
        Sleeped,
        Restrained,
        Stunned,
        Disapeared,
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
        Angry,
        Disgusted
    }
    public CharacterObjets.BossType bossType;
    public Expression expression;
    public string localizationKey;
}