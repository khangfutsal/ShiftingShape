using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image imagePicker;
    private RawImage svImage;
    private ColorPickerControl CC;
    private RectTransform rectTransform, pickerTransform;

    private void Awake()
    {
        svImage = GetComponent<RawImage>();
        CC = FindObjectOfType<ColorPickerControl>();
        rectTransform = GetComponent<RectTransform>();
        pickerTransform = imagePicker.GetComponent<RectTransform>();
        pickerTransform.position = new Vector2(-(rectTransform.sizeDelta.x * 0.5f), -(rectTransform.sizeDelta.y * 0.5f));
    }

    public void UpdateColor(PointerEventData eventData)
    {
        Vector3 worldPos = eventData.pressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, rectTransform.position.z));
        Vector3 localPoint = rectTransform.InverseTransformPoint(worldPos);
        


        float deltaX = rectTransform.sizeDelta.x * 0.5f;
        float deltaY = rectTransform.sizeDelta.y * 0.5f;

        //// Giới hạn vị trí picker
        localPoint.x = Mathf.Clamp(localPoint.x, -deltaX, deltaX);
        localPoint.y = Mathf.Clamp(localPoint.y, -deltaY, deltaY);

        Debug.Log(deltaX);

        float x = localPoint.x + deltaX;
        float y = localPoint.y + deltaY;

        float xNorm = x / rectTransform.sizeDelta.x;
        float yNorm = y / rectTransform.sizeDelta.y;

        //// Đặt vị trí picker đúng cách
        //pickerTransform.localPosition = pos;
        pickerTransform.localPosition = localPoint;

        // Cập nhật màu theo Saturation (x) và Value (y)
        imagePicker.color = Color.HSVToRGB(CC.GetCurrentHue(), xNorm, 1 - yNorm);

        // Gửi dữ liệu đến Color Picker Control
        CC.SetSV(xNorm, yNorm);
    }


    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
