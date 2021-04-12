using Assets.Messaging;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ChessBoard ChessBoard;
    public ChessPieceManager ChessPieceManager;

    public static UserInfo UserInfo; //change somehow to be better
    public static bool IsMyTurn = false;
    public static string GameId;

    public MessageComponent TurnInfo;
    public MessageComponent ChessPiecesInfo;
    public InfoBoxComponent GameEnd;

    private void Awake()
    {
        MessagingSystem.Subscribe(MessageType.InitializeGame, SetupGame);
        MessagingSystem.Subscribe(MessageType.CellClicked, OnCellClicked);
        MessagingSystem.Subscribe(MessageType.MoveSuccess, OnMoveSuccess);
        MessagingSystem.Subscribe(MessageType.MoveFailure, OnMoveFailure);
        MessagingSystem.Subscribe(MessageType.GameEnd, OnGameEnd);
        MessagingSystem.Subscribe(MessageType.OpenGameMenu, OnOpenGameMenu);
        MessagingSystem.Subscribe(MessageType.StartWatchMatch, StartReplay);
    }

    private void SetupGame(object _data)
    {
        var gameData = JsonConvert.DeserializeObject<GameData>(_data.ToString());
        GameId = gameData.GameId;
        IsMyTurn = IsItMyTurn(gameData.StartingPlayer);

        StartGame(gameData.Board);

        TurnInfo.Activate();
        ChessPiecesInfo.Activate();
        TurnInfoUpdate();
        SetChessPieceInfo();
    }

    private void StartGame(BoardData boardData)
    {
        ChessBoard.gameObject.SetActive(true);
        ChessBoard.Create();
        ChessPieceManager.Initialize(boardData, ChessBoard);
    }

    private void TurnInfoUpdate()
    {
        if (IsMyTurn)
        {
            TurnInfo.SetMessage("Your turn");
            return;
        }

        TurnInfo.SetMessage("Opponents turn");
    }

    private void SetChessPieceInfo()
    {
        if(IsMyTurn)
        {
            ChessPiecesInfo.SetMessage("You are WHITE");
            return;
        }

        ChessPiecesInfo.SetMessage("You are BLACK");
    }

    private bool IsItMyTurn(string _id)
    {
        return UserInfo.Id == _id;
    }

    private void OnCellClicked(object _data)
    {
        BasePieceGO selectedChessPiece = ChessPieceManager.GetSelectedPiece();
        if(selectedChessPiece == null)
        {
            return;
        }

        ChessCell chessCell = _data as ChessCell;
        selectedChessPiece.TryMove(chessCell.x, chessCell.y, GameId);
    }

    private void OnMoveSuccess(object _data)
    {
        PieceMoveData[] pieceMoveData = JsonConvert.DeserializeObject<PieceMoveData[]>(_data.ToString());
        for (int i = 0; i < pieceMoveData.Length; i++)
        {
            ChessCell targetCell = ChessBoard.GetChessCellByPosition(pieceMoveData[i].X, pieceMoveData[i].Y);
            ChessCell originalCell = ChessBoard.GetChessCellByChessPieceId(pieceMoveData[i].Id);
            ChessPieceManager.MovePiece(pieceMoveData[i].Id, targetCell, originalCell);
        }
        IsMyTurn = !IsMyTurn;
        TurnInfoUpdate();
    }

    private void OnGameEnd(object _data)
    {
        CallbackData callbackData = JsonConvert.DeserializeObject<CallbackData>(_data.ToString());
        GameEnd.SetMessageComponent(callbackData.Message);
        GameEnd.SetActionMessageType(MessageType.OpenGameMenu);
        GameEnd.Activate();
    }

    private void OnOpenGameMenu(object _data)
    {
        EndGameActions();
    }

    private void EndGameActions()
    {
        TurnInfo.Deactivate();
        ChessPiecesInfo.Deactivate();
        ChessBoard.Cleanup();
        ChessPieceManager.Cleanup();
        ChessBoard.gameObject.SetActive(false);
    }

    private void OnMoveFailure(object _data)
    {
        CallbackData callbackData = JsonConvert.DeserializeObject<CallbackData>(_data.ToString());
        Debug.Log("-------------INVALID MOVE-------------");
    }

    private void StartReplay(object _data)
    {
        GameHistory gameHistory = _data as GameHistory;
        Debug.Log("Replay Goes here");
        //TODO start replay of GameHistory somehow
        StartGame(gameHistory.Board);
        StartCoroutine(MakeMove(ChessPieceManager, ChessBoard, gameHistory));
    }

    private IEnumerator MakeMove(ChessPieceManager _Pmanager, ChessBoard _Bmanager, GameHistory _gameHistory)
    {
        foreach (PieceMoveData move in _gameHistory.MoveHistory)
        {
            yield return new WaitForSeconds(2);
            ChessCell targetCell = _Bmanager.GetChessCellByPosition(move.X, move.Y);
            ChessCell originalCell = _Bmanager.GetChessCellByChessPieceId(move.Id);
            _Pmanager.MovePiece(move.Id, targetCell, originalCell);
        }

        yield return new WaitForSeconds(2);
        GameEnd.SetMessageComponent("Replay over");
        GameEnd.SetActionMessageType(MessageType.OpenGameMenu);
        GameEnd.Activate();
    }

    private void OnDestroy()
    {
        MessagingSystem.Unsubscribe(MessageType.InitializeGame, SetupGame);
        MessagingSystem.Unsubscribe(MessageType.CellClicked, OnCellClicked);
        MessagingSystem.Unsubscribe(MessageType.MoveSuccess, OnMoveSuccess);
        MessagingSystem.Unsubscribe(MessageType.MoveFailure, OnMoveFailure);
        MessagingSystem.Unsubscribe(MessageType.GameEnd, OnGameEnd);
        MessagingSystem.Unsubscribe(MessageType.OpenGameMenu, OnOpenGameMenu);
        MessagingSystem.Unsubscribe(MessageType.StartWatchMatch, StartReplay);
    }
}
