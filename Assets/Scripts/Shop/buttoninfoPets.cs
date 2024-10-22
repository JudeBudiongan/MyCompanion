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

        // Reflects the correct state upon loading the scene
        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerPets>();
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);
        var companionManager = shopManagerScript.companionManager;

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();

            if (ItemID < companionManager.companions.Count) // Ensure index is valid
            {
                var companion = companionManager.companions[ItemID];

                if (companion != null)
                {
                    NameTxt.text = companion.PetName;
                    AuthorTxt.text = companion.Author;

                    if (selectedItem.bought)
                    {
                        BoughtTxt.text = "Owned";
                        PetImage.sprite = HappySprite;

                        // Increase bounce speed once
                        if (!hasAccelerated)
                        {
                            float currentBobSpeed = PetImage.GetComponent<BobUpDownUI>().bobSpeed;
                            PetImage.GetComponent<BobUpDownUI>().bobSpeed = currentBobSpeed * bounceSpeedIncrease;
                            hasAccelerated = true;
                        }
                    }
                    else
                    {
                        BoughtTxt.text = " ";
                        PetImage.sprite = NormalSprite;
                    }
                }
            }
        }
    }
}
