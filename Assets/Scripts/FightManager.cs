using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using NaughtyAttributes;

public class FightManager : MonoBehaviour
{
    public enum FightState
    {
        PlayerTurn,
        IATurn,
        None
    }

    private FightState _state = FightState.None;

    [SerializeField] private bool _enableDebug;

    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;
    [SerializeField] private Image _playerSlider;
    [SerializeField] private GameObject _endTurnButton;

    [SerializeField] private float _playerTimeToPlay;
    private float _currentPlayerTimeToPlay;

    private int _globalAgro;
    private int _currentTurn;

    private Coroutine _playerTurnRoutine;
    private Coroutine _partymembersTurnRoutine;

    private Queue<ICharacter> _characterQueue = new Queue<ICharacter>();
    private List<ICharacter> _characterList = new List<ICharacter>();
    private List<ICharacter> _partyMembersList = new List<ICharacter>();

    private bool _endTurn;

    public event Action OnTurnBegin;
    public event Action OnTurnEnd;
    public Enemie Enemie { get => _enemie; set => _enemie = value; }
    public PartyMember[] PartyMembers { get => _partyMembers; set => _partyMembers = value; }
    public int GlobalAgro { get => _globalAgro; set => _globalAgro = value; }
    public bool EnableDebug { get => _enableDebug; }
    public FightState State { get; private set; }
    public int CurrentTurn { get => _currentTurn; }

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
        StartTurn();
    }

    public void TriggerEvent(AttackEvent.SpecialAttacksTrigerMode triger)
    {
        foreach(ICharacter chara in _characterList)
        {
            chara.TrackSpecialAtkEvents(triger);
        }
    }
    private void StartTurn()
    {
        ReferenceSettersManager.ReconnectAll();
        _currentTurn++;
        OnTurnBegin?.Invoke();

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
        OnTurnEnd?.Invoke();
        _partymembersTurnRoutine = StartCoroutine(IATurnRoutine());
        OnTurnEnd?.Invoke();
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

    public void FinishTurn()
    {
        if(_state == FightState.PlayerTurn)
        {
            _endTurn = true;
        }
    }

    private IEnumerator PlayerTurn()
    {
        _state = FightState.PlayerTurn;
        _endTurnButton.SetActive(true);
        if (_enableDebug)
            Debug.Log("Player turn start");
        while (true)
        {

            // player turn logic

            _currentPlayerTimeToPlay -= Time.deltaTime;
            if(_playerSlider != null)
            {
                _playerSlider.fillAmount = _currentPlayerTimeToPlay / _playerTimeToPlay;
            }
            if (_currentPlayerTimeToPlay <= 0 || _endTurn)
            {
                _endTurn = false;
                _currentPlayerTimeToPlay = _playerTimeToPlay;
                _playerSlider.fillAmount = 0;

                if(_enableDebug)
                    Debug.Log("Player turn is over");

                _endTurnButton.SetActive(false);
                PartyMemberTurn();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator IATurnRoutine()
    {
        _state = FightState.IATurn;
        while (_characterQueue.Count != 0)
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

                    _state = FightState.None;
                    yield break;
                }
                continue;
            }

            chara.StartTurn();
            yield return new WaitUntil( () => !chara.IsPlaying());
            chara.EndTurn();



        }
        _state = FightState.None;

        if (_enemie.GetComponent<ICharacter>().IsDead())
        {
            if (_enableDebug)
                Debug.Log("Boss defeated");
            yield break;
        }

        if (ArePartyStillAlive())
        {
            StartTurn();
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
