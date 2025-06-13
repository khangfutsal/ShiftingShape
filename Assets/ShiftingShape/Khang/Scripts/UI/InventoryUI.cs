using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace Khang
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Tab Variables")]
        [SerializeField] private GameObject tabPrefab;
        [SerializeField] private Transform contentTf;
        [SerializeField] List<ItemTab> itemsTab;

        [Header("Item Variables")]
        [SerializeField] private GameObject itemScrollViewPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform itemTf;
        [SerializeField] List<ItemInventory> listItemsInventory;
        [SerializeField] private List<GameObject> itemsParentObj = new List<GameObject>();

        [Header("Item Display UI Variables")]
        public ItemShopDisplayUI ItemDisplayUI;

        [SerializeField] private ItemInventory currentItemInventory;

        [SerializeField] private Button btnUsing;



        #region Unity Functions
        private void Awake()
        {
            btnUsing = transform.Find("BG").Find("Inventory").Find("BtnUsing").GetComponent<Button>();
            this.gameObject.SetActive(false);
        }

        private void Start()
        {
            //btnUsing.onClick.AddListener(ButtonUsingItem);
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
            listItemsInventory.Clear();
        }

        public void EnableAll()
        {
            InitInventoryItem();
            InitInventoryTab();
            SetDefaultInventory();
        }

        public void SetDefaultInventory()
        {
            var itemGroups = DataManager.Ins.ItemsData;
            var defaultItemType = itemGroups.itemGroups[0].itemType.ToString();


            ShowTab(defaultItemType);
            ShowItemInTab(defaultItemType);
            //RefreshEquippedItems();

        }


        #endregion



        #region Tab Functions
        public void InitInventoryTab()
        {
            var itemGroups = DataManager.Ins.ItemsData;

            for (int i = 0; i < itemGroups.itemGroups.Count; i++)
            {
                GameObject tabObj = Instantiate(tabPrefab, contentTf);
                tabObj.name = $"{itemGroups.itemGroups[i].itemType.ToString()}Tab";
                ItemTab itemTab = tabObj.GetComponent<ItemTab>();

                string itemTypeStr = itemGroups.itemGroups[i].itemType.ToString();
                var localIndex = i; // ⭐ Fix ở đây

                itemTab.ConfigItem(itemGroups.itemGroups[i].spriteItemGroup, itemTypeStr);
                itemTab.SetData(itemGroups.itemGroups[i].itemType);
                itemTab.btnItemTab.onClick.AddListener(() =>
                {
                    ShowItemInTab(itemTypeStr);
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
        public void InitInventoryItem()
        {
            Inventory userInventory = InventoryManager.Ins.GetInventory();
            var itemGroups = DataManager.Ins.ItemsData;

            for (int i = 0; i < itemGroups.itemGroups.Count; i++)
            {
                var group = itemGroups.itemGroups[i];

                GameObject itemParentObj = Instantiate(itemScrollViewPrefab, itemTf);
                itemParentObj.name = $"Item{group.itemType}Parent";
                itemsParentObj.Add(itemParentObj);

                string itemTypeStr = group.itemType.ToString();

                List<string> userOwnsStr = userInventory.UserOwn.FindAll(x => x.Contains(itemTypeStr));
                List<ItemData> userOwnData = group.items.FindAll(item =>
                    userOwnsStr.Any(ownStr => ownStr.Contains(item.itemName))
                );

                for (int j = 0; j < userOwnData.Count; j++)
                {
                    var userItem = userOwnData[j];
                    var itemType = group.itemType;
                    var itemName = userItem.itemName;

                    GameObject itemObj = Instantiate(itemPrefab, itemParentObj.transform.Find("Viewport/Content"));
                    itemObj.name = itemName;

                    ItemInventory itemInventory = itemObj.GetComponent<ItemInventory>();
                    itemInventory.ConfigItemData((ShapeItemData)userItem, itemType);

                    // Lưu lại biến local tránh bug closure
                    int index = listItemsInventory.Count;

                    itemInventory.BtnIconItem.onClick.AddListener(() =>
                    {
                        CheckStatusUsing(listItemsInventory[index].IsUsing);

                        // Các xử lý khác
                        currentItemInventory = listItemsInventory[index];
                        listItemsInventory[index].ButtonIconItem(itemType, itemName);

                        ItemDisplay itemDisplay = ItemDisplayHolder.Ins.CurrentItemDisplay;
                        if (itemDisplay == null) return;
                        ItemDisplayUI.SetItemDisplay(itemDisplay);
                    });

                    listItemsInventory.Add(itemInventory);
                }
            }
        }


        public void CheckStatusUsing(bool _value)
        {
            btnUsing.gameObject.SetActive(!_value); 
        }

        public void ButtonUsingItem()
        {
            if (currentItemInventory == null) return;

            var itemType = currentItemInventory.ItemType.ToString();
            var nameType = currentItemInventory.ShapeItemData.itemName;

            InventoryManager.Ins.AddUserEquip(itemType, nameType);

            for (int i = 0; i < listItemsInventory.Count; i++)
            {
                bool isCurrent = listItemsInventory[i] == currentItemInventory;
                listItemsInventory[i].SetStatusUsing(isCurrent);
                CheckStatusUsing(true);
            }
        }

        public void ShowItemInTab(string nameTab)
        {
            Inventory userInventory = InventoryManager.Ins.GetInventory();
            string userEquipped = userInventory.UserEquipped.Find(e => e.Contains(nameTab));

            // Hiện/ẩn item theo tab
            foreach (Transform child in itemTf)
            {
                bool isActive = child.name.Contains(nameTab);
                child.gameObject.SetActive(isActive);
            }

            // Reset trạng thái using toàn bộ item
            foreach (var item in listItemsInventory)
            {
                item.SetStatusUsing(false);
            }

            // Nếu không tìm thấy item equipped tương ứng tab thì thôi
            if (string.IsNullOrEmpty(userEquipped))
                return;

            string[] split = userEquipped.Split('_');
            if (split.Length != 2)
                return;

            string equippedType = split[0];
            string equippedName = split[1];

            Debug.Log($"equipped : type {equippedType} name {equippedName}");

            var itemInventory = listItemsInventory.Find(item =>
                item.ItemType.ToString() == equippedType &&
                item.ShapeItemData.itemName == equippedName);

            if (itemInventory != null)
            {
                itemInventory.SetStatusUsing(true);

                ItemDisplayHolder.Ins.ShowItemDisplay(itemInventory.ItemType, itemInventory.ShapeItemData.itemName);

                ItemDisplay itemDisplay = ItemDisplayHolder.Ins.CurrentItemDisplay;
                if (itemDisplay == null)
                    return;

                ItemDisplayUI.SetItemDisplay(itemDisplay);
                CheckStatusUsing(true);

                Debug.Log($"Item inventory khác null");
            }
        }



        public void RefreshEquippedItems()
        {
            Inventory userInventory = InventoryManager.Ins.GetInventory();
            List<string> listUserEquipped = userInventory.UserEquipped;

            for (int j = 0; j < listItemsInventory.Count; j++)
            {
                bool isEquipped = false;

                for (int i = 0; i < listUserEquipped.Count; i++)
                {
                    string[] split = listUserEquipped[i].Split('_');
                    if (split.Length != 2) continue;

                    string equippedType = split[0];
                    string equippedName = split[1];

                    if (listItemsInventory[j].ItemType.ToString() == equippedType &&
                        listItemsInventory[j].ShapeItemData.itemName == equippedName)
                    {
                        isEquipped = true;
                        break; // tìm trúng rồi thì break luôn
                    }
                }

                listItemsInventory[j].SetStatusUsing(isEquipped);
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

