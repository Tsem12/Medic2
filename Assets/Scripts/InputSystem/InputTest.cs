using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IGrabbable
{
    public IEnumerator Grab(Vector2 pos);
}
public class InputTest : MonoBehaviour
{
    [SerializeField] PlayerInput _inputaction;
    Coroutine _coroutine;

    // Start is called before the first frame update


    void OnEnable()
    {
        _inputaction.actions["TouchPress"].performed += Pressed;
        _inputaction.actions["TouchPress"].canceled += UnPresse;
    }

    void OnDisable()
    {
        _inputaction.actions["TouchPress"].performed -= Pressed;
        _inputaction.actions["TouchPress"].canceled -= UnPresse;
    }

    public void Pressed(InputAction.CallbackContext context)
    {
        Collider2D coll = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1f);
        IGrabbable grab = coll?.GetComponent<IGrabbable>();
        Debug.Log(grab);
        if(grab != null) 
        {
            StartCoroutine(grab.Grab(Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10f));
        }
        if (_coroutine == null)
        {
            //_coroutine = StartCoroutine();
        }
    }

    public void UnPresse(InputAction.CallbackContext context)
    {
        if(_coroutine != null ) 
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
