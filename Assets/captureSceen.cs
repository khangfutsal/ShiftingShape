namespace BritneyAdventure
{

    using System.Collections;
    using System.Collections.Generic;
#if UNITY_EDITOR

    using UnityEditor;
    using UnityEngine;

    public class captureSceen : MonoBehaviour
    {
        public static captureSceen _ins;
        public Camera cameraFixed;
        private bool isCreateFolder = false;
        public bool isCapturePortrait = false;
        public string name;

        private void Awake()
        {
            _ins = this;
        }

        private void OnValidate()
        {
            string folderPath = "Assets/InfoGame/"; // the path of your project folder
            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
            {
                AssetDatabase.CreateFolder("Assets", "InfoGame");
                AssetDatabase.Refresh();
            }
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{ // capture screen shot on space key down
            //    CaptureScreenshots();
            //}
            //else
            //{
            //    if (Input.GetKeyDown(KeyCode.M))
            //    {
            //        CaptureIcon();
            //    }
            //}
        }

        [ContextMenu("CaptureIcon")]
        void CaptureIcon()
        {
            string folderPath = "Assets/InfoGame/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created
            CaptureScreenshot(folderPath, "Icon_512", 512, 512);
        }

        [ContextMenu("CaptureScreenshots")]
        public void CaptureScreenshots()
        {
            string folderPath = "Assets/InfoGame/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created


            CaptureScreenshot(folderPath, "Image", 1920, 1080);

            // else
            // {
            //     var screenshotName =
            //                         "Screenshot_" +
            //                         System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
            //                         ".png"; // put your favorite data format here
            //
            //     ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 1); // takes the screenshot
            //     AssetDatabase.Refresh();
            //     Debug.Log("Capture Screenshot Name " + screenshotName);
            // }
        }

        void CaptureScreenshot(string folderPath, string resolutionName, int width, int height)
        {
            var renderTexture = new RenderTexture(width, height, 24);
            var screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

            cameraFixed.targetTexture = renderTexture;
            cameraFixed.Render();

            RenderTexture.active = renderTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshotTexture.Apply();

            cameraFixed.targetTexture = null;
            RenderTexture.active = null;

            Destroy(renderTexture);

            var screenshotName =
                                resolutionName + "_" +
                                System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                ".png";

            var screenshotPath = System.IO.Path.Combine(folderPath, name);
            System.IO.File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());
            AssetDatabase.Refresh();
            //Debug.Log("Captured Screenshot: " + screenshotPath);
        }
    }
#endif
}
