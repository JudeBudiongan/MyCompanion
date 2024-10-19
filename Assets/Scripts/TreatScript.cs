using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Ensure to include this for UI Button

public class TreatScript : MonoBehaviour
{
    public HealthBar healthBar;  // Reference to the HealthBar script
    private TreatManager.TreatItem currentTreat;  // Reference to the specific treat being used
    private int currentHealth;   // Player's current health
    public int maxHealth = 100;  // Max health value
    public int healthIncreaseAmount = 10; // Amount to increase health per treat

    private Button buyButton; // Reference to the buy button
    private bool isBuying = false; // Prevent multiple buys

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
        currentHealth = 50;  // Start health at a specified level
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);  // Set the initial health

        // Find the button in the scene
        buyButton = FindObjectOfType<Button>(); // Modify to find your specific button or assign in Inspector
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonPressed);
        }
    }

    // Method to handle buy button press
    private void OnBuyButtonPressed()
    {
        if (!isBuying) // Check if already in a buying process
        {
            isBuying = true; // Set buying flag
            AddTreats(0, 1); // Buy treat ID 0 and quantity 1
            StartCoroutine(ResetBuyingFlag()); // Reset after a short delay
        }
    }

    private IEnumerator ResetBuyingFlag()
    {
        yield return new WaitForSeconds(1f); // Delay before allowing another purchase
        isBuying = false; // Allow new purchases again
    }

    // Method to use a treat and increase health
    public void UseTreat()
    {
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

        if (currentTreat.Quantity > 0)  // Check if treats are available in player storage
        {
            if (currentHealth < maxHealth)  // Check if health can be increased
            {
                currentHealth += healthIncreaseAmount;  // Increase health by the defined amount
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;  // Ensure health doesn't exceed maxHealth
                }

                healthBar.SetHealth(currentHealth);  // Update the health bar
                currentTreat.Quantity--;  // Decrease the player's available treats (not stock)
                Debug.Log("Used a treat. Current health: " + currentHealth + ". Remaining player treats: " + currentTreat.Quantity);
            }
            else
            {
                Debug.Log("Health is already full.");
            }
        }
        else
        {
            Debug.Log("No treats available in player inventory.");
        }
    }

    // Method to add treats when bought from the shop
    public void AddTreats(int treatID, int amount)
    {
        // We set amount to 1 here since we only want to add one treat when bought
        amount = 1;

        // Retrieve the treat from the TreatManager by its ID
        currentTreat = TreatManager.Instance.GetTreatById(treatID);

        if (currentTreat != null)
        {
            // Increase the treat quantity in the TreatManager
            TreatManager.Instance.IncreaseTreatStock(treatID, amount);  // Increase the shop stock
            TreatManager.Instance.IncreaseQuantity(treatID, amount);    // Add to the player's inventory
            Debug.Log($"{amount} treat of {currentTreat.TreatName} added to player inventory.");
        }
        else
        {
            Debug.LogError("Invalid treat ID.");
        }
    }
}
