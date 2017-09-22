using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Camera;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor {
    [CustomEditor(typeof(CameraScreenShot))]
    public class ScreenShotEditor : UnityEditor.Editor {

        private bool Screenshots;

        public override void OnInspectorGUI() {
            var targ = (CameraScreenShot)target;

            targ.Screenshot = (ScreenshotType)EditorGUILayout.EnumPopup("Screenshot Type", targ.Screenshot);
            if (targ.Screenshot == ScreenshotType.Single) {
                targ.TargetCamera = targ.AllCameras[EditorGUILayout.Popup("Target Camera", Array.IndexOf(targ.AllCameras, targ.TargetCamera), targ.AllCameras.Select(o => o.name).ToArray())];
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Resolution", GUILayout.MaxWidth(149));
            var x = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40;
            targ.Resolution.x = EditorGUILayout.IntField("Width", targ.Resolution.x);

            targ.Resolution.y = EditorGUILayout.IntField("Height", targ.Resolution.y);
            EditorGUIUtility.labelWidth = x;
            GUILayout.EndHorizontal();

            targ.ScreenshotDirectory = EditorGUILayout.TextField("Screenshot Directory", targ.ScreenshotDirectory);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Take Screenshot")) {
                targ.ScreenShot();
            }
            if (GUILayout.Button("Open Directory")) {
                EditorUtility.RevealInFinder(targ.Screenshots?[0].FullName ?? targ.ScreenshotDirectory);
            }
            GUILayout.EndHorizontal();

            Screenshots = EditorGUILayout.Foldout(Screenshots, "Screenshots");
            if (Screenshots) {
                targ.Screenshots.ForEach(o => {
                    if (GUILayout.Button(o.Name)) {
                        Process.Start(o.FullName);
                    }
                });
            }
        }
    }
}
