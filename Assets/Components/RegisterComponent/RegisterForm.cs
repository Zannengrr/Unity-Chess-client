using Assets.Messaging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterForm : MonoBehaviour, IPanel
{
    public TMP_InputField Email;
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TextMeshProUGUI WarningMessage;

    public Button RegisterButton;
    public Button BackButton;

    public int MinUsernameLength = 4;
    public int MinPasswordLength = 4;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public const string MatchEmailPattern2 = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`{}|~\\w])*)(?<=[0-9a-z])@))(?([)([(\\d{1,3}.){3}\\d{1,3}])|(([0-9a-z][-0-9a-z]*[0-9a-z]*.)+[a-z0-9][-a-z0-9]{0,22}[a-z0-9]))$";

    public const string MatchEmailPattern3 = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    private void Awake()
    {
        RegisterButton.onClick.AddListener(RegisterButtonAction);
        BackButton.onClick.AddListener(BackAction);
        Deactivate();
    }

    private void OnEnable()
    {
        RegisterButton.interactable = true;
    }

    private void RegisterButtonAction()
    {
        VerifyUserInputs();

        WarningMessage.gameObject.SetActive(!string.IsNullOrEmpty(WarningMessage.text));
        if(!WarningMessage.IsActive())
        {
            SendToServer();
            RegisterButton.interactable = false;
        }
    }

    private bool VerifyUserInputs()
    {
        if(string.IsNullOrEmpty(Email.text) || string.IsNullOrEmpty(Username.text) || string.IsNullOrEmpty(Password.text))
        {
            WarningMessage.text = "Some information is missing, check all the fields";
            return false;
        }

        if(!ValidateEmail(Email.text))
        {
            WarningMessage.text = "Invalid email";
            return false;
        }

        if(!CheckCharacterLength(Username.text, MinUsernameLength))
        {
            WarningMessage.text = "Username too short";
            return false;
        }

        if (!CheckCharacterLength(Password.text, MinPasswordLength))
        {
            WarningMessage.text = "Password too short";
            return false;
        }

        WarningMessage.text = "";
        return true;
    }

    private void BackAction()
    {
        MessagingSystem.DispatchMessage(MessageType.OpenRegisterMenu, null);
        Deactivate();
    }

    private void SendToServer()
    {
        RegisterData data = new RegisterData(Email.text, Username.text, Password.text);
        Network.SendDataToServer("Register", data);
    }

    public void SetWarningMessage(string text)
    {
        WarningMessage.text = text;
        WarningMessage.gameObject.SetActive(!string.IsNullOrEmpty(WarningMessage.text));
    }

    private bool CheckCharacterLength(string _input, int _length)
    {
        return _input.Length > _length;
    }

    private bool ValidateEmail(string _email)
    {
        return Regex.IsMatch(_email, MatchEmailPattern);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
