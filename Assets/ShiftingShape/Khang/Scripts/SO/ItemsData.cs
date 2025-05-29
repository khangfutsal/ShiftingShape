using Khang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    [CreateAssetMenu(menuName = "Khang/SO/ItemsData")]
    public class ItemsData : ScriptableObject
    {
        public List<ItemGroup> itemGroups;

        public ItemGroup GetItemGroup(ItemType itemType) => itemGroups.Find(i => i.itemType == itemType);
    }
}

