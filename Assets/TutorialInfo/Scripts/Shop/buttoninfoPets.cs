using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoPets : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt; // Text fields
    public Text BoughtTxt;
    public Text NameTxt;
    public Text AuthorTxt; 
    public Image PetImage; // Image component to change the pet sprite
    public Sprite NormalSprite; // Normal pet sprite
    public Sprite HappySprite; // Happy pet sprite after purchase
    public GameObject ShopManager;
    

    public ScrollRect scrollRect;

    private float bounceSpeedIncrease = 5f; // Multiplier to increase bounce speed after purchase
    private bool hasAccelerated = false; // Track if bounce speed has been increased

    void Start()
    {
        // Ensure the scroll view starts at the top
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f; // Sets the scroll position to the top
        }
    }

    private void Update()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerPets>();
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();
            NameTxt.text = selectedItem.itemName;

            if (selectedItem.bought)
            {
                BoughtTxt.text = "Owned";
                PetImage.sprite = HappySprite; // Change to happy sprite
                
                // Increase bounce speed once
                if (!hasAccelerated)
                {
                    float currentBobSpeed = PetImage.GetComponent<BobUpDownUI>().bobSpeed;
                    PetImage.GetComponent<BobUpDownUI>().bobSpeed = currentBobSpeed * bounceSpeedIncrease;
                    hasAccelerated = true; // Set flag to true to prevent further increases
                }
            }
            else
            {
                BoughtTxt.text = "Not Owned";
                PetImage.sprite = NormalSprite; // Normal sprite
            }
        }
    }
}
