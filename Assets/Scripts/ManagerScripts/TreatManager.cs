using System.Collections.Generic;
using UnityEngine;

public class TreatManager : MonoBehaviour
{
    [System.Serializable]
    public class Treat
    {
        public int Stock;      // Stock of the treat (how much is available in the shop)
        public int Quantity;   // Quantity in the player's storage (how much the player owns)

        public Treat(int stock)
        {
            Stock = stock;
            Quantity = 0;  // Initialize quantity in the player's inventory to 0
        }
    }

    [System.Serializable]
    public class TreatItem : Treat
    {
        public int TreatID;   // Unique ID for the treat
        public string TreatName; // Name of the treat

        public TreatItem(int treatID, string treatName)
            : base(10)  // Initialize with 10 stock in the shop
        {
            TreatID = treatID;
            TreatName = treatName;
        }
    }

    public static TreatManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    public List<TreatItem> treats = new List<TreatItem>();

    void Start()
    {
        // Add treat items to the list with IDs and names
        treats.Add(new TreatItem(0, "Chewy Apple"));
        treats.Add(new TreatItem(1, "Iron Biscuit"));
        treats.Add(new TreatItem(2, "Tasty Bone"));
        treats.Add(new TreatItem(3, "Yummy Chewstick"));
    }

    // Method to increase the quantity of a treat in player's inventory
    public void IncreaseQuantity(int treatID)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            treats[treatID].Quantity += 1;  // Always adds 1 to the player's inventory
            Debug.Log($"1 of {treats[treatID].TreatName} added to player's inventory. Current quantity: {treats[treatID].Quantity}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID.");
        }
    }

    // Method to get all available treats in the player's inventory
    public List<TreatItem> GetAllAvailableTreats()
    {
        List<TreatItem> availableTreats = new List<TreatItem>();

        foreach (TreatItem treat in treats)
        {
            if (treat.Quantity > 0) // Check if the treat is available
            {
                availableTreats.Add(treat);  // Add to the available treats list
            }
        }

        return availableTreats;  // Return the list of available treats
    }

    // Method to give a treat to a companion
    public void GiveTreatToCompanion(int companionID, int treatID)
    {
        // First check if the player owns the treat
        TreatItem treat = GetTreatById(treatID);
        if (treat != null && treat.Quantity > 0)
        {
            // Decrease treat quantity from player's inventory
            treat.Quantity--;

            // Give treat to the companion
            CompanionManager companionManager = CompanionManager.Instance;
            if (companionManager != null)
            {
                CompanionManager.Companion companion = companionManager.GetCompanionById(companionID);
                if (companion != null)
                {
                    // Increase satisfaction by a fixed amount (let's say 10 for each treat)
                    companion.IncreaseSatisfaction(10);
                    Debug.Log($"{treat.TreatName} was given to {companion.PetName}. Current satisfaction: {companion.SatisfactionLevel}");

                    // Optional: Update the UI, save state, etc.
                }
                else
                {
                    Debug.LogWarning("Companion not found.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No treat available to give.");
        }
    }

    // Method to retrieve a specific treat by its ID
    public TreatItem GetTreatById(int treatID)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            return treats[treatID];
        }
        return null;
    }
}
