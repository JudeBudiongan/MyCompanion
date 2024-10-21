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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances 
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
    public Sprite spriteCat, spriteSkibidi, spritelileduj;

    void Start()
    {
        // STARTER COMPANIONS
        companions.Add(new Companion(0, "Alien", spriteAlien, "DD"));
        companions.Add(new Companion(1, "Berry", spriteBerry, "JB"));
        companions.Add(new Companion(2, "Grey", spriteGrey, "ED")); // Clarified naming
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

        // Load satisfaction levels from saved data
        foreach (var companion in companions)
        {
            LoadCompanionData(companion);
        }
    }

    public void SetCompanionBought(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            companions[companionID].IsBought = true;
            IncreaseNumberOfPets(); // increase number of pets by 1
            Debug.Log($"{companions[companionID].PetName} has been marked as bought.");
        }
        else
        {
            Debug.LogWarning("Invalid companion ID.");
        }
    }

    public Companion GetCompanionById(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            return companions[companionID];
        }
        return null;
    }

    // New method to check if a companion is bought
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
            healthBar.SetMaxSatisfaction(100); // Assuming max satisfaction is 100
            healthBar.SetSatisfaction(selectedCompanion.SatisfactionLevel);
        }
        else
        {
            Debug.LogWarning("Selected Companion is null.");
        }
    }

    // New Methods to Save and Load Satisfaction Data
    public void SaveCompanionData(Companion companion)
    {
        PlayerPrefs.SetInt("Satisfaction_" + companion.CompanionID, companion.SatisfactionLevel);
        PlayerPrefs.Save();
    }

    public void LoadCompanionData(Companion companion)
    {
        companion.SatisfactionLevel = PlayerPrefs.GetInt("Satisfaction_" + companion.CompanionID, 50); // Default to 50 if not set
    }
}
