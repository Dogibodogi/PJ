using System.Collections.Generic;
using System.Linq; // Added for sorting
using UnityEngine;

public class FingerprintManager : MonoBehaviour
{
    public string highestCode;
    public string lowestCode;

    void Start()
    {
        UVRevealMark[] allMarks = FindObjectsOfType<UVRevealMark>();
        List<UVRevealMark> fingerprintList = new List<UVRevealMark>();

        foreach (UVRevealMark mark in allMarks)
        {
            if (mark.isFingerprint) fingerprintList.Add(mark);
        }

        // Shuffle
        for (int i = 0; i < fingerprintList.Count; i++)
        {
            UVRevealMark temp = fingerprintList[i];
            int randomIndex = Random.Range(i, fingerprintList.Count);
            fingerprintList[i] = fingerprintList[randomIndex];
            fingerprintList[randomIndex] = temp;
        }

        // We will store the digits of the 4 chosen fingerprints here
        List<int> chosenDigits = new List<int>();

        for (int i = 0; i < fingerprintList.Count; i++)
        {
            if (i < 4)
            {
                fingerprintList[i].revealedAlpha = 1.0f;
                chosenDigits.Add(fingerprintList[i].digitValue); // Save the digit
            }
            else
            {
                fingerprintList[i].revealedAlpha = Random.Range(0.1f, 0.3f);
            }
        }

        CalculateMinMax(chosenDigits);
    }

    void CalculateMinMax(List<int> digits)
    {
        // 1. Sort the digits in ascending order (e.g., 1, 3, 5, 8)
        digits.Sort();

        // 2. Create the lowest number (e.g., "1358")
        string lowStr = "";
        foreach (int d in digits) lowStr += d.ToString();
        lowestCode = lowStr;

        // 3. Reverse the list for the highest number (e.g., "8531")
        digits.Reverse();
        string highStr = "";
        foreach (int d in digits) highStr += d.ToString();
        highestCode = highStr;

        Debug.Log("The 4 digits are: " + string.Join(", ", digits));
        Debug.Log("Highest Possible Code: " + highestCode);
        Debug.Log("Lowest Possible Code: " + lowestCode);
    }
}