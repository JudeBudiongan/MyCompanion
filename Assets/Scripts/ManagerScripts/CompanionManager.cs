using System;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    [System.Serializable]
    public class Pet
    {
        public bool IsBought { get; set; }
        public string Author { get; set; } // Author of the sprite

        public Pet(bool isBought, string author)
        {
            IsBought = isBought;
            Author = author;
        }
    }

    [System.Serializable]
    public class Companion : Pet
    {
        public int CompanionID { get; set; } // Unique ID for the companion
        public string PetName { get; set; }
        public Sprite CompanionSprite { get; set; } // Add Sprite for the companion

        // Satisfaction and Level properties
        public int SatisfactionLevel { get; set; }
        public int Level { get; private set; }

        // Emotional sprites
        public Sprite AngrySprite { get; set; }
        public Sprite SadSprite { get; set; }
        public Sprite NormalSprite { get; set; }
        public Sprite HappySprite { get; set; }

        public Companion(int companionID, string petName, Sprite sprite, string author)
            : base(false, author) // Initialized isBought and author here
        {
            CompanionID = companionID;
            PetName = petName;
            CompanionSprite = sprite; // Assign the sprite
            SatisfactionLevel = 50; // Default initial satisfaction
            Level = 1;              // Default initial level
        }

        public void LevelUp(CoinManager coinManager)
        {
            Level++;
            ResetSatisfaction();

            // Earn coins for leveling up
            coinManager.EarnCoinsForLevelUp(this);

            Debug.Log($"Hooray! Companion leveled up to {Level}!");
        }

        public void LevelDown()
        {
            Level--;
            SatisfactionLevel = 50;

            Debug.Log($"Ouch! Companion leveled down to {Level}!");
        }

        public void IncreaseSatisfaction(int amount)
        {
            SatisfactionLevel += amount;
            if (SatisfactionLevel > 100)
            {
                SatisfactionLevel = 100;
            }
        }

        public void DecreaseSatisfaction(int amount)
        {
            if (amount > SatisfactionLevel)
            {
                LevelDown();
            }
            else
            {
                SatisfactionLevel -= amount;
                if (SatisfactionLevel < 0)
                {
                    SatisfactionLevel = 0;
                }
            }
        }

        private void ResetSatisfaction()
        {
            SatisfactionLevel = 100; // Reset to full satisfaction on level up
            Debug.Log("Satisfaction has been reset to 100.");
        }
    }

     public static CompanionManager Instance;
     public event Action OnCompanionAdded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("CompanionManager instance created.");
        }
        else
        {
            Debug.Log("Duplicate CompanionManager instance destroyed.");
            Destroy(gameObject);
        }
    }


    public List<Companion> companions = new List<Companion>();
    public int NumberOfPets {get; private set; } = 0;
    
    public void IncreaseNumberOfPets() {
        NumberOfPets++;
        Debug.Log($"Number of pets increased.");
    }

    // Sprites to be set in the Inspector
    public Sprite spriteAlien, spriteBerry, spriteGrey, spriteWoshi;
    public Sprite spriteGrimWooper, spriteFak, spriteXv6Riscv, spriteTTiddy;
    public Sprite spritePriscue, spriteSushiSlayer, spriteRFilly, spriteEilmar;
    public Sprite spriteCat, spriteSkibidi, spritelileduj, spriteBing;

    // Emotional sprites to be set in the Inspector
    public Sprite spriteAlienAngry, spriteAlienSad, spriteAlienNormal, spriteAlienHappy;
    public Sprite spriteBerryAngry, spriteBerrySad, spriteBerryNormal, spriteBerryHappy;
    public Sprite spriteGreyAngry, spriteGreySad, spriteGreyNormal, spriteGreyHappy;
    public Sprite spriteWoshiAngry, spriteWoshiSad, spriteWoshiNormal, spriteWoshiHappy;
    public Sprite spriteGrimWooperAngry, spriteGrimWooperSad, spriteGrimWooperNormal, spriteGrimWooperHappy;
    public Sprite spriteFakAngry, spriteFakSad, spriteFakNormal, spriteFakHappy;
    public Sprite spriteXv6RiscvAngry, spriteXv6RiscvSad, spriteXv6RiscvNormal, spriteXv6RiscvHappy;
    public Sprite spriteTTiddyAngry, spriteTTiddySad, spriteTTiddyNormal, spriteTTiddyHappy;
    public Sprite spritePriscueAngry, spritePriscueSad, spritePriscueNormal, spritePriscueHappy;
    public Sprite spriteSushiSlayerAngry, spriteSushiSlayerSad, spriteSushiSlayerNormal, spriteSushiSlayerHappy;
    public Sprite spriteRFillyAngry, spriteRFillySad, spriteRFillyNormal, spriteRFillyHappy;
    public Sprite spriteEilmarAngry, spriteEilmarSad, spriteEilmarNormal, spriteEilmarHappy;
    public Sprite spriteCatAngry, spriteCatSad, spriteCatNormal, spriteCatHappy;
    public Sprite spriteSkibidiAngry, spriteSkibidiSad, spriteSkibidiNormal, spriteSkibidiHappy;
    public Sprite spriteliledujAngry, spriteliledujSad, spriteliledujNormal, spriteliledujHappy;

    void Start()
    {
        // STARTER COMPANIONS
        companions.Add(new Companion(0, "Alien", spriteAlien, "DD"));
        companions.Add(new Companion(1, "Berry", spriteBerry, "JB"));
        companions.Add(new Companion(2, "Grey", spriteGrey, "ED"));
        companions.Add(new Companion(3, "Woshi", spriteWoshi, "JG"));

        // SHOP COMPANIONS
        companions.Add(new Companion(4, "Grim-Wooper", spriteGrimWooper, "JB"));
        companions.Add(new Companion(5, "Fak", spriteFak, "KB"));
        companions.Add(new Companion(6, "xv6-riscv", spriteXv6Riscv, "KR"));
        companions.Add(new Companion(7, "T-Tiddy", spriteTTiddy, "AS"));
        companions.Add(new Companion(8, "Priscue", spritePriscue, "FM"));
        companions.Add(new Companion(9, "Sushi-Slayer", spriteSushiSlayer, "JZ"));
        companions.Add(new Companion(10, "R-Filly", spriteRFilly, "AB"));
        companions.Add(new Companion(11, "Eilmar", spriteEilmar, "ES"));
        companions.Add(new Companion(12, "cat", spriteCat, "JB"));
        companions.Add(new Companion(13, "skibidi", spriteSkibidi, "KR"));
        companions.Add(new Companion(14, "lil e-duj", spritelileduj, "DA"));
        companions.Add(new Companion(15, "Bing", spriteBing, "JG"));

        // Load satisfaction levels from saved data
        foreach (var companion in companions)
        {
            LoadCompanionData(companion);
        }

        // Set emotional sprites for each companion
        SetCompanionEmotionalSprites();
    }

    // New method to set emotional sprites for each companion
    private void SetCompanionEmotionalSprites()
    {
        // Assign emotional sprites for companions
        SetCompanionEmotionalSprites(0, spriteAlienAngry, spriteAlienSad, spriteAlienNormal, spriteAlienHappy);
        SetCompanionEmotionalSprites(1, spriteBerryAngry, spriteBerrySad, spriteBerryNormal, spriteBerryHappy);
        SetCompanionEmotionalSprites(2, spriteGreyAngry, spriteGreySad, spriteGreyNormal, spriteGreyHappy);
        SetCompanionEmotionalSprites(3, spriteWoshiAngry, spriteWoshiSad, spriteWoshiNormal, spriteWoshiHappy);
        SetCompanionEmotionalSprites(4, spriteGrimWooperAngry, spriteGrimWooperSad, spriteGrimWooperNormal, spriteGrimWooperHappy);
        SetCompanionEmotionalSprites(5, spriteFakAngry, spriteFakSad, spriteFakNormal, spriteFakHappy);
        SetCompanionEmotionalSprites(6, spriteXv6RiscvAngry, spriteXv6RiscvSad, spriteXv6RiscvNormal, spriteXv6RiscvHappy);
        SetCompanionEmotionalSprites(7, spriteTTiddyAngry, spriteTTiddySad, spriteTTiddyNormal, spriteTTiddyHappy);
        SetCompanionEmotionalSprites(8, spritePriscueAngry, spritePriscueSad, spritePriscueNormal, spritePriscueHappy);
        SetCompanionEmotionalSprites(9, spriteSushiSlayerAngry, spriteSushiSlayerSad, spriteSushiSlayerNormal, spriteSushiSlayerHappy);
        SetCompanionEmotionalSprites(10, spriteRFillyAngry, spriteRFillySad, spriteRFillyNormal, spriteRFillyHappy);
        SetCompanionEmotionalSprites(11, spriteEilmarAngry, spriteEilmarSad, spriteEilmarNormal, spriteEilmarHappy);
        SetCompanionEmotionalSprites(12, spriteCatAngry, spriteCatSad, spriteCatNormal, spriteCatHappy);
        SetCompanionEmotionalSprites(13, spriteSkibidiAngry, spriteSkibidiSad, spriteSkibidiNormal, spriteSkibidiHappy);
        SetCompanionEmotionalSprites(14, spriteliledujAngry, spriteliledujSad, spriteliledujNormal, spriteliledujHappy);
    }

    // Helper method to set emotional sprites
    private void SetCompanionEmotionalSprites(int companionID, Sprite angry, Sprite sad, Sprite normal, Sprite happy)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            companions[companionID].AngrySprite = angry;
            companions[companionID].SadSprite = sad;
            companions[companionID].NormalSprite = normal;
            companions[companionID].HappySprite = happy;
        }
    }

    public void SetCompanionBought(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            companions[companionID].IsBought = true;
            IncreaseNumberOfPets(); // increase number of pets by 1
            Debug.Log($"{companions[companionID].PetName} has been marked as bought.");
            PlayerPrefs.SetInt("Companion_" + companionID, 1); // Save as bought
            PlayerPrefs.Save();

            // Trigger event when a companion is bought
           TriggerCompanionAddedEvent();
        }
        else
        {
            Debug.LogWarning("Invalid companion ID.");
        }
    }

     public Companion GetCompanionById(int companionID)
    {
        return companions.Find(c => c.CompanionID == companionID);
    }

    // Add this method to manually trigger the event
    public void TriggerCompanionAddedEvent()
    {
        OnCompanionAdded?.Invoke();
    }

    public bool IsCompanionBought(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            return companions[companionID].IsBought;
        }
        return false; // Return false if the ID is invalid
    }

    public HealthBar healthBar; // Reference to the HealthBar component

    public void UpdateHealthBarForSelectedCompanion(int companionID)
    {
        Companion selectedCompanion = GetCompanionById(companionID);
        if (selectedCompanion != null)
        {
            healthBar.SetMaxSatisfaction(100);
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);
        }
        else
        {
            Debug.LogWarning("Selected Companion is null.");
        }
    }

    public void SaveCompanionData(Companion companion)
    {
        PlayerPrefs.SetInt("Satisfaction_" + companion.CompanionID, companion.SatisfactionLevel);
        PlayerPrefs.Save();
    }

    public void LoadCompanionData(Companion companion)
    {
        companion.SatisfactionLevel = PlayerPrefs.GetInt("Satisfaction_" + companion.CompanionID, 50);
    }

    // New method to count the number of bought companions
    public int GetBoughtCompanionCount()
    {
        int boughtCount = 0;
        foreach (var companion in companions)
        {
            if (companion.IsBought)
            {
                boughtCount++;
            }
        }
        return boughtCount;
    }
}
