using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtCoin;

        private void Awake()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }

        public void ButtonGetRewardAds()
        {
            int reward = LevelManager.Ins.CurrentLevel.rewardForWinner;
            int adsReward = LevelManager.Ins.CurrentLevel.rewardForWinnerAfterAds;

            int total = reward * adsReward;

            Debug.Log("Next Level After Ads");
        }

        public void ButtonGetReward()
        {
            int reward = LevelManager.Ins.CurrentLevel.rewardForWinner;
            int total = reward;

            Debug.Log("Next Level");
        }

        public void ShowTextCoin()
        {
            string text = LevelManager.Ins.CurrentLevel.rewardForWinner.ToString() == null ? "0" : LevelManager.Ins.CurrentLevel.rewardForWinner.ToString();
            txtCoin.text = text;
        }


    }
}

