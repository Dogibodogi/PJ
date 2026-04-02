using UnityEngine;
using TMPro; // Folosim TextMeshPro pentru textul de pe ecran

public class NumpadController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Trage aici obiectul TextMeshPro care reprezintă ecranul")]
    public TextMeshProUGUI screenText;

    [Header("Numpad Settings")]
    public int maxCodeLength = 4;
    private string currentInput = "";

    // for panel on/off activation
    public GameObject gridPuzzlePanel;

    // for audio clip
    public AudioSource musicSource;
    public AudioClip abandonShipClip;

    void Start()
    {
        UpdateScreen();
    }

    // Această funcție va fi apelată de butoane (NumpadButton)
    public void ButtonPressed(string value)
    {
        if (value == "CLEAR")
        {
            ClearInput();
        }
        else if (value == "BACK") // AM ADAUGAT ASTA PENTRU BACKSPACE
        {
            DeleteLastDigit();
        }
        else if (value == "ENTER")
        {
            CheckCode();
        }
        else
        {
            AddDigit(value);
        }
    }

    private void AddDigit(string digit)
    {
        // Adăugăm cifra doar dacă nu am atins limita maximă
        if (currentInput.Length < maxCodeLength)
        {
            currentInput += digit;
            UpdateScreen();
        }
    }

    // Funcție nouă pentru Backspace
    private void DeleteLastDigit()
    {
        if (currentInput.Length > 0)
        {
            // Ștergem ultimul caracter din string
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
            // Dacă nu e nimic introdus, afișăm niște liniuțe pentru design
            if (currentInput == "")
            {
                screenText.text = "----";
            }
            else
            {
                screenText.text = currentInput;
            }
        }
    }

    private void PlayAbandonShip()
    {
        if (musicSource != null && abandonShipClip != null)
        {
            musicSource.Stop();
            musicSource.clip = abandonShipClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("MusicSource // AbandonShipClip not set in Inspector.");
        }
    }

    // Aici definești ce se întâmplă pentru fiecare cod secret!
    private void CheckCode()
    {
        Debug.Log("Verific codul: " + currentInput);

        switch (currentInput)
        {
            case "6742":
                Debug.Log("RONALDO SUIIII!");
                // Aici poți apela o funcție din alt script, ex: BlastDoors.Open();
                break;

            case "6666":
                Debug.Log("COD FATAL: Autodistrugere inițiată!");
                // Exemplu: Apelezi o funcție din DeskController pentru a face masa roșie
                DeskController desk = FindObjectOfType<DeskController>();
                if (desk != null) desk.OnRedButtonPressed();
                break;

            case "0000":
                Debug.Log("COD CORECT: Întoarcere pe Pământ!");
                break;

            case "2540":
                Debug.Log("Hidden panel activated");
                if (gridPuzzlePanel != null)
                    gridPuzzlePanel.SetActive(true);
                break;

            case "2018":
                Debug.Log("Now playing: Abandon Ship (created by nicubynicu)");
                PlayAbandonShip();
                break;

            default:
                Debug.Log("Cod incorect/necunoscut.");
                screenText.text = "ERR"; // Afișăm o eroare scurtă
                Invoke("ClearInput", 1f); // Ștergem eroarea după 1 secundă
                return; // Oprim execuția aici ca să nu șteargă imediat
        }

        // După un cod de succes, curățăm ecranul
        ClearInput();
    }
}