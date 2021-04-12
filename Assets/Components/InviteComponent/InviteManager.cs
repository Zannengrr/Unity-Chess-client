using Assets.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InviteManager : MonoBehaviour, IPanel
{
    public Button Invite;
    public TMP_InputField Username;
    public TextMeshProUGUI WarningMessage;

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        Invite.onClick.AddListener(InviteAction);
        WarningMessage.text = "";
        MessagingSystem.Subscribe(MessageType.OnOpenGamePanel, ActivatePanel);
        MessagingSystem.Subscribe(MessageType.InitializeGame, OnStartGame);
        Deactivate();
    }

    private void InviteAction()
    {
        Network.SendDataToServer("InviteUserToGame",Username.text);
    }

    public void SetWarningMessage(string _text)
    {
        WarningMessage.text = _text;
        WarningMessage.gameObject.SetActive(!string.IsNullOrEmpty(WarningMessage.text));
    }

    private void ActivatePanel(object _data)
    {
        Activate();
    }

    private void OnStartGame(object _data)
    {
        Deactivate();
    }

    private void OnDestroy()
    {
        MessagingSystem.Subscribe(MessageType.OnOpenGamePanel, ActivatePanel);
        MessagingSystem.Subscribe(MessageType.InitializeGame, OnStartGame);
    }

}
