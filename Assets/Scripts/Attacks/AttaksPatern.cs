using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region AttackClass
[System.Serializable]
public class AttackClass
{
    public enum AttackConditions
    {
        None,
        HpLowerThan,
        HpHiggerThan
    }

    public AttacksObject attack;
    public AttackConditions condition;
    [Range(0f, 100f)]
    public int percentageValue;

}
[System.Serializable]
public class AttacksPatern
{
    public string paternName;
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
        AllieBuffed
    }
    public AttacksObject attack;
    public SpecialAttacksTrigerMode trigerMode;

}
#endregion
