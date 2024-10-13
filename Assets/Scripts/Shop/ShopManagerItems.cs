using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerItems : MonoBehaviour
{
    // Define a class to hold item information
    [System.Serializable]
    public class ShopItem
    {
        public int ID;
        public float price;
        public int quantity;
        // public int stock; // Keeping stock info in comments for future use

        public ShopItem(int id, float price /*, int stock */)
        {
            this.ID = id;
            this.price = price;
            this.quantity = 0;
            // this.stock = stock; // For future stock management
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
        shopItems.Add(new ShopItem(1, 10 /*, 10 */)); 
        shopItems.Add(new ShopItem(2, 20 /*, 10 */)); 
        shopItems.Add(new ShopItem(3, 30 /*, 10 */)); 
        shopItems.Add(new ShopItem(4, 40 /*, 10 */)); 
        shopItems.Add(new ShopItem(5, 40 /*, 10 */));
        shopItems.Add(new ShopItem(6, 40 /*, 10 */)); 
    }

    // Buy method
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        var itemID = ButtonRef.GetComponent<buttoninfoPets>().ItemID;

        // Find the item in the shopItems list based on ID
        ShopItem selectedItem = shopItems.Find(item => item.ID == itemID);

        if (selectedItem != null && coins >= selectedItem.price /* && selectedItem.stock > 0 */)
        {
            coins -= selectedItem.price;
            selectedItem.quantity++;
            // selectedItem.stock--; // For future stock management

            // Update UI
            CoinsTxt.text = "Coins: " + coins;
          //  ButtonRef.GetComponent<buttoninfoPets>().QuantityTxt.text = "Quantity: " + selectedItem.quantity.ToString();
            // ButtonRef.GetComponent<buttoninfo>().StockTxt.text = "Stock: " + selectedItem.stock.ToString(); // Future stock
        }
        else
        {
            Debug.Log("Not enough stock or coins.");
        }
    }
}

