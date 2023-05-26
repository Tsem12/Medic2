using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;

    private Queue<ICharacter> characterQueue = new Queue<ICharacter>();

    private void Start()
    {
        OrderCharacters();
    }

    private void OrderCharacters()
    {
        List<ICharacter> characterList = new List<ICharacter>();
        characterList.Add(_enemie.GetComponent<ICharacter>());
        foreach(ICharacter character in _partyMembers)
        {
            characterList.Add(character);
        }
        characterList.Sort(Compare);
        Debug.Log(characterList.Count);
        foreach(ICharacter character in characterList)
        {
            characterQueue.Enqueue(character);
        }

        for (int i = 0; i < characterList.Count; i++)
        {
            ICharacter chara = characterQueue.Dequeue();
            Debug.Log(chara.GetSpeed());
        }
    }

    private int Compare(ICharacter x, ICharacter y)
    {
        if(x.GetSpeed() == y.GetSpeed()) return 0;
        if(x.GetSpeed() == 0) return -1;
        if(x.GetSpeed() == 0) return +1;

        return y.GetSpeed() - x.GetSpeed();
    }
}
