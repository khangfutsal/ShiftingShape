using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    [CreateAssetMenu(menuName = "Khang/SO/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite sprite;
        public int price;
        public GameObject itemObj;
        public bool isBought;
    }

}
