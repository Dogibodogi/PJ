using System.Collections.Generic;
using UnityEngine;

public class WirePuzzleUI : MonoBehaviour
{
    public RectTransform lineContainer;
    public GameObject linePrefab;
    public GameObject popupOverlay;
    public DropScreen codeScreen;

    public GameObject wirePanelClickArea;

    public GameObject solvedLine1;
    public GameObject solvedLine2;
    public GameObject solvedLine3;

    private WirePointUI selectedLeftPoint;

    private Dictionary<int, bool> connectedWires = new Dictionary<int, bool>();
    private Dictionary<int, GameObject> createdLines = new Dictionary<int, GameObject>();


    private void ApplySolvedState()
    {
        if (solvedLine1 != null) solvedLine1.SetActive(true);
        if (solvedLine2 != null) solvedLine2.SetActive(true);
        if (solvedLine3 != null) solvedLine3.SetActive(true);

        if (wirePanelClickArea != null)
            wirePanelClickArea.SetActive(false);

        if (popupOverlay != null)
            popupOverlay.SetActive(false);

        if (codeScreen != null)
            codeScreen.Drop();
    }

    private void Start()
    {
        connectedWires[0] = false;
        connectedWires[1] = false;
        connectedWires[2] = false;

        if (PuzzleState.wirePuzzleSolved)
        {
            ApplySolvedState();
        }
    }

    public void StartWire(WirePointUI leftPoint)
    {
        if (!leftPoint.isLeftSide) return;
        if (connectedWires[leftPoint.wireID]) return;

        selectedLeftPoint = leftPoint;
        Debug.Log("Selected left wire: " + leftPoint.wireID);
    }

    public void EndWire(WirePointUI rightPoint)
    {
        if (selectedLeftPoint == null) return;
        if (rightPoint.isLeftSide) return;

        if (selectedLeftPoint.wireID == rightPoint.wireID)
        {
            if (!connectedWires[selectedLeftPoint.wireID])
            {
                connectedWires[selectedLeftPoint.wireID] = true;

                GameObject lineObj = Instantiate(linePrefab, lineContainer);
                RectTransform lineRect = lineObj.GetComponent<RectTransform>();

                // FIX 1: Ignore the folders! Convert the exact 3D world position of the points 
                // directly into the local coordinates of the LineContainer.
                Vector2 startPos = lineContainer.InverseTransformPoint(selectedLeftPoint.rectTransform.position);
                Vector2 endPos = lineContainer.InverseTransformPoint(rightPoint.rectTransform.position);

                UpdateLine(lineRect, startPos, endPos);

                createdLines[selectedLeftPoint.wireID] = lineObj;
            }

            selectedLeftPoint = null;
            CheckSolved();
        }
        else
        {
            Debug.Log("Wrong wire pair!");
            selectedLeftPoint = null;
        }
    }

    private void CheckSolved()
    {
        foreach (var pair in connectedWires)
        {
            if (!pair.Value)
                return;
        }

        Debug.Log("Wire puzzle solved!");
        PuzzleState.wirePuzzleSolved = true;
        ApplySolvedState();
    }

    public void ResetPuzzle()
    {
        selectedLeftPoint = null;

        foreach (Transform child in lineContainer)
        {
            Destroy(child.gameObject);
        }

        connectedWires[0] = false;
        connectedWires[1] = false;
        connectedWires[2] = false;

        createdLines.Clear();

        Debug.Log("Puzzle reset.");
    }

    private void UpdateLine(RectTransform line, Vector2 start, Vector2 end)
    {
        // FIX 2: Force the line's pivot to the absolute left edge via code 
        // so it perfectly anchors to the starting click.
        line.pivot = new Vector2(0f, 0.5f);

        Vector2 direction = end - start;
        float length = direction.magnitude;

        line.sizeDelta = new Vector2(length, 8f);

        // Place the start of the line exactly on the start coordinate
        line.anchoredPosition = start;

        line.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}