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
    public TreatScript treatScript;    // Reference to TreatScript

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

        if (treatScript == null)
        {
            treatScript = FindObjectOfType<TreatScript>();
        }
    }

    IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(0.1f); // Slight delay before trying to find the objects
        coinManager = FindObjectOfType<CoinManager>();
        treatManager = FindObjectOfType<TreatManager>();
        treatScript = FindObjectOfType<TreatScript>();
        UpdateCoinDisplay();
        SyncShopItemsWithTreatManager();
    }

    void Start()
    {
        StartCoroutine(DelayedInitialization());

        // Add shop items with ID, Price, and Stock
        shopItems.Add(new ShopTreats(0, 20, 10));
        shopItems.Add(new ShopTreats(1, 35, 5));
        shopItems.Add(new ShopTreats(2, 50, 8));
        shopItems.Add(new ShopTreats(3, 15, 12));

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

            // Add the treats to TreatScript (use itemID as treat amount for now)
            treatScript.AddTreats(1);  // Add 1 treat for each purchase (adjust as necessary)

            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoTreats>().UpdateButtonState();
        }
        else
        {
            Debug.Log("Not enough coins or item out of stock.");
        }
    }

    private void RefreshButtonStates()
    {
        foreach (var button in FindObjectsOfType<buttoninfoTreats>())
        {
            button.UpdateButtonState();
        }
    }

    private void SyncShopItemsWithTreatManager()
    {
        foreach (var shopTreat in shopItems)
        {
            var treatItem = treatManager.GetTreatById(shopTreat.ID);
            if (treatItem != null)
            {
                shopTreat.stock = treatItem.Stock;
            }
        }
        RefreshButtonStates();
    }
}
