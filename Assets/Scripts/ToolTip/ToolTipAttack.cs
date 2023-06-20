using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipAttack : MonoBehaviour, IToolTip
{
    [SerializeField] Character character;

    public void ShowToolTip(ToolTip tooltip)
    {
        List <(string name, string desc, Sprite toolTipImage)> list = new List<(string name, string desc, Sprite toolTipImage)>();

        foreach (var item in character.IncomingAttacks)
        {
            list.Add((item.attackName,item.description,item.attackSpriteOverRide));
        }
        
        tooltip.ToolTipMultiInfo(list);
    }
}
