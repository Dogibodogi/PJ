using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChessPuzzleUI : MonoBehaviour
{
    public event System.Action PuzzleSolved;

    [Serializable]
    public class PieceSprites
    {
        public Sprite whiteKing;
        public Sprite whiteQueen;
        public Sprite whiteRook;
        public Sprite whiteBishop;
        public Sprite whiteKnight;
        public Sprite whitePawn;

        public Sprite blackKing;
        public Sprite blackQueen;
        public Sprite blackRook;
        public Sprite blackBishop;
        public Sprite blackKnight;
        public Sprite blackPawn;

        public Sprite GetSprite(char piece)
        {
            switch (piece)
            {
                case 'K': return whiteKing;
                case 'Q': return whiteQueen;
                case 'R': return whiteRook;
                case 'B': return whiteBishop;
                case 'N': return whiteKnight;
                case 'P': return whitePawn;
                case 'k': return blackKing;
                case 'q': return blackQueen;
                case 'r': return blackRook;
                case 'b': return blackBishop;
                case 'n': return blackKnight;
                case 'p': return blackPawn;
                default: return null;
            }
        }
    }

    [Header("Board")]
    [SerializeField] private RectTransform boardRoot;
    [SerializeField] private ChessSquareUI squarePrefab;
    [SerializeField] private PieceSprites pieceSprites;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text feedbackText;

    private ChessSquareUI[] squares = new ChessSquareUI[64];
    private char[] board = new char[64];

    private bool whiteToMove;
    private int selectedSquare = -1;

    private List<string[]> allBranches = new();         // for multiple solutions
    private List<string[]> activeBranches = new();
    private int plyIndex;
    private bool puzzleEnded;
    private bool inputLocked;

    private string startFen;
    private string puzzleTitle;
    private string solutionLinesText;

    private readonly Color lightColor = new Color(0.93f, 0.86f, 0.72f);
    private readonly Color darkColor = new Color(0.45f, 0.31f, 0.20f);
    private readonly Color selectedColor = new Color(0.90f, 0.80f, 0.30f);


    private bool BranchesFinished()
    {
        return activeBranches.Count > 0 && activeBranches.All(line => plyIndex >= line.Length);
    }

    private void ApplyUciMove(string uci)
    {
        int from = CoordToIndex(uci.Substring(0, 2));
        int to = CoordToIndex(uci.Substring(2, 2));

        char movingPiece = board[from];
        board[from] = '.';

        if (uci.Length >= 5)
        {
            char promotion = uci[4];
            movingPiece = whiteToMove ? char.ToUpper(promotion) : char.ToLower(promotion);
        }

        board[to] = movingPiece;
        whiteToMove = !whiteToMove;
    }

    private int CoordToIndex(string coord)
    {
        int file = coord[0] - 'a';
        int rank = coord[1] - '0';
        int row = 8 - rank;
        return row * 8 + file;
    }

    private void WinPuzzle(string message)
    {
        puzzleEnded = true;
        inputLocked = true;
        selectedSquare = -1;
        feedbackText.text = message;
        RefreshBoard();

        PuzzleSolved?.Invoke();
    }

    private void LosePuzzle(string message)
    {
        puzzleEnded = true;
        inputLocked = true;
        selectedSquare = -1;
        feedbackText.text = message;
        RefreshBoard();
    }

    public void RestartPuzzle()
    {
        LoadPuzzle(startFen, solutionLinesText, puzzleTitle);
    }


    private IEnumerator PlayBlackMoveAfterDelay(float delaySeconds)
    {
        inputLocked = true;
        feedbackText.text = "Correct...";

        yield return new WaitForSeconds(delaySeconds);

        var possibleBlackMoves = activeBranches
            .Where(line => plyIndex < line.Length)
            .Select(line => line[plyIndex].ToLowerInvariant())
            .Distinct()
            .ToList();

        if (possibleBlackMoves.Count == 0)
        {
            WinPuzzle("Solved!");
            yield break;
        }

        // choose one black reply from the surviving branches
        string blackMove = possibleBlackMoves[0];

        ApplyUciMove(blackMove);
        RefreshBoard();

        activeBranches = activeBranches
            .Where(line => plyIndex < line.Length && line[plyIndex].Equals(blackMove, StringComparison.OrdinalIgnoreCase))
            .ToList();

        plyIndex++;

        if (BranchesFinished())
        {
            WinPuzzle("Solved!");
        }
        else
        {
            feedbackText.text = "Find White's next move.";
            inputLocked = false;
        }
    }

    // using collections like in java
    private List<string[]> ParseBranches(string raw)
    {
        return raw
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line
                .Split(new[] { ' ', ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.ToLowerInvariant())
                .ToArray())
            .Where(line => line.Length > 0)
            .ToList();
    }

    private void CreateBoardIfNeeded()
    {
        if (squares[0] != null) return;

        for (int i = 0; i < 64; i++)
        {
            ChessSquareUI square = Instantiate(squarePrefab, boardRoot);
            square.gameObject.SetActive(true);
            square.transform.localScale = Vector3.one;
            square.Init(this, i);
            squares[i] = square;
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(boardRoot);
    }

    public void LoadPuzzle(string fen, string rawSolutionLines, string title)
    {
        CreateBoardIfNeeded();

        startFen = fen;
        puzzleTitle = title;
        solutionLinesText = rawSolutionLines;

        allBranches = ParseBranches(rawSolutionLines);
        activeBranches = new List<string[]>(allBranches);

        plyIndex = 0;
        puzzleEnded = false;
        inputLocked = false;
        selectedSquare = -1;

        titleText.text = title;
        feedbackText.text = "";

        ParseFen(startFen);
        RefreshBoard();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(boardRoot);
    }

    public void OnSquareClicked(int index)
    {
        if (puzzleEnded || inputLocked) return;
        if (!whiteToMove) return;

        if (selectedSquare == -1)
        {
            char piece = board[index];
            if (piece == '.') return;
            if (!char.IsUpper(piece)) return; // player controls white only

            selectedSquare = index;
            RefreshBoard();
            return;
        }

        if (selectedSquare == index)
        {
            selectedSquare = -1;
            RefreshBoard();
            return;
        }

        char movingPiece = board[selectedSquare];
        char targetPiece = board[index];

        // clicking another white piece changes selection
        if (targetPiece != '.' && char.IsUpper(targetPiece) == char.IsUpper(movingPiece))
        {
            selectedSquare = index;
            RefreshBoard();
            return;
        }

        string playedMove = IndexToCoord(selectedSquare) + IndexToCoord(index);

        // apply White move first, even if it is bad
        MakeMove(selectedSquare, index);
        whiteToMove = false;
        selectedSquare = -1;
        RefreshBoard();

        var matchingBranches = activeBranches
            .Where(line => plyIndex < line.Length && line[plyIndex].Equals(playedMove, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingBranches.Count == 0)
        {
            LosePuzzle("Wrong or illegal move. Restart the puzzle.");
            return;
        }

        activeBranches = matchingBranches;
        plyIndex++;

        if (BranchesFinished())
        {
            WinPuzzle("Solved!");
            return;
        }

        StartCoroutine(PlayBlackMoveAfterDelay(0.5f));
    }

    private void MakeMove(int from, int to)
    {
        board[to] = board[from];
        board[from] = '.';
    }

    private void RefreshBoard()
    {
        for (int i = 0; i < 64; i++)
        {
            int row = i / 8;
            int col = i % 8;
            bool isLight = (row + col) % 2 == 0;

            Color baseColor = isLight ? lightColor : darkColor;
            if (i == selectedSquare) baseColor = selectedColor;

            squares[i].SetBackground(baseColor);
            squares[i].SetPiece(pieceSprites.GetSprite(board[i]));
        }
    }

    private void ParseFen(string fen)
    {
        Array.Fill(board, '.');

        string[] parts = fen.Split(' ');
        string boardPart = parts[0];
        whiteToMove = parts.Length > 1 && parts[1] == "w";

        int index = 0;
        foreach (char c in boardPart)
        {
            if (c == '/')
                continue;

            if (char.IsDigit(c))
            {
                int empty = c - '0';
                for (int i = 0; i < empty; i++)
                {
                    board[index++] = '.';
                }
            }
            else
            {
                board[index++] = c;
            }
        }
    }

    private string IndexToCoord(int index)
    {
        int file = index % 8;
        int rank = 8 - (index / 8);

        char fileChar = (char)('a' + file);
        return $"{fileChar}{rank}";
    }
}