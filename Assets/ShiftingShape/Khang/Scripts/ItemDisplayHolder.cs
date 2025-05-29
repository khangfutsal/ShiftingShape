using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    public class ItemDisplayHolder : Singleton<ItemDisplayHolder>
    {
        public ItemDisplay CurrentItemDisplay;
        public void SpawnItem(ItemType itemType)
        {
            if (!HasGameObjectSpawnedByNameTab(itemType))
            {
                Debug.Log("123");
                SpawnItemByType(itemType);
            }
        }

        public void SpawnItemByType(ItemType itemType)
        {
            List<ItemData> items = DataManager.Ins.ItemsData.GetItemGroup(itemType).items;

            GameObject typeObj = new GameObject($"{itemType.ToString()}");
            typeObj.transform.parent = transform;

            for (int i = 0; i < items.Count; i++)
            {
                GameObject item = Instantiate(items[i].itemObj, typeObj.transform);
                item.name = items[i].name;
                item.SetActive(false);
            }
        }

        public void ShowItemDisplay(ItemType itemType, string nameType)
        {
            HideAllItemParent();

            GameObject itemParentObj = FindItemParentByType(itemType);

            Debug.Log($"Test {itemParentObj}");
            itemParentObj.SetActive(true);
            if (itemParentObj == null) return;

            foreach (Transform item in itemParentObj.transform)
            {
                bool isTargetItem = item.name == nameType;
                item.gameObject.SetActive(isTargetItem);

                if (isTargetItem)
                {
                    CurrentItemDisplay = item.GetComponentInChildren<ItemDisplay>();
                }
            }
        }

        public void HideAllItemParent()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public GameObject FindItemParentByType(ItemType itemType)
        {
            foreach (Transform child in transform)
            {
                if (child.name == itemType.ToString())
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        public bool HasGameObjectSpawnedByNameTab(ItemType itemType)
        {
            foreach (Transform child in transform)
            {
                if (child.name == itemType.ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}

