using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private ChessPuzzleUI chessPuzzleUI;
    [SerializeField] private GameObject rewardObject;

    private bool rewardSpawned;         // true if UV lamp spawned already
    private int lastPuzzleIndex = -1;

    // title, fen, solution lines
    private (string title, string fen, string solutionLinesText)[] puzzles;

    private void Awake()
    {
        puzzles = new (string title, string fen, string solutionLinesText)[]
        {
            ("White to play for winning", "8/P1R3p1/r4p2/1b2p3/1Kp5/4kP2/4P2P/8 w - - 0 1",@"b4b5 a6a1 b5c4 e3e2 c4b3 e2f3 c7c3 f3f2 c3c2 f2g1 c2a2"),
            ("Just resign", "5rk1/3b1pp1/p3p2p/2R5/2p1N3/4QPP1/P3PK1P/3q4 w - - 0 1",@"e4f6 g7f6 c5h5 d7c6 e3h6 d1d4 f2g2 c6f3 e2f3 d4b2 g2h3 b2c2 h6h8
                                                                                        e4f6 g8h8 c5h5 d1b1 h5h6 g7h6 e3h6 b1h7 h6h7"),
            ("Every failure is a step to success", "6k1/pp2rpp1/2br3p/2p5/6q1/1P3N2/PBP1QPPP/R4RK1 w - - 0 1",@"e2c4 g4f3 g2f3 c6f3 c4f4 d6g6 f4g3
                                                                                                                e2e7 g4f3 e7g5 h6g5 g2f3")
        };

        if (rewardObject != null)
            rewardObject.SetActive(false);

        if (chessPuzzleUI != null)
            chessPuzzleUI.PuzzleSolved += HandlePuzzleSolved;
    }

    public void OpenPuzzle()
    {
        if (puzzles == null || puzzles.Length == 0)
        {
            Debug.LogWarning("No chess puzzles defined in BoardTrigger.");
            return;
        }

        int index = Random.Range(0, puzzles.Length);

        if (puzzles.Length > 1 && index == lastPuzzleIndex)
        {
            index = (index + 1) % puzzles.Length;
        }

        lastPuzzleIndex = index;
        var puzzle = puzzles[index];

        puzzlePanel.SetActive(true);
        chessPuzzleUI.LoadPuzzle(puzzle.fen, puzzle.solutionLinesText, puzzle.title);
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);
    }

    // give reward only the first time the puzzle is solved
    private void HandlePuzzleSolved()
    {
        if (rewardSpawned) 
            return;

        rewardSpawned = true;

        if (rewardObject != null)
            rewardObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (chessPuzzleUI != null)
            chessPuzzleUI.PuzzleSolved -= HandlePuzzleSolved;
    }
}