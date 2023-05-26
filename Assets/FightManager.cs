using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;

    [SerializeField] private float _playerTimeToPlay;
    private float _currentPlayerTimeToPlay;

    private Coroutine _playerTurnRoutine;

    private Queue<ICharacter> characterQueue = new Queue<ICharacter>();

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;
    }
    private void StartFight()
    {
        OrderCharacters();
        _playerTurnRoutine = StartCoroutine(PlayerTurn());
    }

    private void PartyMemberTurn()
    {

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


    private IEnumerator PlayerTurn()
    {
        while (true)
        {

            // player turn logic

            _currentPlayerTimeToPlay -= Time.deltaTime;
            if(_currentPlayerTimeToPlay <= 0)
            {
                _currentPlayerTimeToPlay = _playerTimeToPlay;
                PartyMemberTurn();
                yield break;
            }
            yield return null;
        }
    }

    #region sort
    private int Compare(ICharacter x, ICharacter y)
    {
        if(x.GetSpeed() == y.GetSpeed()) return 0;
        if(x.GetSpeed() == 0) return -1;
        if(x.GetSpeed() == 0) return +1;

        return y.GetSpeed() - x.GetSpeed();
    }
    #endregion
}
