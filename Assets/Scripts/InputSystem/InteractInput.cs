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
    [SerializeField] float secondsForToolTip = 2.5f;
    [SerializeField] float value = 1.8f;
    Coroutine _dragCoroutine = null;
    GameObject _getObject;

    private void Start()
    {
        _inputs.pressedEvent += Interact;
        _inputs.unPressedEvent += Drop;
        refs.fightManager.OnTurnEnd += CanceledDrop;
        _inputs.cancel += CanceledDrop;
    }

    private void OnDestroy()
    {
        _inputs.pressedEvent -= Interact;
        _inputs.unPressedEvent -= Drop;
        refs.fightManager.OnTurnEnd -= CanceledDrop;
        _inputs.cancel -= CanceledDrop;

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
        float time = Time.time;
        float waitTime = time + secondsForToolTip;
        bool isToolTip = false;
        while (true)
        {

            if(Input.touches.Length > 0)
            {
                if(!isToolTip)
                {
                    if (time < waitTime)
                    {
                        time += Time.deltaTime;
                    }
                    else
                    {
                        isToolTip = true;
                        Debug.Log(((Vector2)(Camera.main.ScreenToWorldPoint(Input.touches[0].rawPosition) - Camera.main.ScreenToWorldPoint(Input.touches[0].position))).magnitude);
                        if (((Vector2)(Camera.main.ScreenToWorldPoint(Input.touches[0].position) - Camera.main.ScreenToWorldPoint(Input.touches[0].rawPosition))).magnitude <= value)
                        {
                            CanceledDrop();
                            ToolTip();
                            
                        }
                    }
                }

                if(_getObject != null)
                {
                    _getObject.transform.position = Camera.main.ScreenToWorldPoint(Input.touches[0].position) + Vector3.forward * 10f;
                }
            }
            yield return null;
        }
    }

    void CanceledDrop()
    {
        if(_dragCoroutine != null)
        {
            if (_getObject != null) // Check if we got object to interact with
            {
                if(_getObject.GetComponent<IInteractable>() != null)
                {
                    _getObject.GetComponent<IInteractable>().Cancel();//Interact with object
                }
                _getObject = null;
            }
            StopCoroutine(_dragCoroutine);
            _dragCoroutine = null;
        }
    }

    void ToolTip()
    {
        Debug.Log("ToolTip");
    }
}
