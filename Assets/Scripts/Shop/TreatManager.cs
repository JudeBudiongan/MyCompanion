using System.Collections.Generic; 
using UnityEngine; 

public class TreatManager : MonoBehaviour
{
    [System.Serializable]
    public class Treat
    {
        public int Stock { get; set; } // Stock of the treat
        public int Quantity { get; set; } // Quantity in the player's storage

        public Treat(int stock)
        {
            Stock = stock;
            Quantity = 0; // Initialize quantity to 0
        }
    }

    [System.Serializable]
    public class TreatItem : Treat
    {
        public int TreatID { get; set; } // Unique ID for the treat
        public string TreatName { get; set; }

        public TreatItem(int treatID, string treatName)
            : base(10) // Initialized with 10 stock
        {
            TreatID = treatID;
            TreatName = treatName;
        }
    }

    public static TreatManager Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public List<TreatItem> treats = new List<TreatItem>();

    void Start()
    {
        // Add treats to the list with IDs and names
        treats.Add(new TreatItem(0, "Chewy Delight"));
        treats.Add(new TreatItem(1, "Crunchy Biscuit"));
        treats.Add(new TreatItem(2, "Tasty Bone"));
        treats.Add(new TreatItem(3, "Yummy Chewstick"));
    }

    public void IncreaseTreatStock(int treatID, int amount)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            treats[treatID].Stock += amount;
            Debug.Log($"{amount} of {treats[treatID].TreatName} added. Current stock: {treats[treatID].Stock}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID.");
        }
    }

    public void DecreaseTreatStock(int treatID, int amount)
    {
        if (treatID >= 0 && treatID < treats.Count && treats[treatID].Stock >= amount)
        {
            treats[treatID].Stock -= amount;
            Debug.Log($"{amount} of {treats[treatID].TreatName} used. Current stock: {treats[treatID].Stock}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID or insufficient stock.");
        }
    }

    public void IncreaseQuantity(int treatID)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            treats[treatID].Quantity += 1;
            Debug.Log($"1 of {treats[treatID].TreatName} added to quantity. Current quantity: {treats[treatID].Quantity}");
        }
        else
        {
            Debug.LogWarning("Invalid treat ID.");
        }
    }

    public TreatItem GetTreatById(int treatID)
    {
        if (treatID >= 0 && treatID < treats.Count)
        {
            return treats[treatID];
        }
        return null;
    }
}
