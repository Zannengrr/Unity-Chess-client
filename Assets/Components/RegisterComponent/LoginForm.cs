using Assets.Messaging;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour, IPanel
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TextMeshProUGUI WarningMessage;

    public Button LoginButton;
    public Button BackButton;

    private void Awake()
    {
        LoginButton.onClick.AddListener(LoginAction);
        BackButton.onClick.AddListener(BackAction);
        Deactivate();
    }

    private void OnEnable()
    {
        LoginButton.interactable = true;
    }

    private void LoginAction()
    {
        LoginData data = new LoginData(Username.text, Password.text);
        Network.SendDataToServer("Login", data);
        LoginButton.interactable = false;
    }
    private void BackAction()
    {
        MessagingSystem.DispatchMessage(MessageType.OpenRegisterMenu, null);
        Deactivate();
    }

    public void SetWarningMessage(string text)
    {
        WarningMessage.text = text;
        WarningMessage.gameObject.SetActive(!string.IsNullOrEmpty(WarningMessage.text));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
