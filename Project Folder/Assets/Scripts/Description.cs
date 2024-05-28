using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    public TMP_Text objectNameTMP;  // Reference to the TMP Text element for object name
    public TMP_Text descriptionTMP; // Reference to the TMP Text element for description
    public Button closeButton;      // Reference to the Close button element
    public Button moreInfoButton;   // Reference to the More Info button element

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the TMP Texts and the Buttons
        objectNameTMP.gameObject.SetActive(false);
        descriptionTMP.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        moreInfoButton.gameObject.SetActive(false);

        // Add listeners to the Buttons
        closeButton.onClick.AddListener(CloseText);
        moreInfoButton.onClick.AddListener(ShowMoreInfo);

        Debug.Log("UI elements initialized and listeners added.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch detected.");

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("Hit: " + hit.transform.name + " : " + hit.transform.tag);

                    // Update the TMP Text with the name of the hit object
                    if (hit.transform.tag == "Lungs")
                    {
                        Debug.Log("Hit on lungs");
                        //objectNameTMP.text = "Clicked on: " + hit.transform.name;
                        objectNameTMP.text = "Clicked on: Lungs";
                        objectNameTMP.gameObject.SetActive(true);
                        closeButton.gameObject.SetActive(true);
                        moreInfoButton.gameObject.SetActive(true);
                        descriptionTMP.gameObject.SetActive(false); // Hide description initially

                        Debug.Log("Displayed object name and buttons.");
                    }
                    else if (hit.transform.tag == "Liver")
                    {
                        Debug.Log("Hit on liver");
                        objectNameTMP.text = "Clicked on: Liver";
                        objectNameTMP.gameObject.SetActive(true);
                        closeButton.gameObject.SetActive(true);
                        moreInfoButton.gameObject.SetActive(true);
                        descriptionTMP.gameObject.SetActive(false); // Hide description initially

                        Debug.Log("Displayed object name and buttons.");
                    }
                    else
                    {
                        ClearText();
                        Debug.Log("Cleared text and buttons.");
                    }

                    if (hit.transform.tag == "info")
                    {
                        ClearText();
                        Debug.Log("Cleared text and buttons (info tag).");

                        // Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    // Method to close the TMP Texts
    public void CloseText()
    {
        descriptionTMP.gameObject.SetActive(false);
        objectNameTMP.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        moreInfoButton.gameObject.SetActive(false);
        Debug.Log("Description text and other UI elements hidden.");
    }

    // Method to show more information from the file
    public void ShowMoreInfo()
    {
        objectNameTMP.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        moreInfoButton.gameObject.SetActive(true);
        string description = "";
        if (objectNameTMP.text == "Clicked on: Lungs")
        {
            description = ReadDescriptionFromFile("LungsDescription");
        }
        if (objectNameTMP.text == "Clicked on: Liver")
        {
            description = ReadDescriptionFromFile("LiverDescription");
        }
        descriptionTMP.text = description;
        descriptionTMP.gameObject.SetActive(true);
        Debug.Log("Displayed more info.");
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
            Debug.LogError("File not found in Resources: " + fileName);
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
        Debug.Log("Cleared all texts and buttons.");
    }
}
