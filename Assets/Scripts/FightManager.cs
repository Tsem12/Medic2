using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using NaughtyAttributes;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private LevelDataObject _levelData;
    [SerializeField] private AllReferences _refs;

    [SerializeField] private Enemie _enemie;
    [SerializeField] private PartyMember[] _partyMembers;
    [SerializeField] private Image _playerSlider;
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private ParticleSystem _shuffleParticles;

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
    public event Action OnWin;

    #region statusSprites
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite baseAttack;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite strengthened;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite initiative;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite regenerating;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite shielded;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite fatigue;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite poisoned;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite sleeped;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite restrained;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite stunned;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite taunt;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite reflectShield;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite fire;
    [Foldout("StatusSprites")]
    [SerializeField] private Sprite disapear;
    public Sprite BaseAttack { get => baseAttack; }
    public Sprite Strengthened { get => strengthened; }
    public Sprite Initiative { get => initiative; }
    public Sprite Regenerating { get => regenerating; }
    public Sprite Shielded { get => shielded; }
    public Sprite Fatigue { get => fatigue; }
    public Sprite Poisoned { get => poisoned; }
    public Sprite Sleeped { get => sleeped; }
    public Sprite Restrained { get => restrained; }
    public Sprite Stunned { get => stunned; }
    public Sprite Taunt { get => taunt; set => taunt = value; }
    public Sprite ReflectShield { get => reflectShield; set => reflectShield = value; }
    public Sprite Fire { get => fire; set => fire = value; }
    public Sprite Disapear { get => disapear; set => disapear = value; }
    #endregion
    public Enemie Enemie { get => _enemie; set => _enemie = value; }
    public PartyMember[] PartyMembers { get => _partyMembers; set => _partyMembers = value; }
    public int GlobalAgro { get => _globalAgro; set => _globalAgro = value; }
    public bool EnableDebug { get => _enableDebug; }
    public FightState State { get; private set; }
    public int CurrentTurn { get => _currentTurn; }
    public List<ICharacter> PartyMembersList { get => _partyMembersList; set => _partyMembersList = value; }
    public List<ICharacter> CharacterList { get => _characterList; set => _characterList = value; }
    public ParticleSystem ShuffleParticles { get => _shuffleParticles; set => _shuffleParticles = value; }

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;
        if(_levelData.difficulty == LevelDataObject.Difficulty.Easy)
        {
            _playerSlider.gameObject.SetActive(false);
        }


        ICharacter _enemie = Enemie.GetComponent<ICharacter>();

        CharacterList.Add(Enemie.GetComponent<ICharacter>());

        foreach (ICharacter character in PartyMembers)
        {
            CharacterList.Add(character);
            PartyMembersList.Add(character);
        }
        StartTurn();
    }

    public void TriggerEvent(AttackEvent.SpecialAttacksTrigerMode triger, int value)
    {
        foreach(ICharacter chara in CharacterList)
        {
            chara.TrackSpecialAtkEvents(triger, value);
        }
    }
    private void StartTurn()
    {
        //ReferenceSettersManager.ReconnectAll();
        _currentTurn++;
        OnTurnBegin?.Invoke();

        SetGlobalAgroValue();
        foreach (ICharacter character in CharacterList)
        {
            character.SetAttack();
            character.SetTarget();
            character.SetPartyMemberAttackPreview(character.GetNextAttackSprite());
        }
        OrderCharacters();
        _playerTurnRoutine = StartCoroutine(PlayerTurn());
    }

    public void ResetTargets()
    {
        foreach (ICharacter character in CharacterList)
        {
            character.SetTarget();
            character.SetPartyMemberAttackPreview(character.GetNextAttackSprite());
        }
        OrderCharacters();
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

    public void OrderCharacters()
    {
        _characterQueue.Clear();
        CharacterList.Sort(Compare);

        foreach(ICharacter character in CharacterList.ToList())
        {
            Status status = character.GetStatus(Status.StatusEnum.Initiative);
            if (status != null)
            {
                _characterQueue.Enqueue(character);
            }
        }

        foreach (ICharacter character in CharacterList.ToList())
        {
            if (!_characterQueue.Contains(character))
            {
                _characterQueue.Enqueue(character);
            }
        }

    }

    private bool ArePartyStillAlive()
    {
        int availableMember = PartyMembersList.Count;
        foreach (ICharacter chara in PartyMembersList)
        {
            Status s = chara.GetStatus(Status.StatusEnum.Disapeared);
            if (s != null)
            {
                availableMember--;
                continue;
            }
            if (chara.IsDead())
            {
                availableMember--;
            }
        }
        return availableMember > 0;
    }

    public void FinishTurn()
    {
        if(_state == FightState.PlayerTurn)
        {
            _refs.audioManager.Play("ButtonPress2");
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

            if (_levelData.difficulty != LevelDataObject.Difficulty.Easy)
            { 
                _currentPlayerTimeToPlay -= Time.deltaTime;
            }
            if(_playerSlider != null && _levelData.difficulty != LevelDataObject.Difficulty.Easy)
            {
                _playerSlider.fillAmount = _currentPlayerTimeToPlay / _playerTimeToPlay;
            }
            if (_currentPlayerTimeToPlay <= 0 || _endTurn)
            {
                _endTurn = false;
                if(_levelData.difficulty != LevelDataObject.Difficulty.Easy)
                {
                    _currentPlayerTimeToPlay = _playerTimeToPlay;
                }
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
                    _refs.gameManager.ToggleMenu(true);
                yield break;
            }


            if (chara.IsDead())
            {
                PartyMembersList.Remove(chara);

                if (_enableDebug)
                    Debug.Log($"{chara.GetName()} is dead he cannot attack");

                if(!ArePartyStillAlive())
                {
                    if (_enableDebug)
                        Debug.Log("GAME OVER");

                    _refs.gameManager.ToggleMenu(true);
                    _enemie.ClearAllStatus();
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
            _refs.gameManager.ToggleMenu(true);
            OnWin?.Invoke();
            yield break;
        }

        if (ArePartyStillAlive())
        {
            StartTurn();
        }
        else
        {
            
            Debug.Log("GAMEOVER");
            _refs.gameManager.ToggleMenu(true);
            _enemie.ClearAllStatus();
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
