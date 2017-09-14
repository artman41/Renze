using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Attributes;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Terrain {
    public enum CubeFaces {
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    public class CubeTexture : MonoBehaviour {

        [Serializable]
        public class TextureClassifier {
            public Texture2D Item1;
            public CubeFaces[] Item2;

            public TextureClassifier(Texture2D t, CubeFaces[] c) {
                Item1 = t;
                Item2 = c;
            }
        }

        [Serializable]
        public class RectClassifier {
            public Rect Rect;
            public CubeFaces[] Face;

            public RectClassifier(Rect t, CubeFaces[] c) {
                Rect = t;
                Face = c;
            }
        }

        public TextureClassifier[] Textures; //front, side, top
        [ReadOnly] public Texture2D Atlas;
        [ReadOnly] public List<RectClassifier> rects;
        /// <summary>
        /// Front, Back, Left, Right, Top, Bottom
        /// </summary>
        public readonly Tuple<int[], CubeFaces>[] uvOrder = {
            new Tuple<int[], CubeFaces>(new int[] {0, 1, 2, 3}, CubeFaces.FRONT),
            new Tuple<int[], CubeFaces>(new int[] {10, 11, 6, 7}, CubeFaces.BACK),
            new Tuple<int[], CubeFaces>(new int[] {16, 19, 17, 18}, CubeFaces.LEFT),
            new Tuple<int[], CubeFaces>(new int[] {20, 21, 23, 22}, CubeFaces.RIGHT),
            new Tuple<int[], CubeFaces>(new int[] {8, 9, 4, 5}, CubeFaces.TOP),
            new Tuple<int[], CubeFaces>(new int[] {12, 14, 15, 1}, CubeFaces.BOTTOM)
        };

        // Use this for initialization
        void Start() {
            var mf = GetComponent<MeshFilter>();
            Mesh mesh = mf?.mesh;

            if (mesh == null || mesh.uv.Length != 24) {
                Debug.Log("Script needs to be attached to built-in Cube!");
                return;
            }

            Atlas = new Texture2D(1000, 1000);
            rects = new List<RectClassifier>();

            var y = Atlas.PackTextures(Textures.Select(o => o.Item1).ToArray(), 0);

            for (int i = 0; i < Textures.Length; i++) {
                rects.Add(new RectClassifier(y[i], Textures[i].Item2));
            }

            var uvs = mesh.uv;

            foreach (var item in uvOrder) {
                var rect = rects.FirstOrDefault(o => o.Face.Contains(item.Item2))?.Rect ?? rects[0].Rect;

                uvs[item.Item1[0]] = rect.position;
                uvs[item.Item1[1]] = new Vector2(rect.position.x + rect.width, rect.position.y);
                uvs[item.Item1[2]] = new Vector2(rect.position.x, rect.position.y + rect.height);
                uvs[item.Item1[3]] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height);
            }

            mesh.uv = uvs;
            GetComponent<MeshFilter>().mesh = mesh;
            var mat = GetComponent<MeshRenderer>().material;
            mat.mainTexture = Atlas;
            mat.SetFloat("_Metallic", 1f);
            mat.SetFloat("_Glossiness", 0f);
        }
    }
}