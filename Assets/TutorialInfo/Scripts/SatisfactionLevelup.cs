using System;

public class Pet {
    public int Level {get; private set;}
    public int Satisfaction{get; private set;}

    public Pet() {
        Level = 1;
        Satisfaction = 100;
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
        Console.WriteLine($"Hooray! Your companion has levelled up to {Level} and satisfaction has been reset to 100%.");

    }
}

class Program
{
    static void Main(string[] args) {
        Pet myPet = new Pet();

        myPet.DecreaseSatisfaction(25); //satisfaction drops to 75%
        Console.WriteLine($"Pet Satisfaction: {myPet.Satisfaction}%");

        myPet.LevelUp();
        Console.WriteLine($"Pet satisfaction after level up {myPet.Satisfaction}%");
    }
}