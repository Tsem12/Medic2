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


    void Enable()
    {
        _inputaction.actions["Test"].performed += c =>Pressed();
        _inputaction.actions["Test"].canceled += c => UnPresse();
    }

    void Disable()
    {
        _inputaction.actions["Test"].performed -= c => Pressed();
        _inputaction.actions["Test"].canceled -= c => UnPresse();
    }

    public void Pressed()
    {
        Debug.Log("Test");
        Collider2D coll = Physics2D.OverlapCircle(Input.mousePosition, 10f);
        IGrabbable grab = coll.gameObject?.GetComponent<IGrabbable>();
        if (grab != null && _coroutine == null)
        {
            _coroutine = StartCoroutine(grab.Grab(Input.mousePosition));
        }
    }

    public void UnPresse()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }



}
