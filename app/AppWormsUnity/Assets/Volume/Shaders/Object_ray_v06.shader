Shader "Vxcm/Object/ray_v06"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,0)
		_Emission("Emissive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.1, 1)) = 0.7
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	// 1 texture stage GPUs
	SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		//Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#include "UnityCG.cginc"
		#define VXCM_OBJECT_VS
		#include "UnityVxcm.cginc"

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert //vertex:vert //fullforwardshadows
		#pragma fullforwardshadows alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform float _Cutoff;

		struct Input {
			float2 uv_MainTex;
			//float3 volumePos;
			float4 localPos;
			INTERNAL_DATA
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//o.volumePos = mul(u_objectToVolumeTrx, v.vertex);
			o.localPos = v.vertex;
		}


		void surf(Input IN, inout SurfaceOutput  o) {
			// Albedo comes from a texture tinted by color

			float4 volumePos = mul(u_objectToVolumeTrx, IN.localPos);

			float3 rayOriginTex = volumePos;
			float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
			float3 rayDirTex = normalize(IN.localPos - localCameraPos.xyz);

			float4 texcol = raycast(rayOriginTex, rayDirTex);

		/*	if (texcol.a > 0)
			{
			
				// get normal in object space
				float4 samplePos = float4(rayOriginTex + rayDirTex * texcol.x, 1);

				float4 objCoord = mul(u_objectToVolumeInvTrx, samplePos);
				float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
			//	o.depth = vpos.z / vpos.w;

			//	float3 lightDir = normalize(ObjSpaceLightDir(objCoord));

				float3 N = normalize(calcNormal(samplePos.xyz));

				o.Normal = N;

				o.Albedo = N;
			}
			else
				o.Albedo = float3(1, 1, 0);
				*/

			//o.Normal = float3(0, 1, 0);
			o.Albedo = float3(1,1,1);
			o.Alpha = texcol.a;

			// ---------------

			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = texcol.rgb;
			//o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
		//	o.Metallic = _Metallic;
		//	o.Smoothness = _Glossiness;
		//	o.Alpha = c.a;

		//	clip(texcol.a*_Color.a - _Cutoff);

		//	o.Albedo = float3(1, 1, 0);
			//o.Metallic = 0.1;
			//o.Alpha = 1;
			//o.Smoothness = 0.5;
		}
		ENDCG
	}

	Fallback "Vxcm/Base"
}