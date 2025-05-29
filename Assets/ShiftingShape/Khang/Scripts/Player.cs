using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Khang
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<BaseShape> shapes = new List<BaseShape>();

        [SerializeField] private BaseShape currentShape;

        public BaseShape GetCurrentShape() => currentShape;


        private void Awake()
        {
            shapes = transform.GetComponentsInChildren<BaseShape>(true).ToList();
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            DisableShapes();
            SetOwnerShape();
            SetShape(ShapeType.Human);
            InitSkinShape();

        }

        public void InitSkinShape()
        {
            var userEquipped = InventoryManager.Ins.GetInventory().UserEquipped;
            for (int i = 0; i < userEquipped.Count; i++)
            {
                string[] split = userEquipped[i].Split('_');
                if (split.Length != 2)
                    return;

                string equippedType = split[0];
                string equippedName = split[1];

                var shape = shapes.Find(s => s.gameObject.name.Contains(equippedType));
                List<CurrentSkinShape> listSkinShapes = shape.GetComponentsInChildren<CurrentSkinShape>(true).ToList();
                

                for(int j = 0; j < listSkinShapes.Count; j++)
                {
                    bool isMatchName = listSkinShapes[j].name.Contains(equippedName);
                    Debug.Log($"type {equippedType} skinShape {listSkinShapes[j]}");
                    listSkinShapes[j].gameObject.SetActive(isMatchName);
                }

            }
        }

        public void ChangeShape(ShapeType shapeType)
        {
            BaseShape latestShape = shapes.Find(s => s.shapeData.shapeType == shapeType);
            StartCoroutine(ChangeShapeCouroutine(currentShape, latestShape));
        }

        public void DisableShapes()
        {
            foreach(var shape in shapes)
            {
                shape.Disable();
            }
        }

        public void EnableShapes()
        {
            foreach (var shape in shapes)
            {
                shape.Enable();
            }
        }

        public void SetOwnerShape()
        {
            foreach (var shape in shapes)
            {
                shape.SetIsPlayer(true);
            }
        }

        public void SetShape(ShapeType shapeType)
        {
            foreach (var shape in shapes)
            {
                bool isTarget = shape.shapeData.shapeType == shapeType;
                shape.gameObject.SetActive(isTarget);

                if (isTarget)
                    currentShape = shape;
            }
        }

        public IEnumerator ChangeShapeCouroutine(BaseShape prevShape, BaseShape laterShape)
        {
            if (prevShape == null) { Debug.Log("Prev Shape is null"); yield break; }
            

            StartCoroutine(prevShape.DisappearShape(0.5f));
            yield return new WaitForSeconds(0.25f);
            laterShape.transform.position = prevShape.transform.position;
            currentShape = laterShape;
            StartCoroutine(laterShape.AppearShape(0.5f));
        }

    }

}
