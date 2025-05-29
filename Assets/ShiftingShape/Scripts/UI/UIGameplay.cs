using System.Collections;
using System.Collections.Generic;
using System.Timers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
namespace ShiftingShape
{
    public class UIGameplay : MonoBehaviour
    {
        [SerializeField] private List<BaseShape> shapes;
        [SerializeField] private Transform pointSpawnHorz;
        [SerializeField] private GameObject btnChoicePref;
        [SerializeField] private int numShapes;
        [SerializeField] private List<BaseShape> listShapes;
        [SerializeField] private int count;
        [SerializeField] private GameObject panel;

        [Header("About Countdown")]
        [SerializeField] private TextMeshProUGUI textCountdown;
        [SerializeField] private float textCountdownTime;
        [SerializeField] private int numberLimit;
        private Coroutine textCountdownCorou;

        [Header("About Button")]
        [SerializeField] private Sprite targetSprite;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Button curButton;


        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            count = 1;
            float initTime = 0.1f;
            shapes = ShapeManager.Ins.GetListShapes();
            for (int i = 1; i <= numShapes; i++)
            {
                int index = i;
                GameObject obj = Instantiate(btnChoicePref, pointSpawnHorz);
                Button btnChoice = obj.GetComponent<Button>();
                Image img = btnChoice.transform.Find("Image").GetComponent<Image>();

                Debug.Log("Shape Gameplay : " + listShapes[index].name);
                btnChoice.image.sprite = defaultSprite;
                btnChoice.onClick.AddListener(() =>
                {

                    ButtonShape(listShapes[index],btnChoice);
                });

                StartCoroutine(RandomChoice(img, initTime, listShapes[index].shapeSO.shapeData.sprite));


                initTime += 0.2f;
            }

            textCountdown.gameObject.SetActive(false);
        }

        public void IntializeList(List<BaseShape> listShapes)
        {
            this.listShapes = listShapes;
        }

        public void ButtonShape(BaseShape baseShape,Button btn)
        {
            if (curButton != null)
            {
                curButton.image.sprite = defaultSprite;
            }
            curButton = btn;
            btn.image.sprite = targetSprite;

            Debug.Log("Change Type : " + baseShape);
            if (!baseShape.isUsing)
            {
                ShapeType shapeType = baseShape.shapeSO.shapeData.shapeType;
                Player player = GameManager.Ins.GetPlayer();
                player.ChangeShape(shapeType);
                
            }
        }


        public IEnumerator RandomChoice(Image img, float time, Sprite spriteShape)
        {
            List<int> ints = new List<int>();
            int randomShapes;
            while (time >= 0)
            {
                if (ints.Count == shapes.Count)
                {
                    ints.Clear();
                }

                do
                {
                    randomShapes = Random.Range(0, shapes.Count);
                } while (ints.Contains(randomShapes));
                ints.Add(randomShapes);


                Sprite sprite = shapes[randomShapes].shapeSO.shapeData.sprite;
                img.sprite = sprite;

                time -= Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
                yield return null;
            }
            img.sprite = spriteShape;
            count++;
            yield return new WaitUntil(() => count == listShapes.Count);
            Debug.Log("Test :" + textCountdownCorou);
            if (textCountdownCorou == null)
            {
                textCountdownCorou = StartCoroutine(StartCountdown(textCountdownTime));
            }

        }
        public IEnumerator StartCountdown(float time)
        {
            Debug.Log("Khanggg");
            textCountdown.gameObject.SetActive(true);
            for (int i = numberLimit; i > 0; i--)
            {
                textCountdown.text = i.ToString();
                textCountdown.transform.localScale = Vector3.zero;
                textCountdown.DOFade(1, 0f);
                textCountdown.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    textCountdown.DOFade(0, 0.5f);
                });
                Debug.Log("" + i);

                yield return new WaitForSeconds(time);
            }
            panel.SetActive(false);
            GameManager.Ins.StartGame();
        }

    }
}

