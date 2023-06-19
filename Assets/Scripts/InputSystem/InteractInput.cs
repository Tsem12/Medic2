using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
    public void Cancel();
    
}

interface IToolTip
{
    public void ShowToolTip(ToolTip tooltip);
}

public class InteractInput : MonoBehaviour
{
    [SerializeField] InputHandlerObject _inputs;
    [SerializeField] AllReferences refs;
    [SerializeField] float secondsForToolTip = 1f;
    [SerializeField] float value = 0.7f;
    [SerializeField] ToolTip toolTipCanva;
    Coroutine _dragCoroutine = null;
    Coroutine _toolTipCoroutine = null;
    GameObject _getObject;
    bool wasTooltip = false;

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
                else if(_getObject.CompareTag("ToolTip"))
                {
                    _toolTipCoroutine = StartCoroutine(WaitForTooltip());
                }
            }
        }
    }

    void Drop()
    {
        if(_getObject != null) // Check if we got object to interact with
        {
            if(!wasTooltip)
            {
                if (_getObject.GetComponent<IInteractable>() != null)
                {
                    _getObject.GetComponent<IInteractable>().Interact();//Interact with object
                }
            }
            
            if (_dragCoroutine != null)
            {
                StopCoroutine(_dragCoroutine);
                _dragCoroutine = null;
            }

            _getObject = null;
        }

        if (_toolTipCoroutine != null)
        {
            StopCoroutine(_toolTipCoroutine);
            _toolTipCoroutine = null;
        }

        if(wasTooltip)
        {
            wasTooltip = false;
            toolTipCanva.gameObject.SetActive(false);
        }
    }

    IEnumerator Drag()
    {
        float time = Time.time;
        float waitTime = time + secondsForToolTip;
        bool stopToolTip = false;
        while (true)
        {
            if(Input.touches.Length > 0 && _getObject != null)
            {
                if(!stopToolTip)
                {
                    if (((Vector2)(Camera.main.ScreenToWorldPoint(Input.touches[0].position) - _getObject.transform.position)).magnitude <= value)
                    {
                        if (time < waitTime)
                        {
                            time += Time.deltaTime;
                        }
                        else
                        {
                            stopToolTip = true;
                            ToolTip();
                            CanceledDrop();
                        }
                    }
                    else
                    {
                        stopToolTip = true;
                    }
                }
                else
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
        wasTooltip = true;
        toolTipCanva.gameObject.SetActive(true);
        if(_getObject != null && _getObject.GetComponent<IToolTip>() != null)
        {
            _getObject.GetComponent<IToolTip>().ShowToolTip(toolTipCanva);
        }
    }

    IEnumerator WaitForTooltip()
    {
        wasTooltip = true;
        yield return new WaitForSeconds(secondsForToolTip);
        toolTipCanva.gameObject.SetActive(true);
        if (_getObject != null && _getObject.GetComponent<IToolTip>() != null)
        {
            _getObject.GetComponent<IToolTip>().ShowToolTip(toolTipCanva);
        }
    }
}
