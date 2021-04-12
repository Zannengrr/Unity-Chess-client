using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Invitation : MonoBehaviour
{
    public Button Accept;
    public Button Decline;
    public TextMeshProUGUI InformationText;

    private string gameInvitationID;
    private InvitationCallback invitationCallback = new InvitationCallback();
    private GameMenu gameMenuReference;

    private void Awake()
    {
        Accept.onClick.AddListener(AcceptAction);
        Decline.onClick.AddListener(DeclineAction);
    }

    public void Initialize(string _gameId, GameMenu _parent)
    {
        gameInvitationID = _gameId;
        gameMenuReference = _parent;
        InformationText.text = $"You have been invited to a game with ID: {gameInvitationID}";
    }

    private void AcceptAction() 
    {
        invitationCallback.Data.Success = true;
        invitationCallback.GameId = gameInvitationID;
        Network.SendDataToServer("OnInvitationCallback", invitationCallback);
        gameMenuReference.OnInvitationActionTriggered();
    }

    private void DeclineAction()
    {
        invitationCallback.Data.Success = false;
        Network.SendDataToServer("OnInvitationCallback", invitationCallback);
        gameMenuReference.OnInvitationActionTriggered();
    }
}
