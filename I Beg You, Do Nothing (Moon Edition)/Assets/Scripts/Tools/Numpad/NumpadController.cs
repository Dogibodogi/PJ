using UnityEngine;
using TMPro;

public class NumpadController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI screenText;

    [Header("Dynamic References")]
    public FingerprintManager fingerprintManager;

    [Header("Numpad Settings")]
    public int maxCodeLength = 4;
    private string currentInput = "";

    [Header("Puzzle Elements")]
    public GameObject gridPuzzlePanel;
    public AudioSource musicSource;
    public AudioClip abandonShipClip;

    void Start()
    {
        UpdateScreen();
    }

    public void ButtonPressed(string value)
    {
        if (value == "CLEAR") ClearInput();
        else if (value == "BACK") DeleteLastDigit();
        else if (value == "ENTER") CheckCode();
        else AddDigit(value);
    }

    private void AddDigit(string digit)
    {
        if (currentInput.Length < maxCodeLength)
        {
            currentInput += digit;
            UpdateScreen();
        }
    }

    private void DeleteLastDigit()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateScreen();
        }
    }

    private void ClearInput()
    {
        currentInput = "";
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        if (screenText != null)
        {
            screenText.text = (currentInput == "") ? "----" : currentInput;
        }
    }

    private void CheckCode()
    {
        // Trim removes any accidental invisible spaces at the start or end
        string input = currentInput.Trim();

        if (fingerprintManager == null)
        {
            Debug.LogError("NumpadController error: FingerprintManager is NOT assigned in the Inspector!");
            // We don't return here so the static codes (1234, etc) still work
        }
        else
        {
            // DEEP DEBUG: This will show us if there is a hidden mismatch
            Debug.Log($"[Comparison] Input: '{input}' | High: '{fingerprintManager.highestCode}' | Low: '{fingerprintManager.lowestCode}'");

            if (input == fingerprintManager.highestCode.Trim())
            {
                Debug.Log("<color=green>SUCCESS:</color> Highest fingerprint code entered!");
                ClearInput();
                return;
            }

            if (input == fingerprintManager.lowestCode.Trim())
            {
                Debug.Log("<color=green>SUCCESS:</color> Lowest fingerprint code entered!");
                ClearInput();
                return;
            }
        }

        // 2. Check Static Secret Codes using the trimmed 'input'
        switch (input)
        {
            case "1234":
                Debug.Log("COD CORECT: Se deschid obloanele!");
                break;
            case "6666":
                Debug.Log("COD FATAL: Autodistrugere!");
                break;
            case "2540":
                if (gridPuzzlePanel != null) gridPuzzlePanel.SetActive(true);
                break;
            case "2018":
                PlayAbandonShip();
                break;
            default:
                Debug.Log($"<color=red>FAILURE:</color> Code '{input}' not recognized.");
                screenText.text = "ERR";
                Invoke("ClearInput", 1f);
                return;
        }

        ClearInput();
    }

    private void PlayAbandonShip()
    {
        if (musicSource != null && abandonShipClip != null)
        {
            musicSource.Stop();
            musicSource.clip = abandonShipClip;
            musicSource.Play();
        }
    }
}