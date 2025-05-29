using UnityEngine;
using System.Collections;
using System.IO;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using UnityEngine.UI;
using BritneyAdventure;
public class AdjustionUI : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Slider sliderThickness;

    private void Start()
    {
        sliderThickness.onValueChanged.AddListener((float value) =>
        {
            Painting._ins.lineThickness = value;
        });
    }


    public void SaveImage()
    {
        captureSceen._ins.CaptureScreenshots();
    }

    public void OpenFile()
    {
        var bp = new BrowserProperties();
        //bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg, *.jpeg, *.jpe, *.jfif, *.png";
        bp.filter = "PNG Image (*.png) | *.png";
        bp.filterIndex = 0;
        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            StartCoroutine(LoadFile(path));
        });
    }
    public IEnumerator LoadFile(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                Texture2D texture2D = uwrTexture as Texture2D;
                if (texture2D != null)
                {
                    // Tạo Sprite từ Texture2D
                    Sprite newSprite = Sprite.Create(texture2D,
                        new Rect(0, 0, texture2D.width, texture2D.height),
                        new Vector2(0.5f, 0.5f));

                    // Gán vào Image
                    image1.sprite = newSprite;
                    image2.sprite = newSprite;
                }
            }
        }
    }
}