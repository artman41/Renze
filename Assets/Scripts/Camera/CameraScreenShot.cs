using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Camera {
    [Serializable]
    public class Resolution {
        public int x;
        public int y;

        public Resolution(int a, int b) {
            x = a;
            y = b;
        }
    }

    public enum ScreenshotType {
        Single, Multi
    }

    [RequireComponent(typeof(CameraShader))]
    public class CameraScreenShot : MonoBehaviour {
        private UnityEngine.Camera Camera => UnityEngine.Camera.main;
        private UnityEngine.Camera[] OtherCameras => GetComponent<CameraShader>().otherCameras;
        public UnityEngine.Camera[] AllCameras => GetCameras();
        public FileInfo[] Screenshots => GetScreenshots();

        private bool TakeScreenshot;

        public ScreenshotType Screenshot;
        public UnityEngine.Camera TargetCamera;
        public Resolution Resolution;
        public bool UseGameWindowSize;
        public String ScreenshotDirectory = "./ScreenShots/";

        public string ScreenshotPath => Path.Combine(Application.persistentDataPath, ScreenshotDirectory);

        private void Update() {
            if (Input.GetKey(KeyCode.K) && !TakeScreenshot) {
                ScreenShot();
            }
        }

        public void ScreenShot() => TakeScreenshot = true;

        private void LateUpdate() {
            if (TakeScreenshot) {
                var ub = UseGameWindowSize ? new Resolution(Screen.width, Screen.height) : Resolution;
                var rt = new RenderTexture(ub.x, ub.y, 16);
                TargetCamera.targetTexture = rt;
                TargetCamera.Render();
                var screenShot = new Texture2D(ub.x, ub.y, TextureFormat.RGB24, false);
                TargetCamera.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, ub.x, ub.y), 0, 0);
                TargetCamera.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                var name = $"Screenshot_{Application.productName}-{Application.version}_{DateTime.Now:H-mm-ss}.png";
                Directory.CreateDirectory(ScreenshotPath);
                File.WriteAllBytes($"{ScreenshotPath}{name}", bytes);
                Debug.Log($"Saved Screenshot {name} to {ScreenshotPath.Replace("\\.", "")}");
                TakeScreenshot = false;
            }
        }

        UnityEngine.Camera[] GetCameras() {
            return new[] {Camera}.Concat(OtherCameras).ToArray();
        }

        FileInfo[] GetScreenshots() {
            return new DirectoryInfo(ScreenshotPath).EnumerateFiles().Where(o => o.Extension == ".png").ToArray();
        }
    }
}
