using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerMisc : MonoBehaviour
{
    [System.Serializable]
    public class ShopMisc
    {
        public int ID;
        public float price;
        public bool bought;

        public ShopMisc(int id, float price)
        {
            this.ID = id;
            this.price = price;
            this.bought = false;
        }
    }

    public List<ShopMisc> shopItems = new List<ShopMisc>();
    public Text CoinsTxt;
    public MiscManager miscManager;  // Reference to MiscManager
    public CoinManager coinManager;  // Reference to CoinManager

    void Awake() {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
        }

        if (miscManager == null)
        {
            miscManager = FindObjectOfType<MiscManager>();
        }
    }

    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f); // Slight delay before trying to find the objects
        coinManager = FindObjectOfType<CoinManager>();
        miscManager = FindObjectOfType<MiscManager>();
        UpdateCoinDisplay();
        SyncShopItemsWithMiscManager();  // Ensure correct item state
    }

    void Start()
    {
        StartCoroutine(DelayedInitialization());

        // Add shop items with ID and Price
        shopItems.Add(new ShopMisc(0, 15));
        shopItems.Add(new ShopMisc(1, 25));
        shopItems.Add(new ShopMisc(2, 50));
        shopItems.Add(new ShopMisc(3, 30));
        shopItems.Add(new ShopMisc(4, 60));
        shopItems.Add(new ShopMisc(5, 40));

        // Sync all button states to ensure they reflect the correct bought status
        RefreshButtonStates();
    }

    private void UpdateCoinDisplay()
    {
        CoinsTxt.text = "Coins: " + coinManager.TotalCoins; // Synchronize with CoinManager
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoMisc>().ItemID;

        ShopMisc selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coinManager.TotalCoins >= selectedItem.price && !selectedItem.bought)
        {
            // Deduct the price using the new method in CoinManager
            coinManager.DeductCoins(selectedItem.price);
            selectedItem.bought = true;

            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoMisc>().BoughtTxt.text = "Owned";

            // Sync with MiscManager: Mark the item as bought
            miscManager.SetMiscItemBought(itemID);

            // Refresh button state for this item
            ButtonRef.GetComponent<buttoninfoMisc>().UpdateButtonState();
        }
        else
        {
            Debug.Log("Not enough coins or item already bought.");
        }
    }

    // Method to refresh button states across all buttons in the scene
    private void RefreshButtonStates()
    {
        foreach (var button in FindObjectsOfType<buttoninfoMisc>())
        {
            button.UpdateButtonState();
        }
    }

    // Sync the bought status of ShopMisc with MiscManager on scene load
    private void SyncShopItemsWithMiscManager()
    {
        foreach (var shopMisc in shopItems)
        {
            shopMisc.bought = miscManager.IsMiscItemBought(shopMisc.ID);
        }
        RefreshButtonStates();
    }
}
