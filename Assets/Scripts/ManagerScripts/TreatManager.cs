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

    // Method to increase the stock of a treat
    public void IncreaseTreatStock(int treatID, int amount)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            treats[treatID].Stock += amount;  // Increase stock
            Debug.Log($"{amount} of {treats[treatID].TreatName} added to shop stock. Current stock: {treats[treatID].Stock}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID.");
        }
    }

    // Method to decrease the stock of a treat after purchase/use
    public void DecreaseTreatStock(int treatID, int amount)
    {
        if (treatID >= 0 && treatID < treats.Count && treats[treatID].Stock >= amount)
        {
            treats[treatID].Stock -= amount;  // Decrease stock
            Debug.Log($"{amount} of {treats[treatID].TreatName} used. Current stock: {treats[treatID].Stock}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID or insufficient stock.");
        }
    }

    // Method to increase player's inventory quantity when a treat is bought
    public void IncreaseQuantity(int treatID)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            // We want to add only 1 treat to the inventory when bought
            treats[treatID].Quantity += 1;  // Always adds 1 to the player's inventory
            Debug.Log($"1 of {treats[treatID].TreatName} added to player's inventory. Current quantity: {treats[treatID].Quantity}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID.");
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
}
