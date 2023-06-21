using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using NaughtyAttributes;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

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
    [SerializeField] private Image _switchCardButton;
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private ParticleSystem _shuffleParticles;
    [SerializeField] private ParticleSystem _switchBossParticles;
    [SerializeField] private DataSetter[] _dataSetters;

    [SerializeField] private float _playerTimeToPlay;
    [SerializeField] private int _chanceToTriggerDialogue;
    [SerializeField] private int _chanceIntervalToTriggerDialogue;
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
    public int ChanceToTriggerAfxDialogue { get => _chanceToTriggerDialogue; set => _chanceToTriggerDialogue = value; }

    private void Start()
    {
        _currentPlayerTimeToPlay = _playerTimeToPlay;
        if(_levelData.difficulty == LevelDataObject.Difficulty.Easy || _levelData.difficulty == LevelDataObject.Difficulty.EndLess)
        {
            _playerSlider.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if (_levelData.difficulty == LevelDataObject.Difficulty.Hard)
        {
            _switchCardButton.gameObject.SetActive(false);
        }


        ICharacter _enemie = Enemie.GetComponent<ICharacter>();

        CharacterList.Add(Enemie.GetComponent<ICharacter>());

        foreach (ICharacter character in PartyMembers)
        {
            CharacterList.Add(character);
            PartyMembersList.Add(character);
        }
    }

    public void TriggerEvent(AttackEvent.SpecialAttacksTrigerMode triger, int value)
    {
        foreach(ICharacter chara in CharacterList)
        {
            chara.TrackSpecialAtkEvents(triger, value);
        }
    }
    public void StartTurn()
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

        float thinkingTime = 0f;
        while (true)
        {
            // player turn logic
            thinkingTime += Time.deltaTime;
            if(thinkingTime >= _chanceIntervalToTriggerDialogue)
            {
                thinkingTime = 0;
                int random = Random.Range(0, ChanceToTriggerAfxDialogue + 1);
                if(random == 0)
                {
                    ICharacter chara = PartyMembersList[Random.Range(0, PartyMembersList.Count)];
                    if (!chara.IsDead())
                    {
                        chara.GetMessageBehaviour().DisplayMessage(Message.MessageType.Afk, chara.getCharaObj(), _enemie.CharacterObj.bossType);
                    }
                }
            }
            if (_levelData.difficulty == LevelDataObject.Difficulty.Hard)
            { 
                _currentPlayerTimeToPlay -= Time.deltaTime;
            }
            if(_playerSlider != null && _levelData.difficulty != LevelDataObject.Difficulty.Hard)
            {
                _playerSlider.fillAmount = _currentPlayerTimeToPlay / _playerTimeToPlay;
            }
            if (_currentPlayerTimeToPlay <= 0 || _endTurn)
            {
                _endTurn = false;
                if(_levelData.difficulty != LevelDataObject.Difficulty.Hard)
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
                Win();
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

                    _refs.gameManager.ToggleWinLooseMenu(false);
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
            Win();
            yield break;
        }

        if (ArePartyStillAlive())
        {
            StartTurn();
        }
        else
        {
            
            Debug.Log("GAMEOVER");
            _refs.gameManager.ToggleWinLooseMenu(false);
            _enemie.ClearAllStatus();
        }
    }

    private void Win()
    {
        if(_levelData.difficulty == LevelDataObject.Difficulty.EndLess)
        {
            StartCoroutine(StartNewBossRoutine());
        }
        else
        {
            _refs.gameManager.ToggleWinLooseMenu(true);
        }
        OnWin?.Invoke();
    }

    IEnumerator StartNewBossRoutine()
    {
        _switchBossParticles.Play();

        string currentMusic = _levelData.levels[_levelData.currentSceneIndex].themeName;

        yield return new WaitForSeconds(0.1f);
        _levelData.currentSceneIndex = (_levelData.currentSceneIndex + 1) % (_levelData.levels.Length);
        foreach (DataSetter data in _dataSetters)
        {
            data.Reconnectvalue();
        }

        //_refs.audioManager.FadeToNextMusic(currentMusic, _levelData.levels[_levelData.currentSceneIndex].themeName);
        _refs.audioManager.Fade(currentMusic, _levelData.levels[_levelData.currentSceneIndex].themeName);
        _enemie.AssignValues();
        CharacterList.Add(_enemie.GetComponent<ICharacter>());
        yield return new WaitForEndOfFrame();
        _enemie.Health.ResetHealth();
        StartTurn();
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
