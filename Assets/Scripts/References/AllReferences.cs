using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "AllReferencesObjects", menuName = "ScriptableObjects/AllReferences")]
public class AllReferences : ScriptableObject
{
    public Camera camera;
    public GameObject player;
    public InputManager inputManager;
    public GameManager gameManager;

}