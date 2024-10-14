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
    public GameObject itemPrefab;              // Drag your prefab into this variable in the inspector
    public Transform itemsParent;               // Parent transform to hold instantiated items
    public static ShopManagerPets Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadPetData(); // Load previously saved pet data
        UpdateCoinDisplay(); // Set initial coin display from CoinManager
        AddShopItems(); // Add items to the shop
        PopulateShopUI(); // Populate the UI with items
    }

    // Method to add shop items
    private void AddShopItems()
    {
        shopItems.Add(new ShopPets(4, 50));  // Grim-Wooper
        shopItems.Add(new ShopPets(5, 73));  // Fak
        shopItems.Add(new ShopPets(6, 873)); // xv6-riscv
        shopItems.Add(new ShopPets(7, 69));  // T-Tiddy
        shopItems.Add(new ShopPets(8, 573)); // Priscue
        shopItems.Add(new ShopPets(9, 699)); // Sushi-Slayer
        shopItems.Add(new ShopPets(10, 1));   // R-Filly
        shopItems.Add(new ShopPets(11, 30));  // Eilmar
    }

    // Method to populate the shop UI with items
    private void PopulateShopUI()
    {
        foreach (var shopItem in shopItems)
        {
            GameObject itemInstance = Instantiate(itemPrefab, itemsParent); // Instantiate item prefab
            var buttonInfo = itemInstance.GetComponent<buttoninfoPets>(); // Get buttoninfoPets script from the instantiated item
            buttonInfo.ItemID = shopItem.ID; // Set the ItemID
            buttonInfo.UpdateUI(); // Call UpdateUI to initialize the item details
        }
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

            // Save pet data
            SavePetData();

            // Update UI
            UpdateCoinDisplay();
            ButtonRef.GetComponent<buttoninfoPets>().UpdateUI(); // Update the button info UI
            companionManager.SetCompanionBought(itemID); // Sync with CompanionManager
        }
        else
        {
            Debug.Log("Not enough coins or item already bought.");
        }
    }

    // Method to save pet data
    private void SavePetData()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            PlayerPrefs.SetInt("Pet_" + shopItems[i].ID, shopItems[i].bought ? 1 : 0);
        }
        PlayerPrefs.Save(); // Ensure the data is saved
    }

    // Method to load pet data
    private void LoadPetData()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            int bought = PlayerPrefs.GetInt("Pet_" + shopItems[i].ID, 0);
            shopItems[i].bought = bought == 1;
        }
    }
}
