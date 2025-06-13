using Connect4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Khang
{
    public class LevelManager : SingletonSerializable<LevelManager>
    {
        public LevelData CurrentLevel;
        public List<ShapeData> CurShapesInLevel = new List<ShapeData>();

        protected override void Awake()
        {

        }

        void Start()
        {

        }

        public void InitializeGameplayLevel(int level)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            Debug.Log(currentSceneName + " " + DataManager.Ins.LevelsData[1]);

            LevelData levelData = DataManager.Ins.LevelsData.Find(level => level.levelType.ToString() == currentSceneName);
            if (levelData == null)
            {
                Debug.LogError("Level Data Khong lay duoc ");
                return;
            }
            CurrentLevel = levelData;
            Debug.Log(CurrentLevel);

            foreach (var shapeType in levelData.shapeTypes)
            {
                var shape = DataManager.Ins.ShapesData.Find(s => s.shapeType == shapeType);
                if (shape != null)
                {
                    CurShapesInLevel.Add(shape);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

