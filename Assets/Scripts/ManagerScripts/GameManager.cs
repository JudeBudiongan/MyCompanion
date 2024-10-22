using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentSatisfaction; // Holds the current satisfaction level
    private int selectedCompanionID; // Holds the ID of the selected companion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate GameManagers
        }
    }

    // Set the selected companion ID and load satisfaction
    public void SetSelectedCompanionID(int id)
    {
        selectedCompanionID = id;
        LoadSatisfaction(); // Load satisfaction when the companion is set
    }

    // Get the selected companion ID
    public int GetSelectedCompanionID()
    {
        return selectedCompanionID;
    }

    // Set the satisfaction level and save it
    public void SetSatisfaction(int satisfaction)
    {
        currentSatisfaction = satisfaction;
        SaveSatisfaction(); // Save every time it's updated
    }

    // Get the current satisfaction level
    public int GetSatisfaction()
    {
        return currentSatisfaction;
    }

    // Save the current satisfaction to PlayerPrefs
    private void SaveSatisfaction()
    {
        PlayerPrefs.SetInt("CompanionSatisfaction_" + selectedCompanionID, currentSatisfaction);
        PlayerPrefs.Save();
    }

    // Load the satisfaction from PlayerPrefs
    private void LoadSatisfaction()
    {
        currentSatisfaction = PlayerPrefs.GetInt("CompanionSatisfaction_" + selectedCompanionID, 50); // Default to 50 if not set
    }

    // Optional: Check if a valid companion is selected
    public bool IsCompanionSelected()
    {
        return selectedCompanionID >= 0; // Assuming IDs start from 0
    }

    // Optional: Reset satisfaction to a default value
    public void ResetSatisfaction()
    {
        currentSatisfaction = 50; // Reset to a default value
        SaveSatisfaction();
    }
}
