using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Connect4;
using JetBrains.Annotations;
namespace ShiftingShape
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<BaseShape> listShapes;
        [SerializeField] private BaseShape currentShape;
        [SerializeField] private Transform sphere;

        [SerializeField] private const float offsetYSphere = 0.45f;
        private Sequence sq;

        public List<BaseShape> GetListShapes() => listShapes;
        public BaseShape GetCurrentShape() => currentShape;

        private void Start()
        {
            currentShape = listShapes[0];
            if (sphere == null)
            {
                sphere = transform.Find("Sphere");
            }

            for (int i = 0; i < listShapes.Count; i++)
            {
                BaseShape shape = listShapes[i];
                shape.SubscribeEventPlayerControl();
            }

        }


        private void Update()
        {
            if (GameManager.Ins.GetGameState() == GameState.StartGame)
            {
                Vector3 vCurrentShape = new Vector3(currentShape.transform.position.x, currentShape.transform.position.y + offsetYSphere, currentShape.transform.position.z);
                sphere.transform.position = vCurrentShape;
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

        public void ChangeShape(ShapeType shapeType)
        {
            currentShape.TransitionShape();
            currentShape.isUsing = false;
            BaseShape shapeExpected = GetShape(shapeType);
            Debug.Log("Change : " + shapeType);
            if (sq != null)
            {
                //Debug.Log("KILl");
                sq.Kill();
            }
            sq = DOTween.Sequence();
            Vector3 scaleDefault = new Vector3(0.01f, 0.01f, 0.01f);
            Vector3 scaleInspire = Vector3.one;
            sphere.gameObject.SetActive(true);
            sq.Append(currentShape.transform.DOScale(scaleDefault, 0.3f));
            sq.Join(sphere.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f));
            sq.AppendCallback(() =>
            {
                //Debug.Log("Test");
                currentShape.gameObject.SetActive(false);
                
                shapeExpected.transform.position = currentShape.transform.position;
                currentShape = shapeExpected;
                currentShape.transform.rotation = Quaternion.Euler(0, currentShape.transform.rotation.y, currentShape.transform.rotation.z);

                shapeExpected.gameObject.SetActive(true);
                currentShape.isUsing = true;
                currentShape.CompleteShape();

            });
            //sq.AppendInterval(0.5f);
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

