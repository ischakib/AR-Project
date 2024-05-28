using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;

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
                if (hit.transform.tag == "Lungs")
                {
                    ShowInfo("Lungs");
                }
                else if (hit.transform.tag == "Liver")
                {
                    ShowInfo("Liver");
                }
                else if (hit.transform.tag == "Urinary System")
                {
                    ShowInfo("Urinary System");
                }
                else if (hit.transform.tag == "Colon")
                {
                    ShowInfo("Colon");
                }
                else if (hit.transform.tag == "Gingiva")
                {
                    ShowInfo("Gingiva");
                }
                else if (hit.transform.tag == "Reproductive System")
                {
                    ShowInfo("Reproductive System");
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

    private void ShowInfo(string organName)
    {
        UnityEngine.Debug.Log($"Hit on {organName}");

        objectNameTMP.text = $"Clicked on: {organName}";
        objectNameTMP.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        moreInfoButton.gameObject.SetActive(true);
        descriptionTMP.gameObject.SetActive(false); // Hide description initially

        // Show and animate the titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(hideShowBarOriginalPos.x, hideShowBarOriginalPos.y - descriptionPanelRect.rect.height ), 0.3f));

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

        // Hide titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, -Screen.height), 0.3f));

        // Hide descriptionPanel
        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, -Screen.height), 0.3f));

        UnityEngine.Debug.Log("Description text and other UI elements hidden.");
    }

    // Method to show more information from the file
    public void ShowMoreInfo()
    {
        string organName = objectNameTMP.text.Replace("Clicked on: ", "");
        string description = ReadDescriptionFromFile(organName + "Description");

        descriptionTMP.text = description;

        objectNameTMP.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        moreInfoButton.gameObject.SetActive(true);

        descriptionTMP.gameObject.SetActive(true);
        // Show and animate the descriptionPanel
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, titleInfoButtonBarOriginalPos.y), 0.3f));

        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, descriptionPanelOriginalPos.y), 0.3f));

        // Move the titleInfoButtonBar up to make space
        //StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, titleInfoButtonBarOriginalPos.y + titleInfoButtonBarRect.rect.height), 0.5f));

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

        // Hide titleInfoButtonBar
        StartCoroutine(SmoothMove(titleInfoButtonBarRect, new Vector2(titleInfoButtonBarOriginalPos.x, -Screen.height), 0.3f));

        // Hide descriptionPanel
        StartCoroutine(SmoothMove(descriptionPanelRect, new Vector2(descriptionPanelOriginalPos.x, -Screen.height), 0.3f));

        UnityEngine.Debug.Log("Cleared all texts and buttons.");
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
