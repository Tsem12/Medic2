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
        FightManager
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
                    Debug.LogError($"{gameObject.name} n'a pas r�cup�rer: {item}");
                }
                return;
            case ReferencesType.InputManager:
                references.inputManager = GetComponent<InputManager>();
                if (references.inputManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas r�cup�rer: {item}");
                }
                return;
            case ReferencesType.GameManager:
                references.gameManager = GetComponent<GameManager>();
                if (references.gameManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas r�cup�rer: {item}");
                }
                return;
            case ReferencesType.FightManager:
                references.fightManager = GetComponent<FightManager>();
                if (references.fightManager == null)
                {
                    Debug.LogError($"{gameObject.name} n'a pas r�cup�rer: {item}");
                }
                return;
            default:
                return;
        }
    }
}