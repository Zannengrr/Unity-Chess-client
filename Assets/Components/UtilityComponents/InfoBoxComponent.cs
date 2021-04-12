using Assets.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InfoBoxComponent : MonoBehaviour
{
    public MessageComponent messageComponent;
    public Button buttonComponent;
    public MessageType MessageType;

    private void Awake()
    {
        buttonComponent.onClick.AddListener(ButtonAction);
        Deactivate();
    }

    private void ButtonAction()
    {
        MessagingSystem.DispatchMessage(MessageType, null);
        Deactivate();
    }

    public void SetMessageComponent(string _message)
    {
        messageComponent.SetMessage(_message);
    }

    public void SetActionMessageType(MessageType _messageType)
    {
        MessageType = _messageType;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        messageComponent.Activate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
