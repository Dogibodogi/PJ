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

    private void Start()
    {
        connectedWires[0] = false;
        connectedWires[1] = false;
        connectedWires[2] = false;
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

                Vector2 startPos = selectedLeftPoint.rectTransform.anchoredPosition;
                Vector2 endPos = rightPoint.rectTransform.anchoredPosition;

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
        Vector2 direction = end - start;
        float length = direction.magnitude;

        line.sizeDelta = new Vector2(length, 8f);
        line.anchoredPosition = start + direction / 2f;
        line.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}