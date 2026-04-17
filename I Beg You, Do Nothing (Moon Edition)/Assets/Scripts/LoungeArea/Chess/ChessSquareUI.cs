using UnityEngine;
using UnityEngine.UI;

public class ChessSquareUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image pieceImage;
    [SerializeField] private Button button;

    private ChessPuzzleUI board;
    private int index;

    public void Init(ChessPuzzleUI boardRef, int squareIndex)
    {
        board = boardRef;
        index = squareIndex;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => board.OnSquareClicked(index));
    }

    public void SetBackground(Color color)
    {
        color.a = 1f;
        background.enabled = true;
        background.color = color;
    }

    public void SetPiece(Sprite sprite)
    {
        pieceImage.sprite = sprite;
        pieceImage.enabled = sprite != null;
    }
}