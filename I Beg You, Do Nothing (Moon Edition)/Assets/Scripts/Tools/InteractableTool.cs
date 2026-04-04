using UnityEngine;
using UnityEngine.EventSystems; // Necesar pentru a detecta click-urile pe UI

// Clasa abstractă înseamnă că nu putem atașa acest script direct, 
// ci doar scripturile care o moștenesc (precum UVLightTool)
public abstract class InteractableTool : MonoBehaviour, IPointerClickHandler
{
    [Header("Tool Info")]
    public string toolName = "Generic Tool";

    // Funcție abstractă: fiecare unealtă va decide ce face când este folosită
    public abstract void UseTool();

    // Această funcție este apelată automat de Unity când dai click pe obiectul de UI
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        UseTool();
    }
}