using Assets.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellColor
{
    Light,
    Dark,
}


public class ChessCell : MonoBehaviour, IPointerClickHandler
{
    public Image Color;
    public Image OutlineColor;
    public BasePieceGO ChessPiece = null;
    public int x;
    public int y;

    private ChessBoard chessBoardRef;
    private Color32 darkColor= new Color32(202, 167, 132, 255);
    private Color32 lightColor = new Color32(230, 220, 186, 255);

    public void Awake()
    {
        Color.color = darkColor;
    }

    public void Initialize(int _x, int _y, ChessBoard _chessBoardRef)
    {
        x = _x;
        y = _y;
        chessBoardRef = _chessBoardRef;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.IsMyTurn)
        {
            MessagingSystem.DispatchMessage(MessageType.CellClicked, this);
        }
    }

    public void SetColor(CellColor color)
    {
        switch (color)
        {
            case CellColor.Light:
                Color.color = lightColor;
                break;
            case CellColor.Dark:
                Color.color = darkColor;
                break;
        }
    }
}
