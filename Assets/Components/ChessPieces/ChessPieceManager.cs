using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessPieceManager : MonoBehaviour
{
    BasePieceGO selectedPiece = null;

    public ChessPiecesSO chessPiecesSO;

    public List<BasePieceGO> ActiveChessPieces = new List<BasePieceGO>();

    /// <summary>
    /// Uses Server Board data to create Chess Pieces on the Client board
    /// </summary>
    /// <param name="serverBoard"></param>
    /// <param name="clientBoard"></param>
    public void Initialize(BoardData serverBoard, ChessBoard clientBoard)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (serverBoard.AllCells[i, j].ChessPiece != null)
                {
                    CellData serverCell = serverBoard.AllCells[i, j];
                    ChessCell chessCell = clientBoard.AllCells[i, j];

                    BasePieceGO basePieceGO = Instantiate(chessPiecesSO.ChessPieces[serverCell.ChessPiece.Type], chessCell.transform).GetComponent<BasePieceGO>();
                    basePieceGO.Initialize(serverCell.ChessPiece, this);

                    chessCell.ChessPiece = basePieceGO;
                    ActiveChessPieces.Add(basePieceGO);
                }
            }
        }
        Debug.Log("ChessPieces Initialized");
    }

    public void SelectPiece(BasePieceGO basePiece)
    {
        selectedPiece = basePiece;
        foreach (var piece in ActiveChessPieces)
        {
            piece.ChessSprite.raycastTarget = false;
        }
        selectedPiece.ChessSprite.raycastTarget = true;
    }

    public void MovePiece(int _id, ChessCell _targetCell, ChessCell _originalCell)
    {
        BasePieceGO basePieceGO = GetChessPieceById(_id);
        basePieceGO.Move(_targetCell, _originalCell);
    }

    public BasePieceGO GetChessPieceById(int _id)
    {
        return ActiveChessPieces.FirstOrDefault(chesspiece => chesspiece.ChessPieceData.ChessPieceId == _id);
    }

    public void DeselectPiece()
    {
        foreach (var piece in ActiveChessPieces)
        {
            piece.ChessSprite.raycastTarget = true;
        }
        selectedPiece = null;
    }

    public BasePieceGO GetSelectedPiece()
    {
        return selectedPiece;
    }

    public void RemoveChessPiece(BasePieceGO basePieceGO)
    {
        ActiveChessPieces.Remove(basePieceGO);
        Destroy(basePieceGO.gameObject);
    }

    public void Cleanup()
    {
        ActiveChessPieces = new List<BasePieceGO>();
        selectedPiece = null;
    }
}
