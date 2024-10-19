using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int currentSatisfaction;
    private int selectedCompanionID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedCompanionID(int id)
    {
        selectedCompanionID = id;
        LoadSatisfaction(); // Load satisfaction when the companion is set
    }

    public int GetSelectedCompanionID()
    {
        return selectedCompanionID;
    }

    public void SetSatisfaction(int satisfaction)
    {
        currentSatisfaction = satisfaction;
        SaveSatisfaction(); // Save every time it's updated
    }

    public int GetSatisfaction()
    {
        return currentSatisfaction;
    }

    private void SaveSatisfaction()
    {
        PlayerPrefs.SetInt("CompanionSatisfaction_" + selectedCompanionID, currentSatisfaction);
        PlayerPrefs.Save();
    }

    private void LoadSatisfaction()
    {
        currentSatisfaction = PlayerPrefs.GetInt("CompanionSatisfaction_" + selectedCompanionID, 50); // Default to full satisfaction
    }
}
