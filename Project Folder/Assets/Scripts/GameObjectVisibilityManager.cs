using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameObjectVisibilityManager : MonoBehaviour
{
    public Button separationButton; // Le bouton "Animation"
    public List<GameObject> otherSystems; // Les autres systèmes à masquer/afficher

    private bool areSystemsVisible = true;

    void Start()
    {
        separationButton.onClick.AddListener(ToggleSystemsVisibility);
    }

    void ToggleSystemsVisibility()
    {
        //areSystemsVisible = !areSystemsVisible;
        areSystemsVisible = false;
        SetSystemsVisibility(areSystemsVisible);
    }

    void SetSystemsVisibility(bool isVisible)
    {
        foreach (var system in otherSystems)
        {
            system.SetActive(isVisible);
        }
    }
}
