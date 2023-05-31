using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScript : MonoBehaviour
{
 //   public List<Status> status = new List<Status>();
}


public class Status
{
    public class Buff : Status
    {
        public class BuffSpell : Buff
        {
            public string spellName;
        }
    }
    public class Debuff : Status
    {
        public class DebuffSpell : Debuff
        {
            public string spellName;
        }
    }


}



