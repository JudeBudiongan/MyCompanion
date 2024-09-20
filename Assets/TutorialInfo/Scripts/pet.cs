using UnityEngine;

public class Pet : MonoBehaviour {
    public int Level { get; private set; }
    public int Satisfaction { get; private set; }

    // This method is called when the script starts
    void Start() {
        // Initialize the pet's level and satisfaction
        Level = 1;
        Satisfaction = 100;

        // Test: Decrease satisfaction and print it
        DecreaseSatisfaction(25); // Satisfaction drops to 75%
        Debug.Log($"Pet Satisfaction: {Satisfaction}%");

        // Test: Level up the pet and print satisfaction
        LevelUp();
        Debug.Log($"Pet satisfaction after level up: {Satisfaction}%");
    }

    public void IncreaseSatisfaction(int amount) {
        Satisfaction += amount;
        if (Satisfaction > 100) {
            Satisfaction = 100;
        }
    }

    public void DecreaseSatisfaction(int amount) {
        Satisfaction -= amount;
        if (Satisfaction < 0) {
            Satisfaction = 0;
        }
    }

    public void LevelUp() {
        Level++;
        ResetSatisfaction();
    }

    public void ResetSatisfaction() {
        Satisfaction = 100;
        Debug.Log($"Hooray! Your companion has leveled up to {Level} and satisfaction has been reset to 100%.");
    }
}
