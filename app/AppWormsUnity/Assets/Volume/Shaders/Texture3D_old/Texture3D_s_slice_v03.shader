Shader "Vxcm/Texture3D/old/slice_v03"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
	}
	
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		#include "../UnityVxcm.cginc"

		struct Input {
			float4 screenPos;
			float3 localUVW; // custom
		};
			
		uniform float4x4 u_objectToVolumeTrx;
		uniform float4x4 u_vxcm_worldViewMatrix;
		// w, h , nearplane
		uniform float3 u_cameraInfo;
		sampler3D _Volume;

		// =====================

		void vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			data.localUVW = mul(u_objectToVolumeTrx, v.vertex).xyz;
		}

		// =====================

		float4 getClipMapTexture(int level, float3 sampleLevelPos) {
			//float3 pos = frac(sampleLevelPos ); // CYCLIC
			// devo usare la forma lod altrimenti non capisce che mipmap utilizzare
			return tex3Dlod(_Volume, float4(sampleLevelPos, 0));
		}

		void vxcm_getPixelRay(Input IN, out float3 rayDirTex, out float3 rayOriginTex)
		{
			half2 screenuv = IN.screenPos.xy / IN.screenPos.w;
	#ifdef SHADER_API_D3D9
				if (_ProjectionParams.x > 0)
				{
					screenuv.y = 1.0f - screenuv.y;
				}
	#endif

			float2 v_position = screenuv * 2.0 - 1.0;
			// in camera space
			float3 viewDirVS = float3(v_position  * u_cameraInfo.xy, u_cameraInfo.z);

			rayDirTex = normalize(mul(u_vxcm_worldViewMatrix, float4(viewDirVS, 1.0)).xyz);

			rayOriginTex = IN.localUVW;
		}

		// =====================

		void surf(Input IN, inout SurfaceOutput o) {

			float3 rayDirTex, rayOriginTex;
			vxcm_getPixelRay(IN, rayDirTex, rayOriginTex);

			float3 min = float3(0, 0, 0);
			float3 max = float3(0.5, 0.5, 0.5);
			float tEnter = 0.0;
			float tLeave = 0.0;

			//rayOriginTex = rayOriginTex - rayDirTex * 0.1;
			//return float4(rayDirTex, 1);

			if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
			{
				//clip(frac((IN.worldPos.y + IN.worldPos.z*0.1) * 5) - 0.5);
				//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
				//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Albedo = float4(rayOriginTex, 1);
			}
			else
				clip(-1);
		}
		ENDCG
	}
	
	Fallback "Diffuse"
}