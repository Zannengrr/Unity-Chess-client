using Assets.Messaging;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryMenu : MonoBehaviour, IPanel
{
    private List<MatchHistoryGO> gameHistories = new List<MatchHistoryGO>();
    public Button WatchMatchHistory;
    public GameObject GameHistoryGO;
    public GameObject HistoryContentPlaceholder;
    private MatchHistoryGO selectedHistory = null;
    public Button BackButton;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        MessagingSystem.Subscribe(MessageType.GameHistoryReceived, OnGameHistoryReceived);
        MessagingSystem.Subscribe(MessageType.OnGameHistorySelect, OnGameHistorySelect);
        MessagingSystem.Subscribe(MessageType.OpenHistoryMenu, OpenHistoryMenu);
        WatchMatchHistory.onClick.AddListener(WatchButtonAction);
        BackButton.onClick.AddListener(BackButtonAction);
        LoadHistoryFromFile();
        Deactivate();
    }

    private void WatchButtonAction()
    {
        MessagingSystem.DispatchMessage(MessageType.StartWatchMatch, selectedHistory.GetMatchHistory());
        OnClose();
        Deactivate();
    }

    private void BackButtonAction()
    {
        MessagingSystem.DispatchMessage(MessageType.OpenGameMenu, null);
        OnClose();
        Deactivate();
    }

    private void OnClose()
    {
        if (selectedHistory != null)
        {
            selectedHistory.Deselect();
        }
        Deactivate();
    }

    private void OnGameHistoryReceived(object _data)
    {
        GameHistory gameHistory = JsonConvert.DeserializeObject<GameHistory>(_data.ToString());
        MatchHistoryGO matchHistoryGO = Instantiate(GameHistoryGO, HistoryContentPlaceholder.transform).GetComponent<MatchHistoryGO>();
        matchHistoryGO.Initialize(gameHistory);
        gameHistories.Add(matchHistoryGO);
    }

    private void OnGameHistorySelect(object _data)
    {
        if(selectedHistory != null)
        {
            selectedHistory.Deselect();
        }
        selectedHistory = _data as MatchHistoryGO;
    }

    private void OpenHistoryMenu(object _data)
    {
        Activate();
    }

    private void LoadHistoryFromFile()
    {
        //TODO
    }

    private void SaveHistoryToFile()
    {
        //TODO
    }

    private void OnDestroy()
    {
        MessagingSystem.Unsubscribe(MessageType.GameHistoryReceived, OnGameHistoryReceived);
        MessagingSystem.Unsubscribe(MessageType.OnGameHistorySelect, OnGameHistorySelect);
    }

    private void OnApplicationQuit()
    {
        SaveHistoryToFile();
    }
}
