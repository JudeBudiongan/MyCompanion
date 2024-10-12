using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerPets : MonoBehaviour
{
    [System.Serializable]
    public class ShopPets
    {
        public int ID;
        public float price;
        public bool bought;

        public ShopPets(int id, float price)
        {
            this.ID = id;
            this.price = price;
            this.bought = false;
        }
    }

    public List<ShopPets> shopItems = new List<ShopPets>();
    public Text CoinsTxt;
    public CompanionManager companionManager;  // Reference to CompanionManager
    public CoinManager coinManager;            // Reference to CoinManager

    void Start()
    {
        // Set initial coin display from CoinManager
        UpdateCoinDisplay();

        // Add shop items with ID and Price
        shopItems.Add(new ShopPets(4, 50));  // Grim-Wooper
        shopItems.Add(new ShopPets(5, 73));  // Fak
        shopItems.Add(new ShopPets(6, 873)); // xv6-riscv
        shopItems.Add(new ShopPets(7, 69));  // T-Tiddy
        shopItems.Add(new ShopPets(8, 573)); // Priscue
        shopItems.Add(new ShopPets(9, 699)); // Sushi-Slayer
        shopItems.Add(new ShopPets(10, 1));   // R-Filly
        shopItems.Add(new ShopPets(11, 30));  // Eilmar
    }

    // Method to update coin display
    private void UpdateCoinDisplay()
    {
        CoinsTxt.text = "Coins: " + coinManager.TotalCoins; // Synchronize with CoinManager
    }

    // Method to handle buying a pet
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoPets>().ItemID;

        ShopPets selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coinManager.TotalCoins >= selectedItem.price && !selectedItem.bought)
        {
            // Deduct the price using the new method in CoinManager
            coinManager.DeductCoins(selectedItem.price);
            selectedItem.bought = true;

            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoPets>().BoughtTxt.text = "Owned";

            // Sync with CompanionManager: Mark the pet as bought
            companionManager.SetCompanionBought(itemID);
        }
        else
        {
            Debug.Log("Not enough coins or item already bought.");
        }
    }
}
