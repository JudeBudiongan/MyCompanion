using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerPets : MonoBehaviour
{
    // Define a class to hold item information
    [System.Serializable]
    public class ShopItem
    {
        public int ID;
        public float price;
        public bool bought;

        public ShopItem(int id, float price)
        {
            this.ID = id;
            this.price = price;
            this.bought = false;
        }
    }

    public List<ShopItem> shopItems = new List<ShopItem>();
    public float coins;
    public Text CoinsTxt;

    // Start is called before the first frame update
    void Start()
    {
        CoinsTxt.text = "Coins: " + coins;

        // Add items to the shop with ID and Price
        shopItems.Add(new ShopItem(1, 10)); 
        shopItems.Add(new ShopItem(2, 20)); 
        shopItems.Add(new ShopItem(3, 30)); 
        shopItems.Add(new ShopItem(4, 40));
        shopItems.Add(new ShopItem(5, 10)); 
        shopItems.Add(new ShopItem(6, 20)); 
        shopItems.Add(new ShopItem(7, 30)); 
        shopItems.Add(new ShopItem(8, 40)); 
        shopItems.Add(new ShopItem(9, 40)); 
    }

    // Buy method
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoPets>().ItemID;

        // Find the item in the shopItems list based on ID
        ShopItem selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coins >= selectedItem.price && !selectedItem.bought)
        {
            coins -= selectedItem.price;
            selectedItem.bought = true;

            // Update UI
            CoinsTxt.text = "Coins: " + coins;
            ButtonRef.GetComponent<buttoninfoPets>().BoughtTxt.text = "Bought"; // Update Bought status
        }
        else
        {
            Debug.Log("Not enough coins or item already bought.");
        }
    }
}
