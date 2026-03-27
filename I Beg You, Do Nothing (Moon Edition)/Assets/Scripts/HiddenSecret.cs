using UnityEngine;
using UnityEngine.UI;

// Ne asigurăm că obiectul are o componentă Image
[RequireComponent(typeof(Image))]
public class HiddenSecret : MonoBehaviour
{
    [Header("Secret Settings")]
    [Tooltip("Transparența când lumina este departe sau STINSĂ")]
    public float hiddenAlpha = 0f;
    [Tooltip("Transparența maximă când lumina este chiar deasupra secretului")]
    public float revealedAlpha = 1f;

    [Header("Distance Settings")]
    [Tooltip("Cât de aproape trebuie să fie lampa pentru a vedea secretul")]
    public float revealRadius = 200f; // Poți modifica asta din Unity Editor!

    private Image secretImage;
    private UVLightTool uvLamp; // Referință către lampa UV

    void Awake()
    {
        secretImage = GetComponent<Image>();

        // Găsim automat lampa UV în scenă ca să îi putem verifica poziția
        uvLamp = FindObjectOfType<UVLightTool>();

        // La început, secretul este invizibil
        SetAlpha(hiddenAlpha);
    }

    // Folosim Update pentru a verifica distanța în fiecare cadru
    void Update()
    {
        // Dacă nu avem lampă în scenă sau lumina este stinsă, ținem secretul ascuns
        if (uvLamp == null || !uvLamp.isUVLightOn)
        {
            SetAlpha(hiddenAlpha);
            return;
        }

        // Calculăm distanța dintre acest secret (amprentă) și lampa UV
        float distance = Vector3.Distance(transform.position, uvLamp.transform.position);

        // Verificăm dacă lampa se află în raza setată
        if (distance <= revealRadius)
        {
            // Calculăm un efect de estompare (fade) pe măsură ce te îndepărtezi de centru
            // Când distanța e 0, fadeAmount e 1. Când distanța e egală cu raza, fadeAmount e 0.
            float fadeAmount = 1f - (distance / revealRadius);

            // Setăm transparența în funcție de cât de aproape ești
            float currentAlpha = Mathf.Lerp(hiddenAlpha, revealedAlpha, fadeAmount);
            SetAlpha(currentAlpha);
        }
        else
        {
            // Dacă lampa e aprinsă, dar e prea departe
            SetAlpha(hiddenAlpha);
        }
    }

    private void SetAlpha(float targetAlpha)
    {
        if (secretImage != null)
        {
            Color currentColor = secretImage.color;
            currentColor.a = targetAlpha; // Modificăm doar canalul Alpha
            secretImage.color = currentColor;
        }
    }
}