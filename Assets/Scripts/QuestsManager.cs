using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestsManager : MonoBehaviour
{
    //buttons for pet/coin rewards
    public List<Button> PetRewardButtons; 
    public List<Button> CoinSpentRewardButtons; 
    private CompanionManager companionManager;
    private CoinManager coinManager;

    void Start()
    {
        companionManager = CompanionManager.Instance;
        coinManager = CoinManager.Instance;

        //loading from playerprefs to set the button states
        LoadQuestStatus();

        //listeners for when they are pressed
        foreach (Button button in PetRewardButtons)
        {
            button.onClick.AddListener(() => OnRewardButtonClicked(button));
        }

        foreach (Button button in CoinSpentRewardButtons)
        {
            button.onClick.AddListener(() => OnRewardButtonClicked(button));
        }
    }

    void Update()
    {
        UpdatePetRewardButtons();
        UpdateCoinSpentRewardButtons();
    }

    private void UpdatePetRewardButtons()
    {
        //enables buttons when condition met
        if (companionManager.NumberOfPets >= 1)
        {
            PetRewardButtons[0].interactable = true; 
        }
        if (companionManager.NumberOfPets >= 3)
        {
            PetRewardButtons[1].interactable = true; 
        }
        if (companionManager.NumberOfPets >= 5)
        {
            PetRewardButtons[2].interactable = true; 
        }
    }

    private void UpdateCoinSpentRewardButtons()
    {
        if (coinManager.TotalCoinsSpent >= 50)
        {
            CoinSpentRewardButtons[0].interactable = true;
        }
        if (coinManager.TotalCoinsSpent >= 250)
        {
            CoinSpentRewardButtons[1].interactable = true;
        }
        if (coinManager.TotalCoinsSpent >= 500)
        {
            CoinSpentRewardButtons[2].interactable = true;
        }
    }

    private void OnRewardButtonClicked(Button button)
    {
        //hide button after its pressed
        button.gameObject.SetActive(false);

        //reward based on which button was clicked
        GiveReward(button);

        //save button state in playerpref
        SaveQuestStatus(button);
    }

    private void GiveReward(Button button)
    {
        //give rewards based on button pressed
        if (PetRewardButtons.Contains(button))
        {
            int index = PetRewardButtons.IndexOf(button);
            switch (index)
            {
                case 0:
                    coinManager.IncreaseCoins(50);
                    break;
                case 1:
                    coinManager.IncreaseCoins(100); 
                    break;
                case 2:
                    coinManager.IncreaseCoins(150); 
                    break;
                default:
                    break;
            }
        }
        else if (CoinSpentRewardButtons.Contains(button))
        {
            //same thing as the petReward
            int index = CoinSpentRewardButtons.IndexOf(button);
            switch (index)
            {
                case 0:
                    coinManager.IncreaseCoins(50);
                    break;
                case 1:
                    coinManager.IncreaseCoins(100); 
                    break;
                case 2:
                    coinManager.IncreaseCoins(150); 
                    break;
                default:
                    break;
            }
        }

        Debug.Log($"Reward given for completing quest associated with button: {button.name}");
    }

    private void SaveQuestStatus(Button button)
    {
        //mark the quest as completed, and save
        PlayerPrefs.SetInt(button.name, 1); 
        PlayerPrefs.Save(); 
    }

    private void LoadQuestStatus()
    {
        //loop thru buttons to their saved states
        foreach (Button button in PetRewardButtons)
        {
            //0 is incomplete
            if (PlayerPrefs.GetInt(button.name, 0) == 1)
            {
                //hides button
                button.gameObject.SetActive(false);
            }
            else
            {
                //unclickable to start + visible
                button.interactable = false;
                button.gameObject.SetActive(true);
            }
        }
        //same as above
        foreach (Button button in CoinSpentRewardButtons)
        {
            if (PlayerPrefs.GetInt(button.name, 0) == 1)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.interactable = false;
                button.gameObject.SetActive(true);
            }
        }
    }
}
