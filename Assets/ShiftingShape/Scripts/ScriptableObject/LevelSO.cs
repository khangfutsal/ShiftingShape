using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShiftingShape
{
    [CreateAssetMenu(fileName = "Level SO", menuName = "SO/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public string nameScene;
        public List<ShapeType> shapeTypes;
    }
}

