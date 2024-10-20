using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoMisc : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt; // Text fields
    public Text BoughtTxt;
    public Text NameTxt;
    public Image MiscImage; // Image component to display the item sprite
    public Sprite NormalSprite; // Sprite for the item
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
        var shopManagerScript = ShopManager.GetComponent<ShopManagerMisc>();
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);
        var miscManager = shopManagerScript.miscManager;

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();

            if (selectedItem.bought)
            {
                BoughtTxt.text = "Owned";
            }
            else
            {
                BoughtTxt.text = "Not Owned";
            }

            // Sync name from MiscManager if it exists
            if (ItemID < miscManager.miscellaneousItems.Count)
            {
                var miscItem = miscManager.miscellaneousItems[ItemID];
                if (miscItem != null)
                {
                    NameTxt.text = miscItem.ItemName;
                }
            }

            // Use the same sprite for both states
            MiscImage.sprite = NormalSprite;
        }
    }
}
