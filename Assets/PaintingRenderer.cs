using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PaintingRenderer : MonoBehaviour
{
    public Camera screenshotCamera; // Camera chuyên dùng để chụp

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // capture screen shot on left mouse button down

        
            //var screenshotName =
            //                        "Screenshot_" +
            //                        System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
            //                        ".png"; // put youre favorite data format here
            //ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            //Debug.Log(folderPath + screenshotName); // You get instant feedback in the console
        }
    }

    [ContextMenu("ScreenShot")]
    public void Test()
    {
        string folderPath = "Assets/Screenshots/"; // the path of your project folder

        if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
            System.IO.Directory.CreateDirectory(folderPath);  // it will get created
        TakeScreenshot(folderPath, 1920, 1080);
    }

    
    void TakeScreenshot(string nameFile, int width,int height)
    {

        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(nameFile + "/Screenshot.png", bytes);

        Debug.Log("Screenshot saved!");
    }

}
