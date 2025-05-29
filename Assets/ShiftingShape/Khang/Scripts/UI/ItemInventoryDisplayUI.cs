using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Khang
{
    public class ItemInventoryDisplayUI : MonoBehaviour
    {
        public TextMeshProUGUI txtItemDisplay;
        public RawImage rawImage;
        public ItemDisplay itemDisplay;
        public Vector3 vMouseCenter;
        public float xCurrentMouse;
        public bool isClicked;
        public float rotateSpeed = 0.2f;


        private void Awake()
        {
            txtItemDisplay = GetComponentInChildren<TextMeshProUGUI>();
            rawImage = GetComponentInChildren<RawImage>();
        }

        public void SetItemDisplay(ItemDisplay target) => itemDisplay = target;


        private bool IsClickOnRawImage()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (rawImage.gameObject == result.gameObject)
                {
                    return true;
                }
            }
            return false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsClickOnRawImage())
                {
                    vMouseCenter = Input.mousePosition;
                    isClicked = true;
                }
            }

            if (Input.GetMouseButton(0) && isClicked)
            {
                if (IsClickOnRawImage())
                {
                    float deltaX = Input.GetAxis("Mouse X");
                    Debug.Log($"Delta X {deltaX}");
                    if (deltaX > 0)
                    {
                        Debug.Log("Mouse moving to the right");
                        itemDisplay.Rotation(deltaX * rotateSpeed);
                    }
                    else if (deltaX < 0)
                    {
                        Debug.Log("Mouse moving to the left");
                        itemDisplay.Rotation(deltaX * rotateSpeed);
                    }
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                isClicked = false;
            }
        }
    }

}
