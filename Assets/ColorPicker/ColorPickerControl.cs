using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerControl : MonoBehaviour
{
    public float currentHue, currentSat, currentVal;

    [SerializeField] private RawImage hueImage, satValImage, outputImage;
    [SerializeField] private Slider hueSlider;
    private Texture2D hueTexture, svTexture, outputTexture;
    [SerializeField] private Material changeThisColor;


    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();

        UpdateOutputImage();
    }

    public float GetCurrentHue()
    {
        return currentHue;
    }


    public void CreateHueImage()
    {
        int height = 256; // Độ phân giải cao hơn
        hueTexture = new Texture2D(1, height);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < height; i++)
        {
            float hue = (float)i / (height - 1);
            hueTexture.SetPixel(0, i, Color.HSVToRGB(hue, 1, 1)); // Tăng V lên 1 để màu tươi
        }

        hueTexture.Apply();
        currentHue = 0;
        hueImage.texture = hueTexture;
    }

    public void CreateSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SalValTexture";
        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float)x / svTexture.width, (float)y / svTexture.height));

            }
        }
        svTexture.Apply();
        currentSat = 0;
        currentVal = 0;
        satValImage.texture = svTexture;
    }

    public void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color curColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, curColor);
        }
        outputTexture.Apply();
        outputImage.texture = outputTexture;
    }

    public void UpdateOutputImage()
    {
        Color curColor = Color.HSVToRGB(currentHue, currentSat, currentVal);
        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, curColor);
        }
        outputTexture.Apply();
        changeThisColor.SetColor("_BaseColor", curColor);
    }

    public void SetSV(float s, float v)
    {
        currentSat = s;
        currentVal = v;
        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = hueSlider.value;
        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float)x / svTexture.width, (float)y / svTexture.height));
            }
        }
        svTexture.Apply();
        UpdateOutputImage();
    }
}
