using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace DailyRewardSystem {
	public enum RewardType {
		Coins
	}

	[Serializable] public struct Reward {
		public RewardType Type;
		public int Amount;
	}

	public class DailyRewards : MonoBehaviour {

		[Header("Main Menu UI")]
		[SerializeField] Text coinsText;

		[Space]
		[Header("Reward UI")]
		[SerializeField] GameObject rewardsCanvas;
		[SerializeField] Button openButton;
		[SerializeField] Button closeButton;
		[SerializeField] Image rewardImage;
		[SerializeField] Text rewardAmountText;
		[SerializeField] Button claimButton;
		[SerializeField] GameObject rewardsNotification;
		[SerializeField] GameObject noMoreRewardsPanel;

		[Space]
		[Header("Rewards Images")]
		[SerializeField] Sprite iconCoinsSprite;

		[Space]
		[Header("FX")]
		[SerializeField] ParticleSystem fxCoins;

		[Space]
		[Header("Timing")]
		[SerializeField] double nextRewardDelay = 23f; // 23 hours for the next reward
		[SerializeField] float checkForRewardDelay = 5f; // Check every 5 seconds for reward availability

		private int nextRewardIndex;
		private bool isRewardReady = false;

		// Reference to CoinManager
		private CoinManager coinManager;

		void Start() {
			Initialize();
			StopAllCoroutines();
			StartCoroutine(CheckForRewards());
		}

		void Initialize() {
			coinManager = CoinManager.Instance;

			nextRewardIndex = PlayerPrefs.GetInt("Next_Reward_Index", 0);
			UpdateCoinsTextUI();

			// Add Click Events for buttons
			openButton.onClick.RemoveAllListeners();
			openButton.onClick.AddListener(OnOpenButtonClick);

			closeButton.onClick.RemoveAllListeners();
			closeButton.onClick.AddListener(OnCloseButtonClick);

			claimButton.onClick.RemoveAllListeners();
			claimButton.onClick.AddListener(OnClaimButtonClick);

			// Set the claim datetime if the game is opened for the first time
			if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime"))) {
				PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
			}
		}

		IEnumerator CheckForRewards() {
			while (true) {
				if (!isRewardReady) {
					DateTime currentDatetime = DateTime.Now;
					DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));

					// Calculate hours between the current time and the last reward claim time
					double elapsedHours = (currentDatetime - rewardClaimDatetime).TotalHours;

					if (elapsedHours >= nextRewardDelay)
						ActivateReward();
					else
						DeactivateReward();
				}

				yield return new WaitForSeconds(checkForRewardDelay);
			}
		}

		void ActivateReward() {
			isRewardReady = true;

			noMoreRewardsPanel.SetActive(false);
			rewardsNotification.SetActive(true);

			// Since we're only dealing with Coins, directly update UI with coin reward
			rewardImage.sprite = iconCoinsSprite;
			rewardAmountText.text = string.Format("+{0}", 100); // Example: reward 100 coins
		}

		void DeactivateReward() {
			isRewardReady = false;

			noMoreRewardsPanel.SetActive(true);
			rewardsNotification.SetActive(false);
		}

		void OnClaimButtonClick() {
			// Here we interact with the CoinManager to add coins
			coinManager.TotalCoins += 250; // Rewarding 250 coins
			fxCoins.Play();
			UpdateCoinsTextUI();

			isRewardReady = false;

			// Increment next reward index
			nextRewardIndex++;
			if (nextRewardIndex >= 5) // Limiting reward cycle to 5 for example
				nextRewardIndex = 0;

			PlayerPrefs.SetInt("Next_Reward_Index", nextRewardIndex);
			PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());

			DeactivateReward();
		}

		// Update the UI with the current coin count
		void UpdateCoinsTextUI() {
			coinsText.text = coinManager.TotalCoins.ToString();
		}

		// Open and close reward UI panels
		void OnOpenButtonClick() {
			rewardsCanvas.SetActive(true);
		}

		void OnCloseButtonClick() {
			rewardsCanvas.SetActive(false);
		}
	}
}
