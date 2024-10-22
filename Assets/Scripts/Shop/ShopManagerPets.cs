using System.Collections;
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

    void Awake() {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
        }

        if (companionManager == null)
        {
            companionManager = FindObjectOfType<CompanionManager>();
        }
    }

    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f); // Slight delay before trying to find the objects
        coinManager = FindObjectOfType<CoinManager>();
        companionManager = FindObjectOfType<CompanionManager>();
        UpdateCoinDisplay();
        SyncShopItemsWithCompanionManager();  // New function call to ensure correct item state
    }

     void Start()
    {
        StartCoroutine(DelayedInitialization());
        coinManager.LoadCoins();

        // Add shop items with ID and Price
        shopItems.Add(new ShopPets(4, 50));
        shopItems.Add(new ShopPets(5, 73));
        shopItems.Add(new ShopPets(6, 873));
        shopItems.Add(new ShopPets(7, 69));
        shopItems.Add(new ShopPets(8, 573));
        shopItems.Add(new ShopPets(9, 699));
        shopItems.Add(new ShopPets(10, 1));
        shopItems.Add(new ShopPets(11, 50));
        shopItems.Add(new ShopPets(12, 40));
        shopItems.Add(new ShopPets(13, 999));
        shopItems.Add(new ShopPets(14, 510));

        // Sync all button states to ensure they reflect the correct bought status
        RefreshButtonStates();
    }

    private void UpdateCoinDisplay()
    {
        CoinsTxt.text = " " + coinManager.TotalCoins; // Synchronize with CoinManager
    }

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
            coinManager.SaveCoins(); // Save the new total


            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoPets>().BoughtTxt.text = "Owned";

            // Sync with CompanionManager: Mark the pet as bought
            companionManager.SetCompanionBought(itemID);

            // Refresh button state for this item
            ButtonRef.GetComponent<buttoninfoPets>().UpdateButtonState();
        }
        else
        {
            Debug.Log("Not enough coins or item already bought.");
        }
    }

    // Method to refresh button states across all buttons in the scene
    private void RefreshButtonStates()
    {
        foreach (var button in FindObjectsOfType<buttoninfoPets>())
        {
            button.UpdateButtonState();
        }
    }

    // Sync the bought status of ShopPets with CompanionManager on scene load
    private void SyncShopItemsWithCompanionManager()
    {
        foreach (var shopPet in shopItems)
        {
            shopPet.bought = companionManager.IsCompanionBought(shopPet.ID);
        }
        RefreshButtonStates();
    }

    
}
