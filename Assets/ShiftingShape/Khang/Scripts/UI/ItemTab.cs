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
        [SerializeField] private Image imgItemTabFocus;
        public Button btnItemTab;

        [SerializeField] private ItemType itemType;

        private void Awake()
        {
            btnItemTab = GetComponent<Button>();
        }

        public void SetData(ItemType itemType)
        {
            this.itemType = itemType;
        }

        public void ConfigItem(Sprite _spriteItemTab, Sprite _spriteItemTabFocus, string _nameItemTab)
        {
            imgItemTab.sprite = _spriteItemTab;
            imgItemTabFocus.sprite = _spriteItemTabFocus;
        }

        public void UpdateTabVisual(bool isActive)
        {
            imgItemTab.gameObject.SetActive(!isActive);
            imgItemTabFocus.gameObject.SetActive(isActive);
        }

        public void SetActiveTab(bool isActive)
        {
            UpdateTabVisual(isActive);
            ItemDisplayHolder.Ins.SpawnItem(itemType);
        }

        private void OnDisable()
        {
            SetActiveTab(false);
        }


    }
}

