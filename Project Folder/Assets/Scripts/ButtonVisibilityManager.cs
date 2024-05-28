using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonVisibilityManager : MonoBehaviour
{
    public Button separationButton; // Le bouton "Animation"
    public List<Button> otherButtons; // Les autres boutons à masquer/afficher
    public List<MoveUpWithSeparation> moveUpComponents; // Les composants de déplacement vers le haut
    public List<MoveDownWithSeparation> moveDownComponents; // Les composants de déplacement vers le bas

    private bool isSeparated = false;

    void Start()
    {
        separationButton.onClick.AddListener(ToggleSeparation);
    }

    void ToggleSeparation()
    {
        if (!isSeparated)
        {
            // Masquer les autres boutons et séparer les composants
            SetButtonsVisibility(false);
            foreach (var component in moveUpComponents)
            {
                component.Move();
            }
            foreach (var component in moveDownComponents)
            {
                component.Move();
            }
            isSeparated = true;
        }
        else
        {
            // Afficher les autres boutons et ramener les composants à leur position d'origine
            SetButtonsVisibility(true);
            foreach (var component in moveUpComponents)
            {
                component.Move();
            }
            foreach (var component in moveDownComponents)
            {
                component.Move();
            }
            isSeparated = false;
        }
    }

    void SetButtonsVisibility(bool isVisible)
    {
        foreach (var button in otherButtons)
        {
            button.gameObject.SetActive(isVisible);
        }
    }
}
