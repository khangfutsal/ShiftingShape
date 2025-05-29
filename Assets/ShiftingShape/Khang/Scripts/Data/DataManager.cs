using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Khang
{
    public class DataManager : Singleton<DataManager>
    {
        public List<ShapeData> ShapesData;
        public List<LevelData> LevelsData;
        public ItemsData ItemsData;

        public ShapeData GetShape(string nameShape)
        {
            return ShapesData.Find(shape => shape.name == nameShape);
        }


        protected override void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {

        }

        public void SetItem(string itemTypeStr, string nameType)
        {
            if (Enum.TryParse<ItemType>(itemTypeStr, out ItemType itemType))
            {
                ItemGroup itemGroup = ItemsData.itemGroups.Find(i => i.itemType == itemType);
                if (itemGroup != null)
                {
                    ItemData itemData = itemGroup.items.Find(i => i.name == nameType);
                    itemData.isBought = true;
                    Debug.Log($"Found item group for {itemType}");
                }
                else
                {
                    Debug.LogWarning("ItemGroup not found.");
                }
            }
        }



#if UNITY_EDITOR

        [ContextMenu("Reset All Item")]
        public void ResetItem()
        {
            for (int i = 0; i < ItemsData.itemGroups.Count; i++)
            {
                for (int j = 0; j < ItemsData.itemGroups[i].items.Count; j++)
                {
                    ItemData itemData = ItemsData.itemGroups[i].items[j];
                    itemData.isBought = false;
                }
            }
        }

        [ContextMenu("Get All Shapes")]
        public void GetAllShapes()
        {
            ShapesData.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Khang.ShapeData", new[] { "Assets/ShiftingShape/Khang/SO/Shapes" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ShapeData shapeData = AssetDatabase.LoadAssetAtPath<ShapeData>(path);
                if (shapeData != null)
                {
                    ShapesData.Add(shapeData);
                }
            }

            Debug.Log("Đã load " + ShapesData.Count + " shape data.");
        }
        [ContextMenu("Get All Levels")]
        public void GetAllLevels()
        {
            LevelsData.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Khang.LevelData", new[] { "Assets/ShiftingShape/Khang/SO/Levels" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                if (levelData != null)
                {
                    LevelsData.Add(levelData);
                }
            }

            Debug.Log("Đã load " + LevelsData.Count + " shape data.");
        }

#endif


    }

}
