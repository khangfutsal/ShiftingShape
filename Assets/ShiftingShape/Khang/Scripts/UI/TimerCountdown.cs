using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace Khang
{
    public class TimerCountdown : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtTimerCountdown;
        [SerializeField] private UnityEvent onStart = new UnityEvent();

        private void Awake()
        {
            txtTimerCountdown = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            txtTimerCountdown.gameObject.SetActive(false);
        }

        public void StartCountdown()
        {
            StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            int count = 3;

            while (count > 0)
            {
                txtTimerCountdown.text = count.ToString();
                txtTimerCountdown.alpha = 1f;

                // Fade alpha từ 1 → 0 trong 0.5s bằng DOTween
                txtTimerCountdown.DOFade(0f, 0.5f);

                yield return new WaitForSeconds(1f); // Chờ 1s rồi đổi số

                count--;
            }

            // Khi kết thúc countdown, bạn có thể clear hoặc làm gì đó tiếp
            txtTimerCountdown.text = "";
            onStart?.Invoke();
            this.gameObject.SetActive(false);
        }
    }

}
