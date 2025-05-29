using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class ItemInventory : MonoBehaviour
    {
        [SerializeField] private ShapeItemData shapeItemData;
        [SerializeField] private ItemType itemType;
        [SerializeField] private bool isUsing;

        public Button BtnIconItem;
        [SerializeField] private Image imgIconItem;

        public ItemType ItemType
        {
            get { return itemType; }
        }

        public bool IsUsing
        {
            get { return isUsing; }
        }

        public ShapeItemData ShapeItemData
        {
            get { return shapeItemData; }
        }


        private void Awake()
        {
            BtnIconItem = GetComponent<Button>();
            imgIconItem = GetComponent<Image>();

        }


        public void ConfigItem(int price, Sprite sprite)
        {
            imgIconItem.sprite = sprite;
        }

        public void ConfigItemData(ShapeItemData _shapeItemData, ItemType _itemType) { shapeItemData = _shapeItemData; itemType = _itemType; }

        public void ButtonIconItem(ItemType _itemType, string _nameType)
        {
            Debug.Log("Button Icon item");
            ItemDisplayHolder.Ins.ShowItemDisplay(_itemType, _nameType);
        }

        public void SetStatusUsing(bool _value)
        {
            isUsing = _value;
        }

    }
}

