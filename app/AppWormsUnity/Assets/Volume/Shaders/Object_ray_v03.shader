Shader "Vxcm/Object/ray_v03"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Volume("Volume (Scalar)", 3D) = "white"{}
	}
	SubShader {
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200
		
		// ===============================================================

		CGPROGRAM
		#include "UnityCG.cginc"

		#define VXCM_OBJECT_VS
		#include "UnityVxcm.cginc"

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float4 screenPos;
			float3 worldPos;
		};


		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alphatest:_Cutoff   addshadow  fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//float3 uvw = mul(u_worldToVolumeTrx, float4(IN.worldPos,1));

			//raycast(IN, o);

			float4 rayOriginTex = mul(u_worldToVolumeTrx, float4(IN.worldPos, 1));

			float2 wcoord = (IN.screenPos.xy/ IN.screenPos.w);
			float2 v_position = wcoord * 2.0 - 1.0;
			float3 viewDirVS = float3(v_position  * u_cameraInfo.xy, u_cameraInfo.z);

			float3 rayDirTex = normalize(mul(u_vxcm_worldViewMatrix, float4(viewDirVS, 1.0)).xyz);

			float4 color = raycast(rayOriginTex, rayDirTex);

			// ---------------------

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		//	o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		//	o.Alpha = c.a;

			o.Albedo = color.rgb;
			o.Alpha = color.a;
		}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}
