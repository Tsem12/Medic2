using Assets.SimpleLocalization;
using DG.Tweening;
using NaughtyAttributes;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBehaviour : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private LocalizedText _textLocalisation;
    [SerializeField] private Image _icon;
    [SerializeField] private RectTransform _gfx;
    private Character _character;
    Vector3 initScale;
    private Coroutine _messageCoroutine;
    private Tween _appearTween;
    private Tween _disappearTween;

    public Sprite test;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        initScale = Vector3.zero;
    }

    private MessageBody GetMessage(Message.MessageType messageType, CharacterObjets chara, CharacterObjets.BossType bossType)
    {
        List<MessageBody> list = new List<MessageBody>();
        foreach(Message m in chara.messages)
        {
            if(m.messageType == messageType)
            {
                foreach(MessageBody mb in m.messages)
                {
                    if(mb.bossType == bossType || mb.bossType == CharacterObjets.BossType.None)
                    {
                        list.Add(mb);
                    }
                }
                return list[Random.Range(0, list.Count)];
            }
        }
        Debug.Log($"Y'a pas de {messageType} dans la liste");
        return null;
    }

    public void DisplayMessage(Message.MessageType messageType, CharacterObjets chara, CharacterObjets.BossType bossType)
    {
        if( _messageCoroutine == null)
        {
            MessageBody message = GetMessage(messageType, chara, bossType);
            if(message != null)
            {
                _messageCoroutine = StartCoroutine(MessageRoutine(message));
            }
        }
        else
        {
            StopCoroutine(_messageCoroutine);
            if( _disappearTween == null || !_disappearTween.IsPlaying())
            {
                Disapear();
            }
            MessageBody message = GetMessage(messageType, chara, bossType);
            if (message != null)
            {
                _messageCoroutine = StartCoroutine(MessageRoutine(message));
            }
        }
    }

    [Button]
    public void Appear()
    {
        _gfx.gameObject.SetActive(true);
        _gfx.localScale = Vector3.zero;
        _appearTween = _gfx.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutCubic);
    }

    [Button]
    public void Disapear()
    {
        _gfx.localScale = Vector3.one;
        _disappearTween = _gfx.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() => _gfx.gameObject.SetActive(false));

    }

    private IEnumerator MessageRoutine(MessageBody message)
    {
        if(_disappearTween != null)
        {
            yield return _disappearTween.WaitForCompletion();
        }

        _textLocalisation.LocalizationKey = message.localizationKey;
        _textLocalisation.Localize();
        _icon.sprite = message.expression == MessageBody.Expression.Happy ? _character.CharacterObj.happyFace : _character.CharacterObj.angryFace;

        Appear();
        yield return new WaitForSeconds(2f);
        Disapear();
        _messageCoroutine = null;
    }
}
