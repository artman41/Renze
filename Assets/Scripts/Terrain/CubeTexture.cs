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

            var firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.FRONT));
            var rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Front  
            uvs[0] = rect.position; //new Vector2(0.0f, 0.0f);                                                  bl
            uvs[1] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                  br
            uvs[2] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                 tl
            uvs[3] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                    tr

            firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.BACK));
            rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Back
            uvs[10] = rect.position; //new Vector2(0.0f, 0.0f);                                                 bl
            uvs[11] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                 br
            uvs[6] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                 tl
            uvs[7] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                    tr

            firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.LEFT));
            rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Left
            uvs[16] = rect.position; //new Vector2(0.0f, 0.0f);                                                 bl
            uvs[19] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                 br
            uvs[17] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                tl
            uvs[18] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                   tr

            firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.RIGHT));
            rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Right        
            uvs[20] = rect.position; //new Vector2(0.0f, 0.0f);                                                 bl
            uvs[21] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                 br
            uvs[23] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                tl
            uvs[22] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                   tr

            firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.TOP));
            rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Top
            uvs[8] = rect.position; //new Vector2(0.0f, 0.0f);                                                  bl
            uvs[9] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                  br
            uvs[4] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                 tl
            uvs[5] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                    tr

            firstOrDefault = rects.FirstOrDefault(o => o.Face.Contains(CubeFaces.BOTTOM));
            rect = firstOrDefault?.Rect ?? rects[0].Rect;
            // Bottom
            uvs[12] = rect.position; //new Vector2(0.0f, 0.0f);                                                  bl
            uvs[14] = new Vector2(rect.position.x + rect.width, rect.position.y); //new Vector2(0.333f, 0.0f);                  br
            uvs[15] = new Vector2(rect.position.x, rect.position.y + rect.height); //new Vector2(0.0f, 0.333f);                 tl
            uvs[13] = new Vector2(rect.position.x + rect.width, rect.position.y + rect.height); //new Vector2(0.333f, 0.333f);                    tr

            mesh.uv = uvs;
            GetComponent<MeshFilter>().mesh = mesh;
            var mat = GetComponent<MeshRenderer>().material;
            mat.mainTexture = Atlas;
            mat.SetFloat("_Metallic", 1f);
            mat.SetFloat("_Glossiness", 0f);
        }
    }
}