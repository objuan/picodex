Shader "Vxcm/Texture3D/old/slice_v02"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
		// Shadow Stuff
		_ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.6
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	#define VXCM_OBJECT_VS
	#include "../UnityVxcm.cginc"

	sampler3D _Volume;

	uniform float u_sample_rate;
	uniform float u_cut_plane_xz;

	// sampleLevelPos  are in texture level space
	float4 getClipMapTexture(int level, float3 sampleLevelPos) {
		//float3 pos = frac(sampleLevelPos ); // CYCLIC
		// devo usare la forma lod altrimenti non capisce che mipmap utilizzare
		return tex3Dlod(_Volume, float4(sampleLevelPos,0));
	}

	float4 raycast(in int layerIdx,
		in float3 enter,
		in float3 leave,
		in float3 dir,
		float clipPlaneY,
		inout float4 accum,
		out int count)
	{
		count = 0;
		float stepLength = length(leave - enter);
		float stepSize = u_sample_rate;//1.0 / u_resolution;
									   //float stepSize = 0.33 / u_resolution;
									   //float stepSize = 0.5f / 64.0f;
									   //float iso_level = 1.0 / 1024;

	//	accum = float4(stepLength, stepLength, stepLength, 1);
	//	return accum;

		float fSampleCount = stepLength / stepSize;
		int iSampleCount = int(ceil(fSampleCount));

		//int debugLayer = u_debug_layer;

		//	float4 returnColor = float4(0);
		[loop] for (int i = 0; i < iSampleCount; ++i) {
			float3 samplePos = enter + dir * stepSize * float(i);
			count++;

			float4 voxelColor = getClipMapTexture(layerIdx, samplePos);

			if (voxelColor.a>0 && samplePos.y < u_cut_plane_xz)
			{
				accum += (1 - accum.a) * voxelColor;
				if (accum.a > 0.99) return accum;
			}
		}
		return accum;
	}

	

	half4 frag(appdata_vxcm_object i) : COLOR
	{
		
		float3 rayDirTex, rayOriginTex;
		vxcm_getPixelRay(i, rayDirTex, rayOriginTex);
			
		float3 min = float3(0,0,0);
		float3 max = float3(1,1,1);
		float tEnter = 0.0;
		float tLeave = 0.0;

		//rayOriginTex = rayOriginTex - rayDirTex * 0.1;
		//return float4(rayDirTex, 1);

		if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
		{
			float3 enter, exit;
			int count = 0;
			float clipPlaneY = 0;
			float4 finalColor = float4(0,0,0,0);

			enter = rayOriginTex + tEnter * rayDirTex;
			exit = rayOriginTex + tLeave * rayDirTex;

			//finalColor = float4(1, 1, 0, 1);

			finalColor =  raycast(0, enter, exit, rayDirTex, clipPlaneY, finalColor, count);

			// decode color
			if (finalColor.a>0)
			{
			//	vec3 sampleWeights = abs(v_viewDirVS);
			//	float invSampleMag = 1.0 / (sampleWeights.x + sampleWeights.y + sampleWeights.z + .0001);
			//	sampleWeights *= invSampleMag;

			//	float opacity = finalColor.x * sampleWeights.x + finalColor.y * sampleWeights.y + finalColor.z * sampleWeights.z;
				//finalColor.rgb = vec3(opacity,0,1-opacity);

				finalColor.a = 1;// float(u_clipMapViewMode) / 100;
			}
			return finalColor;
		}
		else
			return half4(0,0,0,0);	
	}

	void surf(appdata_vxcm_object i, inout SurfaceOutput o) {

		half4 color = frag(i);
		o.Albedo = color;
		/*fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = tex.rgb * _Color.rgb;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;
		//clip(o.Alpha - _Cutoff);
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		*/
	}

	ENDCG

	Subshader {
		Tags{
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		LOD 300

		Pass{
	
			Lighting Off
			cull off
			ztest always
			Zwrite off
			Fog{ Mode Off }
			//ColorMask RGB
			//Blend DstColor One
			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
		//	Offset - 1, -1

			CGPROGRAM
			#pragma vertex vert_vxcm_object //fullforwardshadows approxview
			//#pragma fragment frag
			#pragma surface surf BlinnPhong alpha 
			#pragma target 3.0
			ENDCG
		}
	}

	//Fallback off
	//Fallback "VertexLit" // fa le ombre
	FallBack "Transparent/Cutout/VertexLit"
} // shader