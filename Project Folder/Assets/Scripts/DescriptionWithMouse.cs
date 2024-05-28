using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions; // Needed for Regex

public class DescriptionWithMouse : MonoBehaviour
{
    public TMP_Text objectNameTMP;  // Reference to the TMP Text element for object name
    public TMP_Text descriptionTMP; // Reference to the TMP Text element for description
    public Button closeButton;      // Reference to the Close button element
    public Button moreInfoButton;   // Reference to the More Info button element

    public GameObject titleInfoButtonBar;  // Reference to the Title&infobuttonbar GameObject
    public GameObject descriptionPanel;    // Reference to the DescriptionPanel GameObject
    public GameObject hideShowBar;         // Reference to the Hide&ShowBar GameObject

    private RectTransform hideShowBarRect;
    private RectTransform titleInfoButtonBarRect;
    private RectTransform descriptionPanelRect;

    private Vector2 hideShowBarOriginalPos;
    private Vector2 titleInfoButtonBarOriginalPos;
    private Vector2 descriptionPanelOriginalPos;

    private string clickedObjectTag; // To store the tag of the clicked object
    private GameObject highlightedObject; // To store the currently highlighted object
    private Color originalColor; // To store the original color of the clicked object

    void Start()
    {
        // Initially hide the TMP Texts and the Buttons
        objectNameTMP.gameObject.SetActive(false);
        descriptionTMP.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        moreInfoButton.gameObject.SetActive(false);

        // Get RectTransform components
        hideShowBarRect = hideShowBar.GetComponent<RectTransform>();
        titleInfoButtonBarRect = titleInfoButtonBar.GetComponent<RectTransform>();
        descriptionPanelRect = descriptionPanel.GetComponent<RectTransform>();

        // Store original positions
        hideShowBarOriginalPos = hideShowBarRect.anchoredPosition;
        titleInfoButtonBarOriginalPos = titleInfoButtonBarRect.anchoredPosition;
        descriptionPanelOriginalPos = descriptionPanelRect.anchoredPosition;

        // Initially hide the titleInfoButtonBar and descriptionPanel elements
        titleInfoButtonBarRect.anchoredPosition = new Vector2(titleInfoButtonBarOriginalPos.x, -Screen.height);
        descriptionPanelRect.anchoredPosition = new Vector2(descriptionPanelOriginalPos.x, -Screen.height);

        // Add listeners to the Buttons
        closeButton.onClick.AddListener(CloseText);
        moreInfoButton.onClick.AddListener(ShowMoreInfo);

        UnityEngine.Debug.Log("UI elements initialized and listeners added.");
    }

    void Update()
    {
        // Check for touch input
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Debug.Log("Pressed primary button.");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                UnityEngine.Debug.Log("Hit: " + hit.transform.name + " : " + hit.transform.tag);

                // Update the TMP Text with the name of the hit object
                if (hit.transform.tag == "RespiratorySystem" ||
                    hit.transform.tag == "Liver" ||
                    hit.transform.tag == "UrinarySystem" ||
                    hit.transform.tag == "DigestiveSystem" ||
                    hit.transform.tag == "EndocrineGlands" ||
                    hit.transform.tag == "GenitalSystem" ||
                    hit.transform.tag == "CNS" ||
                    hit.transform.tag == "PNS")
                {
                    ShowInfo(hit.transform.name, hit.transform.tag, hit.transform.gameObject);
                }
                else
                {
                    ClearText();
                    UnityEngine.Debug.Log("Cleared text and buttons.");
                }

                if (hit.transform.tag == "info")
                {
                    ClearText();
                    UnityEngine.Debug.Log("Cleared text and buttons (info tag).");
                }
            }
        }
    }

    private void ShowInfo(string organName, string tag, GameObject clickedObject)
    {
        UnityEngine.Debug.Log($"Hit on {organName}");

        // Reset the color of the previously highlighted object
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
        }

        // Store the original color of the clicked object
        Renderer clickedRenderer = clickedObject.GetComponent<Renderer>();
        if (clickedRenderer != null)
        {
            originalColor = clickedRenderer.material.color;
            // Change the color to highlight it
            clickedRenderer.material.color = Color.yellow;
        }

        highlightedObject = clickedObject; // Store the clicked object

        clickedObjectTag = tag; // Store the tag of the clicked object

        // Clean the organ name before displaying it
        string cleanedName = CleanName(organName);

        objectNameTMP.text = $"{cleanedName}";
        objectNameTMP.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        moreInfoButton.gameObject.SetActive(true);
        descriptionTMP.gameObject.SetActive(false); // Hide description initially

        // Show and animate the titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(hideShowBarOriginalPos.x, hideShowBarOriginalPos.y - descriptionPanelRect.rect.height), 0.3f));

        // Hide descriptionPanel
        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, -Screen.height), 0.3f));

        UnityEngine.Debug.Log("Displayed object name, buttons, and infobar.");
    }

    // Method to close the TMP Texts and hide UI elements
    public void CloseText()
    {
        objectNameTMP.gameObject.SetActive(false);
        descriptionTMP.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        moreInfoButton.gameObject.SetActive(false);

        // Reset the color of the highlighted object
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
            highlightedObject = null;
        }

        // Hide titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, -Screen.height), 0.3f));

        // Hide descriptionPanel
        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, -Screen.height), 0.3f));

        UnityEngine.Debug.Log("Description text and other UI elements hidden.");
    }

    // Method to show more information from the file
    public void ShowMoreInfo()
    {
        string description = ReadDescriptionFromFile(clickedObjectTag + "Description");

        descriptionTMP.text = description;

        objectNameTMP.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        moreInfoButton.gameObject.SetActive(true);

        descriptionTMP.gameObject.SetActive(true);
        // Show and animate the descriptionPanel
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, titleInfoButtonBarOriginalPos.y), 0.3f));

        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, descriptionPanelOriginalPos.y), 0.3f));

        UnityEngine.Debug.Log("Displayed more info.");
    }

    // Method to read the description from a file
    private string ReadDescriptionFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            return textAsset.text;
        }
        else
        {
            UnityEngine.Debug.LogError("File not found in Resources: " + fileName);
            return "Description file not found.";
        }
    }

    // Method to clear all text and buttons
    private void ClearText()
    {
        objectNameTMP.text = "";
        objectNameTMP.gameObject.SetActive(false);
        descriptionTMP.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        moreInfoButton.gameObject.SetActive(false);

        // Reset the color of the highlighted object
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
            highlightedObject = null;
        }

        // Hide titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, -Screen.height), 0.3f));

        // Hide descriptionPanel
        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, -Screen.height), 0.3f));

        UnityEngine.Debug.Log("Cleared all texts and buttons.");
    }

    // Method to clean the name by removing numbers, special characters (except underscores), "grp", and replacing underscores with spaces
    private string CleanName(string name)
    {
        // Remove "grp" substring
        string cleanedName = name.Replace("grp", "");
        // Remove numbers and special characters except underscores
        cleanedName = Regex.Replace(cleanedName, "[^a-zA-Z_]", "");
        // Replace underscores with spaces
        cleanedName = cleanedName.Replace("_", " ");
        return cleanedName;
    }

    // Coroutine to smoothly move a RectTransform to a target position
    private IEnumerator SmoothMove(RectTransform rectTransform, Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float elapsed = 0;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }
}
