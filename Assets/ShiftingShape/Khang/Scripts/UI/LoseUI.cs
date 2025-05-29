using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Khang
{
    public class LoseUI : MonoBehaviour
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

        public void ButtonPlayAgain()
        {
            Debug.Log("Play Again");
        }

        public void ButtonMainMenu()
        {
            Debug.Log("Back to main menu");
        }

        public void ShowTextCoin()
        {
            string text = LevelManager.Ins.CurrentLevel.rewardForLoser.ToString() == null ? "0" : LevelManager.Ins.CurrentLevel.rewardForLoser.ToString();
            txtCoin.text = text;
        }

    }
}

