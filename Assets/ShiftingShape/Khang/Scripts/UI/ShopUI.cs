using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Khang
{
    public class ShopUI : Singleton<ShopUI>
    {
        [Header("Tab Variables")]
        [SerializeField] private GameObject tabPrefab;
        [SerializeField] private Transform contentTf;
        [SerializeField] List<ItemTab> itemsTab;

        [Header("Item Variables")]
        [SerializeField] private GameObject itemScrollViewPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform itemTf;
        [SerializeField] List<ItemShop> items;
        [SerializeField] private List<GameObject> itemsParentObj = new List<GameObject>();

        [Header("Item Display UI Variables")]
        public ItemShopDisplayUI ItemDisplayUI;

        #region Unity Functions

        protected override void Awake()
        {
            this.gameObject.SetActive(false);
        }

        #endregion


        #region Behaviour Enable/Disable Function
        public void DisableAll()
        {
            foreach (var item in itemsTab)
            {
                Destroy(item.gameObject);
            }

            foreach (var item in itemsParentObj)
            {
                Destroy(item);
            }

            itemsTab.Clear();
            itemsParentObj.Clear();
            items.Clear();
        }

        public void EnableAll()
        {
            InitShopItem();
            InitShopTab();
            SetDefaultShop();
        }

        public void SetDefaultShop()
        {
            var itemGroups = DataManager.Ins.ItemsData;
            var defaultItemType = itemGroups.itemGroups[0].itemType.ToString();
            var defaultItem = itemGroups.itemGroups[0].items.Find(i => i.FirstItem == true);

            ShowTab(defaultItemType);
            ShowItem(defaultItemType, defaultItem.itemName);

        }


        #endregion

        #region Tab Functions
        public void InitShopTab()
        {
            var itemGroups = DataManager.Ins.ItemsData;

            for (int i = 0; i < itemGroups.itemGroups.Count; i++)
            {
                GameObject tabObj = Instantiate(tabPrefab, contentTf);
                tabObj.name = $"{itemGroups.itemGroups[i].itemType.ToString()}Tab";

                ItemTab itemTab = tabObj.GetComponent<ItemTab>();

                string itemTypeStr = itemGroups.itemGroups[i].itemType.ToString();
                string nameTypeStr = itemGroups.itemGroups[i].items.Find(i => i.itemName.Contains("Normal")).itemName;

                var localIndex = i; // ⭐ Fix ở đây

                itemTab.ConfigItem(itemGroups.itemGroups[i].spriteItemGroup, itemGroups.itemGroups[i].spriteItemFocus, itemTypeStr);
                itemTab.SetData(itemGroups.itemGroups[i].itemType);
                itemTab.btnItemTab.onClick.AddListener(() =>
                {
                    ShowItem(itemTypeStr, nameTypeStr);
                    ShowTab(itemTypeStr);
                });

                itemsTab.Add(itemTab);
            }
        }


        public void ShowTab(string nameTab)
        {
            foreach (var tab in itemsTab)
            {
                Debug.Log($"(Show Tab) tab name :{tab.name}");
                bool isSelected = tab.name.Contains(nameTab);
                tab.SetActiveTab(isSelected);
            }
        }

        #endregion

        #region Item Functions
        public void InitShopItem()
        {
            var itemGroups = DataManager.Ins.ItemsData;

            for (int i = 0; i < itemGroups.itemGroups.Count; i++)
            {
                GameObject itemParentObj = Instantiate(itemScrollViewPrefab, itemTf);
                itemParentObj.name = ($"Item{itemGroups.itemGroups[i].itemType}Parent");
                itemsParentObj.Add(itemParentObj);


                for (int j = 0; j < itemGroups.itemGroups[i].items.Count; j++)
                {
                    var itemType = itemGroups.itemGroups[i].itemType;
                    var itemName = itemGroups.itemGroups[i].items[j].itemName;
                    var isItemBought = itemGroups.itemGroups[i].items[j].isBought;

                    if (itemName.Contains("Normal")) continue;

                    GameObject itemObj = Instantiate(itemPrefab, itemParentObj.transform.Find("Viewport").Find("Content"));
                    itemObj.name = itemGroups.itemGroups[i].items[j].itemName;

                    ItemShop item = itemObj.GetComponent<ItemShop>();
                    item.ConfigItemData((ShapeItemData)itemGroups.itemGroups[i].items[j], itemType);

                    item.btnIconItem.onClick.AddListener(() =>
                    {
                        item.ButtonIconItem(itemType, itemName);
                        SetActiveItem(item);
                        ItemDisplay itemDislay = ItemDisplayHolder.Ins.CurrentItemDisplay;
                        if (itemDislay == null) return;

                        ItemDisplayUI.SetItemDisplay(itemDislay);

                    });

                    item.btnBuyItem.onClick.AddListener(() =>
                    {
                        item.ButtonBuyItem(itemType.ToString(), itemName);
                        Debug.Log($"Buy item {itemType} {itemName}");
                    });

                    item.SetStatusItem(isItemBought);
                }
            }
        }

        public void SetActiveItem(ItemShop item = null)
        {
            bool isSameItem;
            foreach (var _item in items)
            {
                isSameItem = _item == item;
                _item.SetActiveItem(isSameItem);
            }
        }

        public void ShowItem(string nameTab, string nameItem)
        {
            bool isActive;
            items.Clear();

            foreach (Transform child in itemTf)
            {
                isActive = false;
                if (child.name.Contains(nameTab))
                {
                    isActive = true;
                    Transform content = child.transform.Find("Viewport/Content");
                    items = content.GetComponentsInChildren<ItemShop>(true).ToList();
                    SetActiveItem();

                }
                child.gameObject.SetActive(isActive);
            }

            ItemType itemType;
            if (System.Enum.TryParse(nameTab, out itemType))
            {
                Debug.Log($"Type {itemType} with tab {nameTab} with name{nameItem}");
                ItemDisplayHolder.Ins.ShowItemDisplay(
                           itemType,
                           nameItem
                       );
                ItemDisplay itemDislay = ItemDisplayHolder.Ins.CurrentItemDisplay;
                if (itemDislay == null) return;

                ItemDisplayUI.SetItemDisplay(itemDislay);
            }


        }
        #endregion

        private void OnEnable()
        {
            EnableAll();
        }

        private void OnDisable()
        {
            DisableAll();
        }
    }
}

