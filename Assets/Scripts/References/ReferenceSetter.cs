using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReferenceSetter : MonoBehaviour
{
    public enum ReferencesType
    {
        None,
        Camera,
        Player,
        InputManager,
        GameManager,
        FightManager,
        CardManager,
        AudioManager
    }

    [SerializeField] private AllReferences references;
    [SerializeField] private ReferencesType[] referencesTypes;

    private void Awake()
    {
        foreach (ReferencesType item in referencesTypes)
        {
            ConnectValues(item);
        }
    }

    public void ReconnectValues(ReferencesType item)
    {
        ConnectValues(item);
    }

    public void ReconnectAllValues()
    {
        foreach (ReferencesType item in referencesTypes)
        {
            ConnectValues(item);
        }
    }

    private void ConnectValues(ReferencesType item)
    {
        switch (item)
        {
            case ReferencesType.Player:
                references.player = gameObject;
                return;
            case ReferencesType.Camera:
                references.camera = GetComponent<Camera>();
                if(references.camera == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            case ReferencesType.InputManager:
                references.inputManager = GetComponent<InputManager>();
                if (references.inputManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            case ReferencesType.GameManager:
                references.gameManager = GetComponent<GameManager>();
                if (references.gameManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            case ReferencesType.FightManager:
                references.fightManager = GetComponent<FightManager>();
                if (references.fightManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            case ReferencesType.CardManager:
                references.cardManager = GetComponent<CardManager>();
                if (references.cardManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            case ReferencesType.AudioManager:
                references.audioManager = GetComponent<AudioManager>();
                if (references.audioManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas récupérer: {item}");
                }
                return;
            default:
                return;
        }
    }
}