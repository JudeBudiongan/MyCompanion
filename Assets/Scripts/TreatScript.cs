using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatScript : MonoBehaviour
{
    public HealthBar healthBar;  // Reference to the HealthBar script
    private int treatsAvailable = 0;  // Number of treats available
    private int currentHealth;   // Player's current health
    public int maxHealth = 100;  // Max health value
    public int healthIncreaseAmount = 10; // Amount to increase health per treat

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

        // Initialize the health bar
        currentHealth = maxHealth;  // Start health at max
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);  // Set the initial health
    }

    // Method to add treats (e.g., when bought from the shop)
    public void AddTreats(int amount)
    {
        treatsAvailable += amount;
        Debug.Log("Treats added: " + amount + ". Total treats available: " + treatsAvailable);
    }

    // Method to use a treat and increase health
    public void UseTreat()
    {
        if (treatsAvailable > 0)  // Check if there are treats available
        {
            if (currentHealth < maxHealth)  // Check if health can be increased
            {
                currentHealth += healthIncreaseAmount;  // Increase health by defined amount
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;  // Ensure health doesn't exceed maxHealth
                }

                healthBar.SetHealth(currentHealth);  // Update the health bar
                treatsAvailable--;  // Decrease the number of treats available
                Debug.Log("Used a treat. Current health: " + currentHealth + ". Remaining treats: " + treatsAvailable);
                Debug.Log("Treat has been fed to the companion!"); // Log feeding message
            }
            else
            {
                Debug.Log("Health is already full.");
            }
        }
        else
        {
            Debug.Log("No treats available.");
        }
    }
}
