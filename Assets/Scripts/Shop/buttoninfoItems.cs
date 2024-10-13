using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoItems : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt;
    public Text QuantityTxt;
    // public Text StockTxt; // Stock text for future use
    public GameObject ShopManager;

    // Update is called once per frame
    void Update()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerItems>();

        // Find the corresponding item based on ItemID
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();
            QuantityTxt.text = "Quantity: " + selectedItem.quantity.ToString();
            // StockTxt.text = "Stock: " + selectedItem.stock.ToString(); // Future stock
        }
    }
}

