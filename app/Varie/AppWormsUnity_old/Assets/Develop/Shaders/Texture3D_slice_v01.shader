// primo shader non trasparente
Shader "Vxcm/Texture3D/slice_v01"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
	}

		CGINCLUDE
#include "UnityCG.cginc"

#define VXCM_OBJECT_VS
#include "UnityVxcm.cginc"

	sampler3D _Volume;

#define DISTANCE_FIELD_MAX 4
#define DISTANCE_FIELD_MIN -2

	uniform float u_sample_rate;
	uniform float u_cut_plane_xz;
	uniform int u_debug_layer;

	// sampleLevelPos  are in texture level space
	float4 getClipMapTexture(int level, float3 sampleLevelPos) {
		//float3 pos = frac(sampleLevelPos ); // CYCLIC
		// devo usare la forma lod altrimenti non capisce che mipmap utilizzare
		return tex3Dlod(_Volume, float4(sampleLevelPos, 0));
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

	float4 raycast_distance_field(in int layerIdx,
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
		float fSampleCount = stepLength / stepSize;
		int iSampleCount = int(ceil(fSampleCount));

		int debugLayer = u_debug_layer;
		bool isoMode = debugLayer >= 2;
		if (isoMode) debugLayer -= 2;

		//	float4 returnColor = float4(0);
		[loop] for (int i = 0; i < iSampleCount; ++i) {
			float3 samplePos = enter + dir * stepSize * float(i);
			count++;

			float4 encoded_voxel = getClipMapTexture(layerIdx, samplePos);

			if (encoded_voxel.r>0 && samplePos.y < u_cut_plane_xz)
			{
				float distance_field_decoded = (1.0 - encoded_voxel.r) * (4 - -2) + -2;

				if ((isoMode && distance_field_decoded < 0) || (!isoMode))
				{
					float4 voxelColor;
					if (debugLayer == 0) // normal
					{
						// normale a 3 canali
						float3 normal = encoded_voxel.xyz * 2 - float3(1, 1, 1);
						float opacity = 1;//encoded_voxel.b;
						voxelColor = float4(encoded_voxel.xyz, opacity);
					}
					else if (debugLayer == 1) // distance field
					{
						//	voxelColor = vec4(1,1,1,1);

						float distance_field_decoded = (1.0 - encoded_voxel.r) * (DISTANCE_FIELD_MAX - DISTANCE_FIELD_MIN) + DISTANCE_FIELD_MIN;

						voxelColor = float4(0, 0, 0, 1);
						if (abs(distance_field_decoded) < 0.01)
							voxelColor = float4(0, 1, 0, 1);
						else if (distance_field_decoded < 0)
							voxelColor = float4(0, 0, -distance_field_decoded / abs(DISTANCE_FIELD_MIN), 1);
						else
							voxelColor = float4(distance_field_decoded / DISTANCE_FIELD_MAX, 0, 0, 1);
						//voxelColor.w=1;
						voxelColor.a = 1.0;

						//voxelColor= vec4(encoded_voxel.a,encoded_voxel.a,encoded_voxel.a,1);

					}

					accum += (1 - accum.a) * voxelColor;
					if (accum.a > 0.99) return accum;
				}
			}
		}
		return accum;
	}

	half4 frag(appdata_vxcm_object i) : COLOR
	{
		//	return float4(1,0,0, 1);

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

			//	finalColor = float4(1, 1, 0, 1);
			//	finalColor = float4(rayOriginTex, 1);

			finalColor = raycast_distance_field(0, enter, exit, rayDirTex, clipPlaneY, finalColor, count);
			//finalColor = raycast(0, enter, exit, rayDirTex, clipPlaneY, finalColor, count);

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
		Tags{ "RenderType" = "Opaque" }
			Pass{

			/*	Lighting Off
			cull off
			ztest always
			Zwrite off
			Fog{ Mode Off }
			ColorMask RGB*/
			//Blend DstColor One
			//	Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
			//	Offset - 1, -1

			CGPROGRAM

#pragma vertex vert_vxcm_object
#pragma fragment frag
#pragma target 3.0

			ENDCG
		}
	}

	//Fallback off
	Fallback "VertexLit" // fa le ombre
} // shader