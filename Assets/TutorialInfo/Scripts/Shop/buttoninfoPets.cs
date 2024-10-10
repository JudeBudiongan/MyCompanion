using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoPets : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt;
    public Text BoughtTxt;  // Text field for showing if the item is bought
    public GameObject ShopManager;

    // Update is called once per frame
    void Update()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerPets>();

        // Find the corresponding item based on ItemID
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();
            BoughtTxt.text = selectedItem.bought ? "Bought" : "Not Bought"; // Update BoughtTxt
        }
    }
}
