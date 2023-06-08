using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region AttackClass
[System.Serializable]
public class AttackClass
{

    public enum ConditionMode
    {
        DontAttackWithoutCondition,
        UseBaseAttackWithoutCondition
    }
    public enum AttackConditions
    {
        None,
        HpLowerThan,
        HpHiggerThan,
        HpBarLost,
        HpBarNotLost,
        Random
    }

    [Range(1, 3)]
    public int nrbOfTargets;

    public ConditionMode attackConditionsMode;
    public Status.StatusEnum selfStatus;
    public AttacksObject attack;
    public AttackConditions condition;
    public AttacksObject ConditionalAttack;
    [Range(0f, 100f)]
    [Tooltip("for HpLowerThan & HpHiggerThan value = percentage of health")]
    public int percentageValue;
    [Tooltip("for HpBarLost value = number of bar lost")]
    public int value;

}
[System.Serializable]
public class AttacksPatern
{
    public enum PaternInteruptMode
    {
        Interupt,
        DontInteruptLastInQueue,
        DontInteruptFirstInQueue
    }

    public string paternName;
    public PaternInteruptMode interuptMode;
    public AttackClass[] attacks;

    public Queue<AttackClass> attackQueue = new Queue<AttackClass>();

    public void FillQueue()
    {
        attackQueue.Clear();
        foreach(AttackClass atk in attacks)
        {
            attackQueue.Enqueue(atk);
        }
    }
}

[System.Serializable]
public class AttackEvent
{
    public enum SpecialAttacksTrigerMode
    {
        LooseHealthBar,
        AllieBuffed,
        TurnPassed
    }
    public AttackClass attack;
    public SpecialAttacksTrigerMode trigerMode;

}
#endregion
