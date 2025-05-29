using Khang;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
namespace Khang
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] private Inventory inventory = new Inventory();

        public Inventory GetInventory() { return inventory; }

        public void AddUserOwn(string itemType, string nameType)
        {
            string dataOwnStr = $"{itemType}_{nameType}";

            List<string> userOwnStr = inventory.UserOwn;
            if (!userOwnStr.Contains(dataOwnStr))
            {
                userOwnStr.Add(dataOwnStr);
                DataManager.Ins.SetItem(itemType, nameType);
            }
        }

        public void AddUserEquip(string itemType, string nameType)
        {
            string dataOwnStr = $"{itemType}_{nameType}";

            List<string> userEquippedStr = inventory.UserEquipped;
            int index = userEquippedStr.FindIndex(equippedStr => equippedStr.StartsWith($"{itemType}_"));
            if (index != -1)
            {
                userEquippedStr[index] = dataOwnStr;
            }
            else
            {
                userEquippedStr.Add(dataOwnStr);
            }
        }

        public void Reset()
        {
            inventory.UserEquipped.Clear();
            inventory.UserOwn.Clear();

            List<ItemGroup> itemsGroup = DataManager.Ins.ItemsData.itemGroups;

            for (int i = 0; i < itemsGroup.Count; i++)
            {
                var itemGroup = itemsGroup[i];
                string itemTypeStr = itemGroup.itemType.ToString();

                for (int j = 0; j < itemGroup.items.Count; j++)
                {
                    var item = itemGroup.items[j];
                    item.isBought = false;

                    if (j == 0)
                    {
                        string nameType = item.itemName;
                        AddUserOwn(itemTypeStr, nameType);
                        AddUserEquip(itemTypeStr, nameType);
                    }
                }
            }
        }



    }

}
