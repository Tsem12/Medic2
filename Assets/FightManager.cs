using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;

    [SerializeField] private float _playerTimeToPlay;
    private float _currentPlayerTimeToPlay;

    private int _globalAgro;

    private Coroutine _playerTurnRoutine;
    private Coroutine _partymembersTurnRoutine;

    private Queue<ICharacter> _characterQueue = new Queue<ICharacter>();
    private List<ICharacter> characterList = new List<ICharacter>();

    public Enemie Enemie { get => _enemie; set => _enemie = value; }
    public PartyMember[] PartyMembers { get => _partyMembers; set => _partyMembers = value; }
    public int GlobalAgro { get => _globalAgro; set => _globalAgro = value; }

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;

        characterList.Add(Enemie.GetComponent<ICharacter>());

        foreach (ICharacter character in PartyMembers)
        {
            characterList.Add(character);
        }
        StartFight();
    }
    private void StartFight()
    {
        SetGlobalAgroValue();
        foreach (ICharacter character in characterList)
        {
            character.SetTarget();
        }
        OrderCharacters();
        _playerTurnRoutine = StartCoroutine(PlayerTurn());
    }

    private void PartyMemberTurn()
    {
        _partymembersTurnRoutine = StartCoroutine(PartyMembersTurnRoutine());
    }

    private void SetGlobalAgroValue()
    {
        int agroValue = 0;
        foreach(PartyMember partyMember in PartyMembers)
        {
            agroValue += partyMember.GetComponent<ICharacter>().GetAgro();
        }
        GlobalAgro = agroValue;
        //Debug.Log($"agrovalue: {agroValue}");
    }

    private void OrderCharacters()
    {
        characterList.Sort(Compare);

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
