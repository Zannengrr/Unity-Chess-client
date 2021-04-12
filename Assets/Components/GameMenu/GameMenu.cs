using Assets.Messaging;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour, IPanel
{
    public Button CreateGame;
    public Button JoinGame;
    public Button WatchHistory;
    public Toggle PublicGameToggle;

    private List<Invitation> invitations = new List<Invitation>();
    public GameObject InvitationPrefab;

    private void Awake()
    {
        CreateGame.onClick.AddListener(TryCreateGameSession);
        JoinGame.onClick.AddListener(StartMatchamking);
        WatchHistory.onClick.AddListener(OpenWatchHistory);
        MessagingSystem.Subscribe(MessageType.OnInvitationReceived, GenerateInvitationObject);
        MessagingSystem.Subscribe(MessageType.OpenGameMenu, OpenGameMenu);
        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void TryCreateGameSession()
    {
        GameSessionData data = new GameSessionData(GameManager.UserInfo.Username, PublicGameToggle.isOn);
        Network.SendDataToServer("CreateGame", data);
        //hide screen
        MessagingSystem.DispatchMessage(MessageType.OnOpenGamePanel, null);
        Deactivate();
    }

    private void StartMatchamking()
    {
        Network.SendDataToServer("StartMatchmaking", null);
        //Screen for matchmaking
        Deactivate();
    }

    private void OpenWatchHistory()
    {
        MessagingSystem.DispatchMessage(MessageType.OpenHistoryMenu, null);
        Deactivate();
    }

    private void GenerateInvitationObject(object _data)
    {
        string gameId = _data.ToString();
        Invitation clone = Instantiate(InvitationPrefab, transform).GetComponent<Invitation>();
        clone.Initialize(gameId, this);
        invitations.Add(clone);
    }

    private void OpenGameMenu(object _data)
    {
        Activate();
    }

    public void OnInvitationActionTriggered()
    {
        foreach (var item in invitations)
        {
            Destroy(item.gameObject);
        }
        Deactivate();
    }

    private void OnDestroy()
    {
        MessagingSystem.Unsubscribe(MessageType.OnInvitationReceived, GenerateInvitationObject);
        MessagingSystem.Unsubscribe(MessageType.OpenGameMenu, OpenGameMenu);
    }

}
