using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerTreats : MonoBehaviour
{
    [System.Serializable]
    public class ShopTreats
    {
        public int ID;
        public float price;
        public int stock; // Stock available in the shop

        public ShopTreats(int id, float price, int stock)
        {
            this.ID = id;
            this.price = price;
            this.stock = stock;
        }
    }

    public List<ShopTreats> shopItems = new List<ShopTreats>();
    public Text CoinsTxt;
    public TreatManager treatManager;  // Reference to TreatManager
    public CoinManager coinManager;    // Reference to CoinManager

    void Awake()
    {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
        }

        if (treatManager == null)
        {
            treatManager = FindObjectOfType<TreatManager>();
        }
    }

    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f); // Slight delay before trying to find the objects
        coinManager = FindObjectOfType<CoinManager>();
        treatManager = FindObjectOfType<TreatManager>();
        UpdateCoinDisplay();
        SyncShopItemsWithTreatManager();  // Call the correct sync method
    }

    void Start()
    {
        StartCoroutine(DelayedInitialization());

        // Add shop items with ID, Price, and Stock
        shopItems.Add(new ShopTreats(0, 20, 10));
        shopItems.Add(new ShopTreats(1, 35, 5));
        shopItems.Add(new ShopTreats(2, 50, 8));
        shopItems.Add(new ShopTreats(3, 15, 12));

        // Sync all button states to ensure they reflect the correct stock status
        RefreshButtonStates();
    }

    private void UpdateCoinDisplay()
    {
        CoinsTxt.text = "Coins: " + coinManager.TotalCoins; // Synchronize with CoinManager
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoTreats>().ItemID;

        ShopTreats selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coinManager.TotalCoins >= selectedItem.price && selectedItem.stock > 0)
        {
            // Deduct the price using the new method in CoinManager
            coinManager.DeductCoins(selectedItem.price);
            selectedItem.stock--;

            // Increase the treat quantity in TreatManager
            treatManager.IncreaseQuantity(itemID);

            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoTreats>().UpdateButtonState();
        }
        else
        {
            Debug.Log("Not enough coins or item out of stock.");
        }
    }

    // Method to refresh button states across all buttons in the scene
    private void RefreshButtonStates()
    {
        foreach (var button in FindObjectsOfType<buttoninfoTreats>())
        {
            button.UpdateButtonState();
        }
    }

    // Sync the stock status of ShopTreats with TreatManager on scene load
    private void SyncShopItemsWithTreatManager()
    {
        foreach (var shopTreat in shopItems)
        {
            var treatItem = treatManager.GetTreatById(shopTreat.ID);
            if (treatItem != null) // Check if the treatItem is found
            {
                shopTreat.stock = treatItem.Stock; // Correctly sync stock from TreatManager
            }
        }
        RefreshButtonStates();
    }
}
