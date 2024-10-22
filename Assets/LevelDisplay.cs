using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelTextUI;

    void Start()
    {
        // Initialize the level display when the menu is loaded
        UpdateLevelDisplay();
    }

    public void UpdateLevelDisplay()
    {
        if (CompanionManager.Instance != null && CompanionManager.Instance.companions.Count > 0)
        {
            // Assuming you're tracking the first companion, update the level display
            var companion = CompanionManager.Instance.companions[0]; // or whichever companion you'd like
            levelTextUI.text = "Level: " + companion.Level;
        }
        else
        {
            Debug.LogError("CompanionManager or companions list is missing.");
        }
    }
}
