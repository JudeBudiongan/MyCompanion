using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<CompanionData> companionsData = new List<CompanionData>();
}

[System.Serializable]
public class CompanionData
{
    public int CompanionID;
    public int SatisfactionLevel;
    public bool IsBought;
}

public class GameDataSaver : MonoBehaviour
{
    public CompanionManager companionManager; // Reference to the CompanionManager

    void Start()
    {
        LoadGameData();
    }

    public void SaveGameData()
    {
        GameData gameData = new GameData();

        foreach (var companion in companionManager.companions)
        {
            CompanionData data = new CompanionData
            {
                CompanionID = companion.CompanionID,
                SatisfactionLevel = companion.SatisfactionLevel,
                IsBought = companion.IsBought
            };
            gameData.companionsData.Add(data);
        }

        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();

        Debug.Log("Game data saved.");
    }

    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            foreach (var data in gameData.companionsData)
            {
                var companion = companionManager.GetCompanionById(data.CompanionID);
                if (companion != null)
                {
                    companion.SatisfactionLevel = data.SatisfactionLevel;
                    companion.IsBought = data.IsBought;
                    Debug.Log($"Loaded data for {companion.PetName}: Satisfaction {companion.SatisfactionLevel}, IsBought {companion.IsBought}");
                }
            }
        }
        else
        {
            Debug.Log("No game data found.");
        }
    }
}
