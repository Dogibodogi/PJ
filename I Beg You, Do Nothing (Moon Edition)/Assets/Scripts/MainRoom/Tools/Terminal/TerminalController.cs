using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem; // Required for the New Input System

public class TerminalController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject terminalPanel;
    public TMP_Text outputText;
    public TMP_InputField inputField;

    [Header("Minigame Reference")]
    public ChickenMinigame chickenMinigame;

    [Header("Terminal Settings")]
    public string userName = "USER";
    public string machineName = "SYSTEM_A";

    private FingerprintManager fingerprintManager;

    void Awake()
    {
        // Find the fingerprint manager in the scene for later use
        fingerprintManager = FindObjectOfType<FingerprintManager>();
    }

    void Start()
    {
        // Ensure the terminal starts hidden
        terminalPanel.SetActive(false);

        // Initial text
        outputText.text = "<color=#00FF00>--- TERMINAL INITIALIZED ---</color>\nType 'HELP' for a list of commands.\n";
    }

    void Update()
    {
        // Close terminal with Escape key using the New Input System
        if (terminalPanel.activeSelf && Keyboard.current != null)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleTerminal(false);
            }
        }
    }

    public void ToggleTerminal(bool state)
    {
        terminalPanel.SetActive(state);

        Debug.Log("ToggleTerminalActivated!");

        if (state)
        {
            // Unlock mouse and focus the input field
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            inputField.ActivateInputField();
            inputField.Select();
        }
        else
        {
            // Lock mouse back (useful if you have a first-person controller)
            Cursor.lockState = CursorLockMode.None; // Don't lock it to the center
            Cursor.visible = true;
        }
    }

    // This function is called by the InputField's On End Edit event
    public void OnInputSubmit(string input)
    {
        // 1. Ignore if empty or just whitespace
        if (string.IsNullOrWhiteSpace(input))
        {
            // Even if empty, refocus so the player doesn't have to click
            inputField.ActivateInputField();
            return;
        }

        // 2. Process the command (convert to upper case for easier matching)
        string cleanInput = input.Trim();
        string response = ProcessCommand(cleanInput.ToUpper());

        // 3. Update the output text
        outputText.text += $"\n<color=#aaaaaa>{userName}@{machineName}:</color> {cleanInput}";
        outputText.text += $"\n{response}\n";

        // 4. Clear the input and FORCE focus back to the field
        inputField.text = "";
        inputField.ActivateInputField();
        inputField.Select();
    }

    private string ProcessCommand(string cmd)
    {
        switch (cmd)
        {
            case "HELP":
                return "Commands: HELP, CLEAR, STATUS, REVEAL, EXIT";

            case "CLEAR":
                outputText.text = "";
                return "Console buffer cleared.";

            case "STATUS":
                return "System: <color=green>ONLINE</color>\nSecurity: <color=yellow>VULNERABLE</color>\nSubsystems: Active";

            case "REVEAL":
                if (fingerprintManager != null)
                {
                    return $"<color=cyan>ENCRYPTED DATA FOUND:</color>\nHigh Code: {fingerprintManager.highestCode}\nLow Code: {fingerprintManager.lowestCode}";
                }
                return "Error: Fingerprint Database connection lost.";

            case "EXIT":
                ToggleTerminal(false);
                return "Closing session...";

            case "YNY":
                return "YnySebi superstar! Urc pe scena la concerte lumea ma aplauda...";

            case "CHICKEN INVADERS": // Added command
                if (chickenMinigame != null)
                {
                    // Hide the standard terminal UI and start the game
                    outputText.gameObject.SetActive(false);
                    inputField.gameObject.SetActive(false);
                    chickenMinigame.StartGame();
                    return "";
                }
                return "<color=red>Error: Minigame module missing.</color>";

            default:
                return $"<color=red>Command not recognized:</color> '{cmd}'";
        }
    }
}