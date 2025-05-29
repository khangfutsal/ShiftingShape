using System.Collections;
using System.Collections.Generic;
using Connect4;
using UnityEngine;
namespace ShiftingShape
{
    public class ShapeManager : Singleton<ShapeManager>
    {
        [SerializeField] private List<BaseShape> shapes;

        public List<BaseShape> GetListShapes() => shapes;
    }
}

