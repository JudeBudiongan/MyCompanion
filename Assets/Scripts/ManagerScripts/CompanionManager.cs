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
        public int SatisfactionLevel { get; private set; }
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
            SatisfactionLevel -= amount;
            if (SatisfactionLevel < 0)
            {
                SatisfactionLevel = 0;
            }
        }

        private void ResetSatisfaction()
        {
            SatisfactionLevel = 100; // Reset to full satisfaction on level up
            Debug.Log("Satisfaction has been reset to 100.");
        }
    }

    public static CompanionManager Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // Destroy duplicate instances 
        }
    }

    public List<Companion> companions = new List<Companion>();

    // Sprites to be set in the Inspector
    public Sprite spriteAlien, spriteBerry, spriteGrey, spriteWoshi;
    public Sprite spriteGrimWooper, spriteFak, spriteXv6Riscv, spriteTTiddy;
    public Sprite spritePriscue, spriteSushiSlayer, spriteRFilly, spriteEilmar;
    public Sprite spriteSkibidi;

    void Start()
    {
        // STARTER COMPANIONS
        companions.Add(new Companion(0, "Alien", spriteAlien, "DD"));
        companions.Add(new Companion(1, "Berry", spriteBerry, "JB"));
        companions.Add(new Companion(2, "Woshi", spriteGrey, "ED"));
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

        // SPECIAL REWARD COMPANIONS (FOR FUTURE USE)
        companions.Add(new Companion(12, "skibidi", spriteSkibidi, "KR"));
    }

    public void SetCompanionBought(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            companions[companionID].IsBought = true;
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
}
