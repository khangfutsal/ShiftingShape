using ShiftingShape;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
namespace Khang
{
    public class SetupUI : MonoBehaviour
    {
        [Header("Multi Choice UI")]
        [SerializeField] private Transform multiChoiceTf;
        [SerializeField] private int countChoice = 3;
        [SerializeField] private GameObject btnObj;

        [SerializeField] private GameObject panelObj;

        [SerializeField] private UnityEvent onTransitionSuccess = new UnityEvent();

        [SerializeField] private Player player;

        private void Awake()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }


        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            List<ShapeData> shapesData = LevelManager.Ins.CurShapesInLevel;
            float time = 0.5f;
            int finishedCount = 0;


            panelObj.SetActive(true);

            for (int i = 0; i < countChoice; i++)
            {
                GameObject buttonObj = Instantiate(btnObj, multiChoiceTf);
                Button btn = buttonObj.GetComponent<Button>();

                StartCoroutine(TransitionButton(btn, time, shapesData[i], () =>
                {
                    finishedCount++;
                    if (finishedCount >= countChoice)
                    {
                        onTransitionSuccess?.Invoke();
                    }
                }));
                time += 0.5f;
            }

        }

        public IEnumerator TransitionButton(Button btn, float time, ShapeData shapeData, System.Action onComplete)
        {
            List<Sprite> allShapeSprites = DataManager.Ins.ShapesData
                .Select(shape => shape.SpriteShape)
                .ToList();

            Image imageButton = btn.transform.Find("Image").GetComponent<Image>();
            float timer = 0f;

            while (timer < time)
            {
                timer += Time.deltaTime;
                Sprite randomSprite = allShapeSprites[UnityEngine.Random.Range(0, allShapeSprites.Count)];
                imageButton.sprite = randomSprite;
                yield return new WaitForSeconds(0.05f);
            }

            imageButton.sprite = shapeData.SpriteShape;

            btn.onClick.AddListener(() =>
            {
                player.ChangeShape(shapeData.shapeType);
            });

            onComplete?.Invoke();
            yield return null;
        }
    }

}
