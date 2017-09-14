Shader "Renze/8BitShader" {
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			ColorMask A
			Color(1,1,1,1)
		}
	}
}