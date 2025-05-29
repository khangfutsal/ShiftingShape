using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class ItemTab : MonoBehaviour
    {
        [SerializeField] private Image imgItemTab;
        [SerializeField] private TextMeshProUGUI txtItemTab;
        public Button btnItemTab;

        [SerializeField] private ItemType itemType;

        private void Awake()
        {
            btnItemTab = GetComponent<Button>();

            imgItemTab = transform.Find("Horizontal").GetComponentInChildren<Image>();
            txtItemTab = transform.Find("Horizontal").GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetData(ItemType itemType)
        {
            this.itemType = itemType;
        }

        public void ConfigItem(Sprite _spriteItemTab, string _nameItemTab)
        {
            imgItemTab.sprite = _spriteItemTab;
            txtItemTab.text = _nameItemTab;
        }

        public void SetActiveTab(bool active)
        {
            imgItemTab.color = active ? Color.white : Color.red;
            ItemDisplayHolder.Ins.SpawnItem(itemType);

        }


    }
}

