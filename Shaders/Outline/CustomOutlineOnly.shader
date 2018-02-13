// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Outlined/CustomOutlineOnly" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0, 1)) = .1
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
 
v2f vert(appdata v) {
	// just make a copy of incoming vertex data but scaled according to normal direction
	v2f o;

	v.vertex *= ( 1 + _Outline);

	o.pos = UnityObjectToClipPos(v.vertex);
 
	//float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
	//float2 offset = TransformViewToProjection(norm.xy);

	o.color = _OutlineColor;
	return o;
}
ENDCG
 
	SubShader {
		//Tags {"Queue" = "Geometry+100" }
 
		Pass {
			Name "BASE"
			Cull Back
			Blend Zero One

			// uncomment this to hide inner details:
			//Offset -8, -8

			SetTexture[_OutlineColor] {
				ConstantColor(0,0,0,0)
				Combine constant
			}
		}

		// note that a vertex shader is specified here but its using the one above
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			//Offset 50,50
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
	}
 
	Fallback "Diffuse"
}
