using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxSatisfaction = 100; // Assuming max satisfaction is 100

    public HealthBar healthBar;
    public CompanionManager companionManager;

    private Companion selectedCompanion;

    // Start is called before the first frame update
    void Start()
    {
        if (companionManager == null)
        {
            companionManager = CompanionManager.Instance;
        }

        // Retrieve the currently selected companion from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("SelectedID", -1); // Default to -1 if no companion is selected
        selectedCompanion = companionManager.GetCompanionById(selectedID);

        if (selectedCompanion != null)
        {
            healthBar.SetMaxSatisfaction(maxSatisfaction);
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);
        }
        else
        {
            Debug.LogWarning("No companion has been selected or found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Example input to modify satisfaction, just for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DecreaseSatisfaction(10);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseSatisfaction(10);
        }
    }

    public void DecreaseSatisfaction(int amount)
    {
        if (selectedCompanion != null)
        {
            selectedCompanion.SatisfactionLevel -= amount;
            if (selectedCompanion.SatisfactionLevel < 0) selectedCompanion.SatisfactionLevel = 0;
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);

            // Save the updated satisfaction level to maintain consistency across scenes
            companionManager.SaveCompanionData(selectedCompanion);
        }
    }

    public void IncreaseSatisfaction(int amount)
    {
        if (selectedCompanion != null)
        {
            selectedCompanion.SatisfactionLevel += amount;
            if (selectedCompanion.SatisfactionLevel > maxSatisfaction) selectedCompanion.SatisfactionLevel = maxSatisfaction;
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);

            // Save the updated satisfaction level to maintain consistency across scenes
            companionManager.SaveCompanionData(selectedCompanion);
        }
    }
}
