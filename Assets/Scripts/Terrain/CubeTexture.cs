using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTexture : MonoBehaviour {

    public Texture2D[] Textures; //front, side, top
    public Texture2D Atlas;

    // Use this for initialization
    void Start () {
	    var mf = GetComponent<MeshFilter>();
        Mesh mesh = mf?.mesh;

	    if (mesh == null || mesh.uv.Length != 24) {
	        Debug.Log("Script needs to be attached to built-in Cube!");
	        return;
	    }

        Atlas = new Texture2D(2000, 2000);
        var rects = Atlas.PackTextures(Textures, 2, 2000);

	    var uvs = mesh.uv;

	    // Front  
        uvs[0] = rects[0].min;  //new Vector2(0.0f, 0.0f);
        uvs[1] = new Vector2(rects[0].width, 0.0f); //new Vector2(0.333f, 0.0f);
        uvs[2] = new Vector2(0.0f, rects[0].height); //new Vector2(0.0f, 0.333f);
	    uvs[3] = rects[0].max; //new Vector2(0.333f, 0.333f);

        /*
	    // Top
	    uvs[8] = new Vector2(0.334f, 0.0f);
	    uvs[9] = new Vector2(0.666f, 0.0f);
	    uvs[4] = new Vector2(0.334f, 0.333f);
	    uvs[5] = new Vector2(0.666f, 0.333f);

	    // Back
	    uvs[10] = new Vector2(0.667f, 0.0f);
	    uvs[11] = new Vector2(1.0f, 0.0f);
	    uvs[6] = new Vector2(0.667f, 0.333f);
	    uvs[7] = new Vector2(1.0f, 0.333f);

	    // Bottom
	    uvs[12] = new Vector2(0.0f, 0.334f);
	    uvs[14] = new Vector2(0.333f, 0.334f);
	    uvs[15] = new Vector2(0.0f, 0.666f);
	    uvs[13] = new Vector2(0.333f, 0.666f);

	    // Left
	    uvs[16] = new Vector2(0.334f, 0.334f);
	    uvs[18] = new Vector2(0.666f, 0.334f);
	    uvs[19] = new Vector2(0.334f, 0.666f);
	    uvs[17] = new Vector2(0.666f, 0.666f);

	    // Right        
	    uvs[20] = new Vector2(0.667f, 0.334f);
	    uvs[22] = new Vector2(1.00f, 0.334f);
	    uvs[23] = new Vector2(0.667f, 0.666f);
	    uvs[21] = new Vector2(1.0f, 0.666f);
        */
	    mesh.uv = uvs;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
