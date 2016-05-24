Shader "Vxcm/Pick/ray_v01"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "black" {}
	//	_Volume("Volume (Scalar)", 3D) = "white"{}
	//	_SampleRate("Sample rate",float) = 3
	}

	Subshader {

		Pass{
	
			ZTest Off
			Cull Off
			ZWrite Off
			Fog{ Mode off }

			CGPROGRAM

			#include "UnityCG.cginc"
			#define VXCM_RAYCAST_VS
			//#include "UnityVxcm.cginc"

			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			
			float _SampleRate;
			sampler2D _MainTex;

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			//Common Vertex Shader
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			float4 frag(v2f IN) : COLOR
			{
				half2 ScreenUV = IN.uv;

				float4 v = tex2D(_MainTex, ScreenUV);

				//return float4(1, 0, 0, 1);
				//return float4(ScreenUV, 0, 1);
				return v;
			}

			ENDCG
		}
	}

} // shader