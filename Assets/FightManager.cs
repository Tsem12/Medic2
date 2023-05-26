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
    private Coroutine _partymembersTurnRoutine;

    private Queue<ICharacter> _characterQueue = new Queue<ICharacter>();

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;
        StartFight();
    }
    private void StartFight()
    {
        OrderCharacters();
        _playerTurnRoutine = StartCoroutine(PlayerTurn());
    }

    private void PartyMemberTurn()
    {
        _partymembersTurnRoutine = StartCoroutine(PartyMembersTurnRoutine());
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
            _characterQueue.Enqueue(character);
        }
    }


    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player turn start");
        while (true)
        {

            // player turn logic

            _currentPlayerTimeToPlay -= Time.deltaTime;
            if(_currentPlayerTimeToPlay <= 0)
            {
                _currentPlayerTimeToPlay = _playerTimeToPlay;
                Debug.Log("Player time is over");
                PartyMemberTurn();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator PartyMembersTurnRoutine()
    {
        while(_characterQueue.Count != 0)
        {
            ICharacter chara =  _characterQueue.Dequeue();
            chara.StartTurn();
            yield return new WaitUntil( () => !chara.IsPlaying());
            chara.EndTurn();
        }
        Debug.Log("End of turn");
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
