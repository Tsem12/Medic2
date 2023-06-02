using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public enum StatusEnum
    {
        Strengthened,
        Initiative,
        Regenerating,
        Shielded,
        Fatigue,
        Poisoned,
        Sleeped,
        Restrained,
        Stunned
    }

    public StatusEnum status;
    public int turnDuration;
    public int remainTurn;

    public int value;

    public Status(StatusEnum status, int turnDuration)
    {
        this.status = status;
        this.turnDuration = turnDuration;
        ResetStatus();
    }

    public Status(StatusEnum status, int turnDuration, int value)
    {
        this.status = status;
        this.turnDuration = turnDuration;
        this.value = value;




        ResetStatus();
    }

    public void ResetStatus()
    {
        turnDuration = remainTurn;
    }

}
