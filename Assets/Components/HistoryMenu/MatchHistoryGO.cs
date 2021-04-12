using Assets.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchHistoryGO : MonoBehaviour, IPointerClickHandler
{
    private GameHistory GameHistory;
    public Image Background;
    public TextMeshProUGUI Gameid;
    public TextMeshProUGUI MoveNumber;

    public void Initialize(GameHistory gameHistory)
    {
        GameHistory = gameHistory;
        SetText(Gameid, $"GameID: {GameHistory.GameId}");
        SetText(MoveNumber, $"Moves: {GameHistory.MoveHistory.Length}");
    }

    private void SetText(TextMeshProUGUI text, string message)
    {
        text.text = message;
    }

    public void Select()
    {
        Background.color = new Color32(248, 169, 72, 255);
    }

    public void Deselect()
    {
        Background.color = new Color32(255, 255, 255, 255);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
        MessagingSystem.DispatchMessage(MessageType.OnGameHistorySelect, this);
    }

    public GameHistory GetMatchHistory()
    {
        return GameHistory;
    }
}
