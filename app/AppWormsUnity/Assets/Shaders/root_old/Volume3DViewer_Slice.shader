Shader "Vxcm/volume3DViewer/slice"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
		_SampleRate("Sample rate",float) = 3
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	#define VXCM_RAYCAST_VS
	#include "UnityVxcm.cginc"

	sampler3D _Volume;
	float _SampleRate;

	uniform float3 u_clipMapPivotPositionTex[10];
	uniform float3 u_clipMapOffset[10];
	uniform float3 u_clipMapScale;

	// sampleLevelPos  are in texture level space
	float4 getClipMapTexture(int level, float3 sampleLevelPos) {
		float3 pos = frac(sampleLevelPos + u_clipMapPivotPositionTex[level]); // CYCLIC
																			  // devo usare la forma lod altrimenti non capisce che mipmap utilizzare
		return tex3Dlod(_Volume, float4(pos * u_clipMapScale + u_clipMapOffset[level], 0));
	}

	// sampleLevelPos  are in texture level space
	float4 getClipMapTexture2(int level, float3 sampleLevelPos) {
		float3 pos = frac(sampleLevelPos + u_clipMapPivotPositionTex[level]); // CYCLIC

		return tex3Dlod(_Volume, float4(pos+ u_clipMapOffset[level], 0));
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
		float stepSize = _SampleRate;//1.0 / u_resolution;
									   //float stepSize = 0.33 / u_resolution;
									   //float stepSize = 0.5f / 64.0f;
									   //float iso_level = 1.0 / 1024;

		float fSampleCount = stepLength / stepSize;
		int iSampleCount = int(ceil(fSampleCount));

		//	float4 returnColor = float4(0);
		[loop] for (int i = 0; i < iSampleCount; ++i) {
			float3 samplePos = enter + dir * stepSize * float(i);
			count++;

			float4 voxelColor = getClipMapTexture(layerIdx, samplePos);
		
			if (voxelColor.a>0)
			{
				accum += (1 - accum.a) * voxelColor;
				if (accum.a > 0.99) return accum;
			}
		}
		return accum;
	}


	half4 frag(appdata_vxcm_raycast i) : COLOR
	{
		float3 rayDirTex, rayOriginTex;
		vxcm_getPixelRay(i, rayDirTex, rayOriginTex);
			
		float3 min = float3(0,0,0);
		float3 max = float3(1,1,1);
		float tEnter = 0.0;
		float tLeave = 0.0;

	//	rayOriginTex = float3(0.5, 0.5, 0.5);

	//	return half4(rayDirTex, 1);
		
		if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
		{
			float3 enter, exit;
			int count = 0;
			float clipPlaneY = 0;
			float4 finalColor;

			enter = rayOriginTex + tEnter * rayDirTex;
			exit = rayOriginTex + tLeave * rayDirTex;

			//finalColor = float4(1, 0, 0, 1);

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

	ENDCG

	Subshader {

		Pass{
	
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#pragma vertex vert_vxcm_raycast
			#pragma fragment frag
			#pragma target 3.0

			ENDCG
		}
	}

	Fallback off
} // shader