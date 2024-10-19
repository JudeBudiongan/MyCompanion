using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanionManager;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;
    private Companion currentCompanion;
    private int maxSatisfaction;
    private int currentSatisfaction;

    // Reference to TreatScript (assuming it's on the same GameObject or set via Inspector)
    public TreatScript treatScript;

    void Start()
    {
        // Fetch the currently selected companion (using PlayerPrefs or CompanionManager)
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1);
        currentCompanion = CompanionManager.Instance.GetCompanionById(selectedID);

        if (currentCompanion != null)
        {
            maxSatisfaction = currentCompanion.SatisfactionLevel; // Assume this is the max satisfaction
            currentSatisfaction = maxSatisfaction;
        }
        else
        {
            // If no companion is found, use a default value
            maxSatisfaction = 25;
            currentSatisfaction = maxSatisfaction;
        }

        // Set up the health bar
        healthBar.SetMaxSatisfaction(maxSatisfaction);
        healthBar.SetSatisfaction(currentSatisfaction);
    }

    void Update()
    {
        // Use the Heal method via TreatScript
        if (Input.GetKeyDown(KeyCode.T))
        {
            treatScript.UseTreat();
        }
    }

    public void TakeDamage(int damage)
    {
        currentSatisfaction -= damage;

        // Ensure satisfaction doesn't drop below 0
        if (currentSatisfaction < 0)
        {
            currentSatisfaction = 0;
        }

        healthBar.SetSatisfaction(currentSatisfaction);
        UpdateCompanionSatisfaction(); // Update the companion's satisfaction level
    }

    public void Heal(int amount)
    {
        currentSatisfaction += amount;

        // Ensure satisfaction doesn't exceed maxSatisfaction
        if (currentSatisfaction > maxSatisfaction)
        {
            currentSatisfaction = maxSatisfaction;
        }

        healthBar.SetSatisfaction(currentSatisfaction);
        UpdateCompanionSatisfaction(); // Update the companion's satisfaction level
    }

    private void UpdateCompanionSatisfaction()
    {
        if (currentCompanion != null)
        {
            currentCompanion.SatisfactionLevel = currentSatisfaction;
        }
    }
}
