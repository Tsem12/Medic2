using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
    public void Cancel();
}


public class InteractInput : MonoBehaviour
{
    [SerializeField] InputHandlerObject _inputs;
    [SerializeField] AllReferences refs;
    Coroutine _dragCoroutine = null;
    GameObject _getObject;

    private void Start()
    {
        _inputs.pressedEvent += Interact;
        _inputs.unPressedEvent += Drop;
        refs.fightManager.OnTurnEnd += CanceledDrop;
        _inputs.cancel += CanceledDrop;
    }
    void Interact()
    {
        if(Input.touchCount > 0)
        {
            Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.touches[0].position), 0.2f);
            if (col != null)
            {
                _getObject = col.gameObject;
                if (_getObject.CompareTag("Grabbable"))//Drag if Grabbable
                {
                    _dragCoroutine = StartCoroutine(Drag());
                }
            }
        }
    }

    void Drop()
    {
        if(_getObject != null) // Check if we got object to interact with
        {
            if(_getObject.GetComponent<IInteractable>() != null)
            {
                _getObject.GetComponent<IInteractable>().Interact();//Interact with object
            }
            if (_dragCoroutine != null)
            {
                StopCoroutine(_dragCoroutine);
                _dragCoroutine = null;
            }
            _getObject = null;
        }
    }

    IEnumerator Drag()
    {
        while (true)
        {
            if(Input.touches.Length > 0)
            {
                _getObject.transform.position = Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f;
            }
            yield return null;
        }
    }

    void CanceledDrop()
    {
        //Debug.Log("TryCancel");
        if(_dragCoroutine != null)
        {
            if (_getObject != null) // Check if we got object to interact with
            {
                if(_getObject.GetComponent<IInteractable>() != null)
                {
                    _getObject.GetComponent<IInteractable>().Cancel();//Interact with object
                }
                StopCoroutine(_dragCoroutine);
                _dragCoroutine = null;
                _getObject = null;
            }
        }
    }
}
