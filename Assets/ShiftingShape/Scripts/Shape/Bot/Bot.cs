using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
namespace ShiftingShape
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private List<BaseShape> listShapes;
        [SerializeField] private BaseShape currentShape;
        [SerializeField] private Transform sphere;

        [SerializeField] private const float offsetYSphere = 0.45f;
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;
        [SerializeField] private float curTime;

        private void Start()
        {
            currentShape = listShapes[0];
            if (sphere == null)
            {
                sphere = transform.Find("Sphere");
            }
            RandomTime();
        }

        private void Update()
        {
            if (GameManager.Ins.GetGameState() == GameState.StartGame)
            {
                Vector3 vCurrentShape = new Vector3(currentShape.transform.position.x, currentShape.transform.position.y + offsetYSphere, currentShape.transform.position.z);
                sphere.transform.position = vCurrentShape;
                DecisionChoice();
            }
        }

        public void InitializeList(List<BaseShape> listShapes)
        {
            this.listShapes.Clear();

            for (int i = 0; i < listShapes.Count; i++)
            {
                this.listShapes.AddRange(transform.GetComponentsInChildren<BaseShape>(true).Where(_ => _.GetType() == listShapes[i].GetType()));
            }


        }

        public void RandomTime()
        {
            curTime = (int)Random.Range(startTime, endTime);
        }

        public void DecisionChoice()
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                while (true)
                {
                    int choice = Random.Range(1, listShapes.Count);
                    BaseShape shape = listShapes[choice];
                    if (!shape.isUsing)
                    {
                        ShapeType shapeType = shape.shapeSO.shapeData.shapeType;
                        ChangeShape(shapeType);
                        RandomTime();
                        break;
                    }
                }
            }
        }

        public ShapeType GetShapeByIndex(int choice)
        {
            switch (choice)
            {
                case 0: return ShapeType.Human;
                case 1: return ShapeType.Bike;
                case 2: return ShapeType.Car;
                case 3: return ShapeType.Boat;
                case 4: return ShapeType.Heli;
            }
            return ShapeType.None;
        }

        public void ChangeShape(ShapeType shapeType)
        {
            currentShape.TransitionShape();
            BaseShape shapeExpected = GetShape(shapeType);

            Sequence sq = DOTween.Sequence();
            Vector3 scaleDefault = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 scaleInspire = Vector3.one;
            sphere.gameObject.SetActive(true);
            sq.Append(currentShape.transform.DOScale(scaleDefault, 1f));
            sq.Join(sphere.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f));
            sq.AppendCallback(() =>
            {
                currentShape.gameObject.SetActive(false);
                currentShape.isUsing = false;

                shapeExpected.transform.position = currentShape.transform.position;
                currentShape = shapeExpected;
                currentShape.isUsing = true;
                shapeExpected.gameObject.SetActive(true);


            });
            sq.AppendInterval(0.5f);
            sq.Append(shapeExpected.transform.DOScale(scaleInspire, 0.5f));
            sq.Append(sphere.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f)).OnComplete(() =>
            {
                sphere.gameObject.SetActive(false);
            });
        }

        public BaseShape GetShape(ShapeType shapeType)
        {
            return listShapes.Find(_ => _.shapeSO.shapeData.shapeType == shapeType);
        }
    }
}

