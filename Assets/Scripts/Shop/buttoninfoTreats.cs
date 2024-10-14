using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoTreats : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt; // Text fields
    public Text StockTxt; // Text field for stock
    public Text QuantityTxt; // Text field for quantity owned
    public Text NameTxt;
    public Image TreatImage; // Image component to change the treat sprite
    public Sprite DefaultSprite; // Default sprite for treats
    public GameObject ShopManager;

    public ScrollRect scrollRect;

    void Start()
    {
        // Ensure the scroll view starts at the top
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f; // Sets the scroll position to the top
        }

        // Reflects the correct state upon loading the scene
        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerTreats>();
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);
        var treatManager = shopManagerScript.treatManager;

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();

            if (ItemID < treatManager.treats.Count) // Ensure index is valid
            {
                var treat = treatManager.treats[ItemID];

                if (treat != null)
                {
                    NameTxt.text = treat.TreatName;

                    StockTxt.text = $"Stock: {selectedItem.stock}"; // Show available stock

                    // Show the quantity of treats owned by the player
                    QuantityTxt.text = $"Quantity: {treat.Quantity}"; // Ensure this is updated

                    // Always display the default treat sprite
                    TreatImage.sprite = DefaultSprite; 
                }
            }
        }
    }
}
