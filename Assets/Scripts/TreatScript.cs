using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Ensure to include this for UI Button
using static CompanionManager;

public class TreatScript : MonoBehaviour
{
    public HealthBar healthBar;  // Reference to the HealthBar script
    private TreatManager.TreatItem currentTreat;  // Reference to the specific treat being used
    public CompanionManager companionManager;
    public int satisfactionIncreaseAmount = 10; // Amount to increase health per treat
    private Companion selectedCompanion;

    private Button buyButton; // Reference to the buy button

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
    }

    

    // Method to use a treat and increase health
    public void UseTreat()
{
    // Ensure a companion is selected before using a treat
    if (selectedCompanion == null)
    {
        Debug.LogError("No companion is selected. Please select a companion first.");
        return;
    }

    // Get all available treats from the TreatManager
    List<TreatManager.TreatItem> availableTreats = TreatManager.Instance.GetAllAvailableTreats();

    if (availableTreats.Count == 0)
    {
        Debug.Log("No treats available in player inventory.");
        return;
    }

    // Select the first available treat for this example
    currentTreat = availableTreats[0]; // You can implement your selection logic here

    Debug.Log("Attempting to use treat: " + currentTreat.TreatName); // Debug log

    // Ensure there are treats left before proceeding
    if (currentTreat.Quantity <= 0)
    {
        Debug.Log("No treats available in player inventory.");
        return; // Early exit if there are no treats left
    }

    // Check if satisfaction can be increased
    if (selectedCompanion.SatisfactionLevel < 100)
    {
        selectedCompanion.IncreaseSatisfaction(satisfactionIncreaseAmount); // Increase satisfaction by the defined amount
        if (selectedCompanion.SatisfactionLevel > 100)
        {
            selectedCompanion.SatisfactionLevel = 100; // Ensure satisfaction doesn't exceed 100
        }

        healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel); // Update the health bar

        // Update satisfaction in GameManager to maintain consistency across scenes
        GameManager.Instance.SetSatisfaction(selectedCompanion.SatisfactionLevel);

        currentTreat.Quantity--; // Decrease the number of treats available
        Debug.Log("Used a treat. " + selectedCompanion.PetName + "'s satisfaction: " + selectedCompanion.SatisfactionLevel + ". Remaining treats: " + currentTreat.Quantity);

        // Optional: If no more treats are left, you may want to refresh the treat list or UI
        if (currentTreat.Quantity == 0)
        {
            Debug.Log("No more " + currentTreat.TreatName + " left.");
        }
    }
    else
    {
        Debug.Log("Satisfaction is already full.");
    }
}

}
