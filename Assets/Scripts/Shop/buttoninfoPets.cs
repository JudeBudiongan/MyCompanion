using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoninfoPets : MonoBehaviour
{
    public int ItemID; 
    public Text PriceTxt; 
    public Text BoughtTxt;
    public Text NameTxt;
    public Text AuthorTxt; 
    public Image PetImage; 
    public Sprite NormalSprite; 
    public Sprite HappySprite; 
    public GameObject ShopManager; 
    public ScrollRect scrollRect;

    private float bounceSpeedIncrease = 5f; 
    private bool hasAccelerated = false; 

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
    }

    void Start()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f; 
        }
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        var shopManagerScript = ShopManager.GetComponent<ShopManagerPets>();
        var selectedItem = shopManagerScript.shopItems.Find(item => item.ID == ItemID);
        var companionManager = shopManagerScript.companionManager;

        if (selectedItem != null)
        {
            PriceTxt.text = "Price: $" + selectedItem.price.ToString();

            if (ItemID < companionManager.companions.Count)
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

                        if (!hasAccelerated)
                        {
                            float currentBobSpeed = PetImage.GetComponent<BobUpDownUI>().bobSpeed;
                            PetImage.GetComponent<BobUpDownUI>().bobSpeed = currentBobSpeed * bounceSpeedIncrease;
                            hasAccelerated = true;
                        }
                    }
                    else
                    {
                        BoughtTxt.text = "Not Owned";
                        PetImage.sprite = NormalSprite;
                    }
                }
            }
        }
    }
}
