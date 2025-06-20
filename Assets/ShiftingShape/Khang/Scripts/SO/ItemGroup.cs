using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    [Serializable]
    public class ItemGroup
    {
        public Sprite spriteItemGroup;
        public Sprite spriteItemFocus;
        public ItemType itemType;
        public List<ItemData> items;
    }
}

