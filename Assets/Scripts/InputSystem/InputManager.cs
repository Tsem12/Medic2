using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;
    [SerializeField] InputActionReference _touchPress;
    [SerializeField] InputHandlerObject inputHandlerObj;
    Coroutine _coroutine;
    GameObject _getObj;

    private void Start()
    {
        _refs.inputManager = this;
        Application.targetFrameRate = 300;
    }

    private void OnEnable()
    {
        _touchPress.action.performed += Press;
        _touchPress.action.canceled += UnPress;
    }

    private void OnDisable()
    {
        _touchPress.action.performed -= Press;
        _touchPress.action.canceled -= UnPress;
    }

    void Press(InputAction.CallbackContext ctx)
    {
        inputHandlerObj.PressEvent();
    }

    void UnPress(InputAction.CallbackContext ctx)
    {
        inputHandlerObj.UnPressEvent();
    }
}
