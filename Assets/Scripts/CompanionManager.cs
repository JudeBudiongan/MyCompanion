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

        // Satisfaction and Level properties
        public int SatisfactionLevel { get; private set; }
        public int Level { get; private set; }

        public Companion(int companionID, string petName, string author)
            : base(false, author) // Initialized isBought and author here
        {
            CompanionID = companionID;
            PetName = petName;
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

    void Start()
    {
        // Add companions to the list with IDs, names, and authors

        // STARTER COMPANIONS 
        companions.Add(new Companion(0, "Alien", "DD")); 
        companions.Add(new Companion(1, "Grey", "ED")); 
        companions.Add(new Companion(2, "Woshi", "JG")); 
        companions.Add(new Companion(3, "Kitty", "JB")); 

        // SHOP COMPANIONS
        companions.Add(new Companion(4, "Grim-Wooper", "JB"));
        companions.Add(new Companion(5, "Fak", "KB"));
        companions.Add(new Companion(6, "xv6-riscv", "KR"));
        companions.Add(new Companion(7, "T-Tiddy", "AS"));
        companions.Add(new Companion(8, "Priscue", "FM"));
        companions.Add(new Companion(9, "Sushi-Slayer", "JZ"));
        companions.Add(new Companion(10, "R-Filly", "AB"));
        companions.Add(new Companion(11, "Eilmar", "ES")); 
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
