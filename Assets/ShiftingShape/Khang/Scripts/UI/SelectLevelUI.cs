using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class SelectLevelUI : MonoBehaviour
    {
        [SerializeField] private GameObject levelPrefab;
        [SerializeField] private Transform contentTf;
        [SerializeField] private List<Button> currentlevels;


        private void Awake()
        {
            this.gameObject.SetActive(false);
        }

        private void Start()
        {
            Test();
        }

        public void Test()
        {
            var Levels = DataManager.Ins.LevelsData;

            for (int i = 0; i < Levels.Count; i++)
            {
                int index = i;
                GameObject obj = Instantiate(levelPrefab, contentTf);
                obj.name = Levels[index].levelType.ToString();
                Button btnLevel = obj.GetComponent<Button>();
                TextMeshProUGUI txt = btnLevel.GetComponentInChildren<TextMeshProUGUI>();
                txt.text = (index + 1).ToString();


                btnLevel.onClick.AddListener(() =>
                {
                    ButtonSelectLevel(Levels[index].levelType);
                });
                currentlevels.Add(btnLevel);
            }
        }

        public void ButtonSelectLevel(LevelType levelType)
        {
            Debug.Log($"Select {levelType}");
        }

        public void UpdateStatus()
        {
            var levels = DataManager.Ins.LevelsData;
            for (int i = 0; i < levels.Count; i++)
            {
                bool isFinishedLevel = levels[i].isFinishedLevel;
                Button btnSelectLevel = currentlevels[i];
                SetStatus(btnSelectLevel, isFinishedLevel);
            }
        }

        public void SetStatus(Button btn, bool isFinishedLevel)
        {
            bool check = isFinishedLevel ? false : true;
            btn.interactable = check;
        }

        private void OnEnable()
        {
            if (currentlevels.Count == 0) return;
            UpdateStatus();
        }
    }

}
