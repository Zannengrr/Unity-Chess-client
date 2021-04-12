using Assets.Messaging;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public LoginForm LoginPanel;
    public RegisterForm RegisterPanel;
    public GameMenu GameMenuPanel;

    public Button LoginButtonPanel;
    public Button RegisterButtonPanel;

    private void Awake()
    {
        LoginButtonPanel.onClick.AddListener(() => OpenPanel(LoginPanel));
        LoginButtonPanel.onClick.AddListener(DisableButtons);
        RegisterButtonPanel.onClick.AddListener(() => OpenPanel(RegisterPanel));
        RegisterButtonPanel.onClick.AddListener(DisableButtons);

        MessagingSystem.Subscribe(MessageType.LoginCallback, OnLoginCallback);
        MessagingSystem.Subscribe(MessageType.RegistrationCallback, RegistrationCallback);
        MessagingSystem.Subscribe(MessageType.OpenRegisterMenu, OpenRegisterMenuButtons);
    }

    private void OpenPanel(IPanel _panel)
    {
        _panel.Activate();
    }

    private void ClosePanel(IPanel _panel)
    {
        _panel.Deactivate();
    }

    private void DisableButtons()
    {
        LoginButtonPanel.gameObject.SetActive(false);
        RegisterButtonPanel.gameObject.SetActive(false);
    }

    private void EnableButtons()
    {
        LoginButtonPanel.gameObject.SetActive(true);
        RegisterButtonPanel.gameObject.SetActive(true);
    }

    private void OnLoginCallback(object _data)
    {
        var callbackData = JsonConvert.DeserializeObject<LoginCallbackData>(_data.ToString());
        if (callbackData.Data.Success)
        {
            ClosePanel(LoginPanel);
            OpenPanel(GameMenuPanel);
            //save username somewhere
            GameManager.UserInfo = callbackData.UserInfo;
        }
        else
        {
            LoginPanel.SetWarningMessage(callbackData.Data.Message);
        }
        LoginPanel.LoginButton.interactable = true;
    }

    private void RegistrationCallback(object _data)
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackData>(_data.ToString());
        if (callbackData.Success)
        {
            ClosePanel(RegisterPanel);
            OpenPanel(LoginPanel);
        }
        else
        {
            RegisterPanel.SetWarningMessage(callbackData.Message);
        }
        RegisterPanel.RegisterButton.interactable = true;
    }

    private void OpenRegisterMenuButtons(object _data)
    {
        EnableButtons();
    }

    private void OnDestroy()
    {
        MessagingSystem.Unsubscribe(MessageType.LoginCallback, OnLoginCallback);
        MessagingSystem.Unsubscribe(MessageType.RegistrationCallback, RegistrationCallback);
        MessagingSystem.Unsubscribe(MessageType.OpenRegisterMenu, OpenRegisterMenuButtons);
    }

}
