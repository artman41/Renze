/*
 * Link to source: https://gist.github.com/wtrebella/421e698ff7adb7143bf8
 * 
 */
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraScreenGrab : MonoBehaviour {
	
	//how chunky to make the screen
	protected int pixelSize;

	[HideInInspector] public int gridHeight;
	public int gridWidth = 32;
	public FilterMode filterMode = FilterMode.Point;
	public Camera[] otherCameras;
	private Material mat;
    private Camera Camera => Camera.main; //Created a pointer to MainCam to prevent changes to previous code

    Texture2D tex;
	
	void Start () {
		gridHeight = (int)(gridWidth * ((float)Screen.height / (float)Screen.width));

		pixelSize = Screen.width / gridWidth;

		Camera.pixelRect = new Rect(0,0,Screen.width/pixelSize,Screen.height/pixelSize);

		for (int i = 0; i < otherCameras.Length; i++)
			otherCameras[i].pixelRect = new Rect(0,0,Screen.width/pixelSize,Screen.height/pixelSize);
	}
	
	void OnGUI()
	{
		if (Event.current.type == EventType.Repaint)
			// i do NOT understand why i have to add an extra pixelSize to the height
			// but if i don't, the screen cuts off the bottom pixel. anyone care to explain
			// why this is necessary?
			Graphics.DrawTexture(new Rect(0,0,Screen.width, Screen.height + pixelSize), tex);
	}
	
	
	void OnPostRender()
	{
		if(!mat) {
            /*mat = new Material( "Shader \"Hidden/SetAlpha\" {" +
			                   "SubShader {" +
			                   "	Pass {" +
			                   "		ZTest Always Cull Off ZWrite Off" +
			                   "		ColorMask A" +
			                   "		Color (1,1,1,1)" +
			                   "	}" +
			                   "}" +
			                   "}"
			                   );*/  //ORIGINAL SHADER 
		    mat = new Material(Shader.Find("Custom/8BitShader")); //Shader Corrected for Unity 2017
		}
		// Draw a quad over the whole screen with the above shader
		GL.PushMatrix ();
		GL.LoadOrtho ();
		for (var i = 0; i < mat.passCount; ++i) {
			mat.SetPass (i);
			GL.Begin( GL.QUADS );
			GL.Vertex3( 0, 0, 0.1f );
			GL.Vertex3( 1, 0, 0.1f );
			GL.Vertex3( 1, 1, 0.1f );
			GL.Vertex3( 0, 1, 0.1f );
			GL.End();
		}
		GL.PopMatrix ();	
		
		DestroyImmediate(tex);

		tex = new Texture2D(Mathf.FloorToInt(Camera.pixelWidth), Mathf.FloorToInt(Camera.pixelHeight));
		tex.filterMode = filterMode;
		tex.ReadPixels(new Rect(0, 0, Camera.pixelWidth, Camera.pixelHeight), 0, 0);
		tex.Apply();
	}
	
}