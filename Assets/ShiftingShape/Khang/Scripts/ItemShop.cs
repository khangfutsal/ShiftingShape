using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class ItemShop : MonoBehaviour
    {
        [SerializeField] private ShapeItemData shapeItemData;
        [SerializeField] private ItemType itemType;

        public Button btnIconItem;
        [SerializeField] private Image imgIconItem;
        [SerializeField] private GameObject imgFocus;

        public Button btnBuyItem;
        [SerializeField] private TextMeshProUGUI txtBuyItem;

        private void Awake()
        {
            btnIconItem = GetComponent<Button>();
            imgIconItem = GetComponent<Image>();

            btnBuyItem = transform.Find("BtnBuy").GetComponent<Button>();
            txtBuyItem = transform.Find("BtnBuy").GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ConfigItem(int price, Sprite sprite)
        {
            imgIconItem.sprite = sprite;
            txtBuyItem.text = price.ToString();
        }

        public void ConfigItemData(ShapeItemData _shapeItemData, ItemType _itemType) { shapeItemData = _shapeItemData; itemType = _itemType; }

        public void ButtonIconItem(ItemType _itemType, string _nameType)
        {
            Debug.Log("Button Icon item");
            ItemDisplayHolder.Ins.ShowItemDisplay(_itemType, _nameType);
        }

        public void ButtonBuyItem(string itemType, string nameType)
        {
            InventoryManager.Ins.AddUserOwn(itemType, nameType);
            SetStatusItem(true);
        }

        public void SetStatusItem(bool active)
        {
            btnBuyItem.interactable = active ? false : true;
        }

        public void SetActiveItem(bool isSelected)
        {
            imgFocus.SetActive(isSelected);
        }
    }
}
