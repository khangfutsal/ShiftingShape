using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Khang
{
    [CreateAssetMenu(menuName = "Khang/SO/ShapeData")]
    public class ShapeData : ScriptableObject
    {
        public ShapeType shapeType;
        public float SpeedDesired;
        public float SpeedUndesired;
        public LayerMask InteractLayers;
        public Sprite SpriteShape;
    }
}

