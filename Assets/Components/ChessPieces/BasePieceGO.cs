using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ChessPieceType
{
    None = 0,
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum Side
{
    White = 1,
    Black,
}


public abstract class BasePieceGO : MonoBehaviour, IPointerClickHandler
{
    public bool IsFirstMove = true;
    public Sprite White;
    public Sprite Black;
    public Image ChessSprite;

    public ChessCell CurrentCell;
    public ChessCell TargetSquare;

    public ChessPieceData ChessPieceData;
    public List<ChessCell> HighlightedCells = new List<ChessCell>(); 

    public ChessPieceManager ChessPieceManagerRef;

    public void TryMove(int _posX, int _posY, string _gameId)
    {
        PieceMoveData data = new PieceMoveData(_posX, _posY, ChessPieceData.ChessPieceId, _gameId, ChessPieceData.PlayerId);
        Network.SendDataToServer("TryMove", data);
        ChessPieceManagerRef.DeselectPiece(); //maybe
    }

    //TODO expaned so the method accepts base ActionInterface or Class
    //Pass in REAL action
    public void Move(IBaseAction _baseAction)
    {
        //Action Execute should be called on the real action
        _baseAction.Execute(); //Should superseed Execute in the REAL class and execute that one
    }

    public void Move(ChessCell _targetCell, ChessCell _originalCell)
    {
        if(_targetCell.ChessPiece != null)
        {
            _targetCell.ChessPiece.Kill();
        }
        _targetCell.ChessPiece = this;
        _originalCell.ChessPiece = null;
        transform.SetParent(_targetCell.transform);
        transform.localPosition = Vector3.zero;
    }

    public void Kill()
    {
        ChessPieceManagerRef.RemoveChessPiece(this);
    }

    public void Initialize(ChessPieceData _chessPieceData, ChessPieceManager _chessPieceManager)
    {
        ChessPieceData = _chessPieceData;
        ChessPieceManagerRef = _chessPieceManager;
        SetSprite();
    }

    private void SetSprite()
    {
        switch (ChessPieceData.Color)
        {
            case Side.White:
                ChessSprite.sprite = White;
                break;
            case Side.Black:
                ChessSprite.sprite = Black;
                break;
        }
    }

    public  void OnPointerClick(PointerEventData _eventData)
    {
        if(!GameManager.IsMyTurn)
        {
            return;
        }

        BasePieceGO basePieceGO = ChessPieceManagerRef.GetSelectedPiece();
        if (basePieceGO!= null && basePieceGO.ChessPieceData.ChessPieceId == ChessPieceData.ChessPieceId)
        {
            ChessPieceManagerRef.DeselectPiece();
            return;
        }

        if (ChessPieceData.PlayerId == GameManager.UserInfo.Id)
        {
            Debug.Log($"OnPointerClicked event from {GetType()}");
            ChessPieceManagerRef.SelectPiece(this);
        }
    }
}
