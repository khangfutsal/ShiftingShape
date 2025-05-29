using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Khang
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private List<BaseShape> shapes = new List<BaseShape>();

        [SerializeField] private BaseShape currentShape;
        [SerializeField] private float startTimeToRandom = 3f;
        [SerializeField] private float endTimeToRandom = 5f;


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
            SetShape(ShapeType.Human);
        }

        private IEnumerator BotChangeShapeRoutine()
        {
            List<ShapeData> shapesData = LevelManager.Ins.CurShapesInLevel;
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(startTimeToRandom, endTimeToRandom));

                int randomIndex = Random.Range(0, shapesData.Count);
                ShapeType randomShape = shapesData[randomIndex].shapeType;
                if (currentShape != shapesData[randomIndex])
                {
                    ChangeShape(randomShape);
                }
            }
        }

        public void DisableCouroutine()
        {
            this.StopAllCoroutines();
        }

        public void ChangeShape(ShapeType shapeType)
        {
            BaseShape latestShape = shapes.Find(s => s.shapeData.shapeType == shapeType);
            StartCoroutine(ChangeShapeCouroutine(currentShape, latestShape));
        }

        public void DisableShapes()
        {
            foreach (var shape in shapes)
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
            StartCoroutine(BotChangeShapeRoutine());
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
            currentShape = laterShape;

            StartCoroutine(prevShape.DisappearShape(0.5f));
            yield return new WaitForSeconds(0.25f);
            laterShape.transform.position = prevShape.transform.position;
            StartCoroutine(laterShape.AppearShape(0.5f));
        }


    }
}

