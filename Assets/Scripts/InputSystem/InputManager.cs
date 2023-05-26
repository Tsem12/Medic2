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

    IEnumerator GetObject()
    {
        if(CastObject())
        {
            if (_getObj.CompareTag("Grabbable"))
            {
                while (true)
                {
                    if(Input.touches.Length > 0)
                    {
                        _getObj.transform.position = Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f;
                    }
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
        Debug.Log("Press");
    }

    void UnPress(InputAction.CallbackContext ctx)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            if(_getObj.GetComponent<Card>() != null && !_getObj.GetComponent<Card>().ApplyEffect())
            {
                _getObj.GetComponent<Card>().PosReset();
            }
            _getObj = null;
            _coroutine = null;
        }
        Debug.Log("Unpress");
    }

    bool CastObject()
    {
        Collider2D col = null;
        if (Input.touches.Length > 0)
        {
            col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f, 0.2f);
        }
        if (col != null)
        {
            _getObj = col.gameObject;
            return true;
        }
        return false;
    }


}
