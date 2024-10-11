using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerPets : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public int ID;
        public float price;
        public bool bought;
        public string itemName;
        public string author;  // Author who created the sprite

        public ShopItem(int id, float price, string name, string author)
        {
            this.ID = id;
            this.price = price;
            this.bought = false;
            this.itemName = name;
            this.author = author;
        }
    }

    public List<ShopItem> shopItems = new List<ShopItem>();
    public float coins;
    public Text CoinsTxt;
    public CompanionManager companionManager;  // Reference to CompanionManager

    void Start()
    {
        CoinsTxt.text = "Coins: " + coins;

        // Add shop items with ID, Price, Name, and Author
        shopItems.Add(new ShopItem(1, 10, "Grim-Wooper", "Author A"));
        shopItems.Add(new ShopItem(2, 20, "Fak", "Author B"));
        shopItems.Add(new ShopItem(3, 30, "xv6-riscv", "Author C"));
        shopItems.Add(new ShopItem(4, 40, "T-Tiddy", "Author D"));
        shopItems.Add(new ShopItem(5, 10, "Priscue", "Author E"));
        shopItems.Add(new ShopItem(6, 20, "Sushi-Slayer", "Author F"));
        shopItems.Add(new ShopItem(7, 30, "R-Filly", "Author G"));
        shopItems.Add(new ShopItem(8, 40, "Alien", "Author H"));
        shopItems.Add(new ShopItem(9, 40, "Cat", "Author I"));
    }

    // Method to handle buying a pet
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoPets>().ItemID;

        ShopItem selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coins >= selectedItem.price && !selectedItem.bought)
        {
            coins -= selectedItem.price;
            selectedItem.bought = true;

            // Update UI
            CoinsTxt.text = "Coins: " + coins;
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
