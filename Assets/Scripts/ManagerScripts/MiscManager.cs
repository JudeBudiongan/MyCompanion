using System.Collections.Generic;
using UnityEngine;

public class MiscManager : MonoBehaviour
{
    [System.Serializable]
    public class MiscItem
    {
        public bool IsBought { get; set; } // Indicates if the item is bought

        public MiscItem(bool isBought)
        {
            IsBought = isBought;
        }
    }

    [System.Serializable]
    public class Miscellaneous : MiscItem
    {
        public int MiscID { get; set; } // Unique ID for the item
        public string ItemName { get; set; }

        public Miscellaneous(int miscID, string itemName)
            : base(false) // Initialize IsBought as false
        {
            MiscID = miscID;
            ItemName = itemName;
        }
    }

    public static MiscManager Instance;
    
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

    public List<Miscellaneous> miscellaneousItems = new List<Miscellaneous>();

    void Start()
    {
        // Add miscellaneous items to the list with IDs and names

        // Example Miscellaneous Items
        miscellaneousItems.Add(new Miscellaneous(0, "Rubber Ball"));
        miscellaneousItems.Add(new Miscellaneous(1, "Cleaning Brush"));
        miscellaneousItems.Add(new Miscellaneous(2, "Scratching Post"));
        miscellaneousItems.Add(new Miscellaneous(3, "Feather Toy"));
        miscellaneousItems.Add(new Miscellaneous(4, "Dog Leash"));
        miscellaneousItems.Add(new Miscellaneous(5, "Squeaky Toy"));
    }

    public void SetMiscItemBought(int miscID)
    {
        if (miscID >= 0 && miscID < miscellaneousItems.Count)
        {
            miscellaneousItems[miscID].IsBought = true;
            Debug.Log($"{miscellaneousItems[miscID].ItemName} has been marked as bought.");
        }
        else
        {
            Debug.LogWarning("Invalid miscellaneous item ID.");
        }
    }

    public Miscellaneous GetMiscItemById(int miscID)
    {
        if (miscID >= 0 && miscID < miscellaneousItems.Count)
        {
            return miscellaneousItems[miscID];
        }
        return null;
    }

    // New method to check if a miscellaneous item is bought
    public bool IsMiscItemBought(int miscID)
    {
        if (miscID >= 0 && miscID < miscellaneousItems.Count)
        {
            return miscellaneousItems[miscID].IsBought;
        }
        return false; // Return false if the ID is invalid
    }
}
