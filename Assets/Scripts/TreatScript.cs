using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompanionManager;

public class TreatScript : MonoBehaviour
{
    public HealthBar healthBar; // Reference to the HealthBar script
    public CompanionManager companionManager; // Reference to the CompanionManager script
    public int treatsAvailable = 15; // Number of treats available
    public int satisfactionIncreaseAmount = 10; // Amount to increase satisfaction per treat

    private Companion selectedCompanion; // The currently selected companion

    void Start()
    {
        // Check if the healthBar is not set in the Inspector, find it dynamically
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();

            if (healthBar == null)
            {
                Debug.LogError("HealthBar not found in the scene! Make sure it's assigned or exists in the hierarchy.");
                return;
            }
        }

        // Check if the CompanionManager is not set in the Inspector, find it dynamically
        if (companionManager == null)
        {
            companionManager = CompanionManager.Instance;

            if (companionManager == null)
            {
                Debug.LogError("CompanionManager not found in the scene! Make sure it's assigned or exists in the hierarchy.");
                return;
            }
        }

        // Retrieve the currently selected companion from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if no companion is selected
        selectedCompanion = companionManager.GetCompanionById(selectedID);

        if (selectedCompanion != null)
        {
            // Initialize the health bar with the selected companion's satisfaction
            healthBar.SetMaxSatisfaction(100); // Assuming max satisfaction is 100
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);
        }
        else
        {
            Debug.LogWarning("No companion has been selected or found.");
        }
    }

    // Method to add treats (e.g., when bought from the shop)
    public void AddTreats(int amount)
    {
        treatsAvailable += amount;
        Debug.Log("Treats added: " + amount + ". Total treats available: " + treatsAvailable);
    }

    // Method to use a treat and increase satisfaction
    public void UseTreat()
    {
        if (selectedCompanion == null)
        {
            Debug.LogWarning("No companion selected to feed.");
            return;
        }

        if (treatsAvailable > 0) // Check if there are treats available
        {
            if (selectedCompanion.SatisfactionLevel < 100) // Check if satisfaction can be increased
            {
                selectedCompanion.IncreaseSatisfaction(satisfactionIncreaseAmount); // Increase satisfaction
                if (selectedCompanion.SatisfactionLevel > 100)
                {
                    selectedCompanion.SatisfactionLevel = 100; // Ensure satisfaction doesn't exceed max
                }

                healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel); // Update the health bar
                treatsAvailable--; // Decrease the number of treats available
                Debug.Log("Used a treat. " + selectedCompanion.PetName + "'s satisfaction: " + selectedCompanion.SatisfactionLevel + ". Remaining treats: " + treatsAvailable);
            }
            else
            {
                Debug.Log(selectedCompanion.PetName + "'s satisfaction is already full.");
            }
        }
        else
        {
            Debug.Log("No treats available.");
        }
    }
}
