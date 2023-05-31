using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class FightManager : MonoBehaviour
{
    [SerializeField] private bool _enableDebug;

    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;
    [SerializeField] private Slider _playerSlider;

    [SerializeField] private float _playerTimeToPlay;
    private float _currentPlayerTimeToPlay;

    private int _globalAgro;

    private Coroutine _playerTurnRoutine;
    private Coroutine _partymembersTurnRoutine;

    private Queue<ICharacter> _characterQueue = new Queue<ICharacter>();
    private List<ICharacter> _characterList = new List<ICharacter>();
    private List<ICharacter> _partyMembersList = new List<ICharacter>();

    public Enemie Enemie { get => _enemie; set => _enemie = value; }
    public PartyMember[] PartyMembers { get => _partyMembers; set => _partyMembers = value; }
    public int GlobalAgro { get => _globalAgro; set => _globalAgro = value; }
    public bool EnableDebug { get; private set; }

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;

        ICharacter _enemie = Enemie.GetComponent<ICharacter>();

        _characterList.Add(Enemie.GetComponent<ICharacter>());

        foreach (ICharacter character in PartyMembers)
        {
            _characterList.Add(character);
            _partyMembersList.Add(character);
        }
        StartFight();
    }
    private void StartFight()
    {
        SetGlobalAgroValue();
        foreach (ICharacter character in _characterList)
        {
            character.SetTarget();
        }
        OrderCharacters();
        _playerTurnRoutine = StartCoroutine(PlayerTurn());
    }

    private void PartyMemberTurn()
    {
        _partymembersTurnRoutine = StartCoroutine(IATurnRoutine());
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
        _characterList.Sort(Compare);

        foreach(ICharacter character in _characterList)
        {
            _characterQueue.Enqueue(character);
        }
    }

    private bool ArePartyStillAlive()
    {
        foreach(ICharacter chara in _partyMembersList)
        {
            if(!chara.IsDead())
                return true;
        }
        return false;
    }

    private IEnumerator PlayerTurn()
    {
        if (_enableDebug)
            Debug.Log("Player turn start");
        while (true)
        {

            // player turn logic

            _currentPlayerTimeToPlay -= Time.deltaTime;
            if(_playerSlider != null)
            {
                _playerSlider.value = _currentPlayerTimeToPlay / _playerTimeToPlay;
            }
            if (_currentPlayerTimeToPlay <= 0)
            {
                _currentPlayerTimeToPlay = _playerTimeToPlay;

                if(_enableDebug)
                    Debug.Log("Player time is over");

                PartyMemberTurn();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator IATurnRoutine()
    {
        while(_characterQueue.Count != 0)
        {
            ICharacter chara =  _characterQueue.Dequeue();

            if (_enemie.GetComponent<ICharacter>().IsDead())
            {
                if (_enableDebug)
                    Debug.Log("Boss defeated");
                yield break;
            }

            if(chara.IsDead())
            {
                _partyMembersList.Remove(chara);
                if (_enableDebug)
                    Debug.Log($"{chara.GetName()} is dead he cannot attack");
                if(!ArePartyStillAlive())
                {
                    if (_enableDebug)
                        Debug.Log("GAME OVER");
                    yield break;
                }
                continue;
            }

            chara.StartTurn();
            yield return new WaitUntil( () => !chara.IsPlaying());
            chara.EndTurn();



        }
        if(ArePartyStillAlive())
        {
            StartFight();
        }
        else
        {
            Debug.Log("GAMEOVER");
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
