using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    //CellPrefab
    public GameObject CellPrefab;

    //public BoardData Board = new BoardData();
    public ChessCell[,] AllCells = new ChessCell[8, 8];

    public void Create()
    {
        #region Create
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                GameObject cell = Instantiate(CellPrefab, transform);

                RectTransform rectTransform = cell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                AllCells[x, y] = cell.GetComponent<ChessCell>();
                AllCells[x, y].Initialize(x, y, this);
            }
        }
        #endregion

        #region Color
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 8; y++)
            {
                // Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                // Color
                AllCells[finalX, y].SetColor(CellColor.Light);
            }
        }
        #endregion
    }

    public ChessCell GetChessCellByPosition(int _posX, int _posY)
    {
        return AllCells[_posX, _posY];
    }

    public ChessCell GetChessCellByChessPieceId(int _id)
    {
        foreach (var cell in AllCells)
        {
            if(cell.ChessPiece != null && cell.ChessPiece.ChessPieceData.ChessPieceId == _id)
            {
                return cell;
            }
        }

        return null;
    }

    public void Cleanup()
    {
        AllCells = AllCells = new ChessCell[8, 8];
    }
}
