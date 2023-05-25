using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private AllReferences _refs;
    [SerializeField] InputActionReference _touchPress;
    Coroutine _coroutine;
    GameObject _getObj;

    private void Start()
    {
        _refs.inputManager = this;
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

    IEnumerator GetObject()
    {
        if(Input.touches.Length > 0 && CastObject())
        {
            if (_getObj.CompareTag("Grabbable"))
            {
                //_getObj.Activate();
                while (true)
                {
                    _getObj.transform.position = Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f;
                    yield return null;
                }
            }
        }
    }

    void Press(InputAction.CallbackContext ctx)
    {
        if(_coroutine == null) 
        {
            _coroutine = StartCoroutine(GetObject());
        }
    }

    void UnPress(InputAction.CallbackContext ctx)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _getObj = null;
            _coroutine = null;
        }
    }

    bool CastObject()
    {
        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f, 1f);
        if (col != null)
        {
            _getObj = col.gameObject;
            return true;
        }
        return false;
    }


}
