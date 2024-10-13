/*using UnityEngine;

public class SatisfactionManager
{
    public int SatisfactionLevel { get; private set; }
    public int Level { get; private set; }

    public SatisfactionManager()
    {
        SatisfactionLevel = 50; // Default initial satisfaction
        Level = 1;              // Default initial level
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

    public void LevelUp()
    {
        Level++;
        ResetSatisfaction();
        Debug.Log($"Hooray! Companion leveled up to {Level}!");
    }

    public void ResetSatisfaction()
    {
        SatisfactionLevel = 100; // Reset to full satisfaction on level up
        Debug.Log("Satisfaction has been reset to 100.");
    }
}
*/