using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShiftingShape
{
    [Serializable]
    public class ShapeData
    {
        public ShapeType shapeType;
        public LayerMask interact_Layermask;
        public Sprite sprite;
        public float speedExpected;
        public float speedUnexpected;
    }
}

