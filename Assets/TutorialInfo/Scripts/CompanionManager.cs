using System.Collections.Generic;
using UnityEngine;

public class Pet
{
    public int SatisfactionLevel { get; set; }
    public int Level { get; set; }
    public bool IsBought { get; set; }
    public string Author { get; set; }  // Author of the sprite

    public Pet(int satisfactionLevel, int level, bool isBought, string author)
    {
        SatisfactionLevel = satisfactionLevel;
        Level = level;
        IsBought = isBought;
        Author = author;
    }
}

public class Companion : Pet
{
    public string PetName { get; set; }

    public Companion(string petName, string author)
        : base(50, 1, false, author)
    {
        PetName = petName;
    }
}

public class CompanionManager : MonoBehaviour
{
    public List<Companion> companions = new List<Companion>();

    void Start()
    {
        // Add companions to the list
        companions.Add(new Companion("Grim-Wooper", "Author A"));
        companions.Add(new Companion("Fak", "Author B"));
        companions.Add(new Companion("xv6-riscv", "Author C"));
        companions.Add(new Companion("T-Tiddy", "Author D"));
        companions.Add(new Companion("Priscue", "Author E"));
        companions.Add(new Companion("Sushi-Slayer", "Author F"));
        companions.Add(new Companion("R-Filly", "Author G"));
        companions.Add(new Companion("Alien", "Author H"));
        companions.Add(new Companion("Cat", "Author I"));
    }

    public void SetCompanionBought(int companionID)
    {
        if (companionID >= 0 && companionID < companions.Count)
        {
            companions[companionID].IsBought = true;
        }
    }
}
