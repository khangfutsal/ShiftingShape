using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class PauseUI : MonoBehaviour
    {

        private void Awake()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }



        public void ButtonPause()
        {
            GameController.Ins.GameManager.SetGameState(GameState.Pause);
        }

        public void ButtonContinue()
        {
            GameController.Ins.GameManager.SetGameState(GameState.Playing);

        }

        public void ButtonBackToMenu()
        {
            GameController.Ins.GameManager.SetGameState(GameState.MainMenu);
        }
    }

}
