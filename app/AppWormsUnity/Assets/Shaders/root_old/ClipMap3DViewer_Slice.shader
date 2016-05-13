Shader "Vxcm/clipmap3DViewer/slice"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	#define VXCM_RAYCAST_VS
	#include "UnityVxcm.cginc"

	sampler3D _Volume;

	#define DISTANCE_FIELD_MAX 4
	#define DISTANCE_FIELD_MIN -2

	uniform float u_sample_rate;
	uniform float u_debug_layer;

	uniform float u_clipMapPivotPositionTex[10];
	uniform float u_clipMapOffset[10];
	uniform float u_clipMapScale;

	// sampleLevelPos  are in texture level space
	float4 getClipMapTexture(int level, float3 sampleLevelPos) {
		float3 pos = frac(sampleLevelPos + u_clipMapPivotPositionTex[level]); // CYCLIC
		// devo usare la forma lod altrimenti non capisce che mipmap utilizzare
		return tex3Dlod(_Volume, float4(pos * u_clipMapScale + u_clipMapOffset[level],0));
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

		float fSampleCount = stepLength / stepSize;
		int iSampleCount = int(ceil(fSampleCount));

		int debugLayer = u_debug_layer;
		bool isoMode = u_debug_layer >= 3;
		if (isoMode) debugLayer -= 3;

		//	float4 returnColor = float4(0);
		[loop] for (int i = 0; i < iSampleCount; ++i) {
			float3 samplePos = enter + dir * stepSize * float(i);
			count++;

			// salto se taglio
			//if (samplePos.y > clipPlaneY) continue;

			// leggo e trovo il valore giusto
			//float4 encoded_voxel= texture3D(u_clipMap,samplePos );
			float4 encoded_voxel = getClipMapTexture(layerIdx, samplePos);
			float distance_field_decoded = (1.0 - encoded_voxel.a) * (DISTANCE_FIELD_MAX - DISTANCE_FIELD_MIN) + DISTANCE_FIELD_MIN;

			//	if ((isoMode && encoded_voxel.a >= ISO_VALUE_ENC) || ( !isoMode && encoded_voxel.a>0))
			//	if ((isoMode && distance_field_decoded >0 ) || ( !isoMode && encoded_voxel.a>0))
			if (encoded_voxel.a>0)
			{
				float distance_field_decoded = (1.0 - encoded_voxel.a) * (DISTANCE_FIELD_MAX - DISTANCE_FIELD_MIN) + DISTANCE_FIELD_MIN;
				if ((isoMode && distance_field_decoded <0) || (!isoMode))
				{
					float4 voxelColor;
					if (debugLayer == 0) // normal
					{
						// normale a 3 canali
						float3 normal = encoded_voxel.xyz * 2 - float3(1, 1, 1);
						// normale a stereo
						//float3 normal = decodeNormal_stereo (encoded_voxel.xy);
						//float3 normal = decodeNormal_sphereMap_azimut (encoded_voxel.xy);

						float opacity = 1;//encoded_voxel.b;

										  //voxelColor = float4(normal,opacity);
						voxelColor = float4(encoded_voxel.xyz, opacity);
						//voxelColor = float4(1,0,1,1);
					}
					else if (debugLayer == 1) // distance field
					{
						//	voxelColor = float4(1,1,1,1);

						float distance_field_decoded = (1.0 - encoded_voxel.a) * (DISTANCE_FIELD_MAX - DISTANCE_FIELD_MIN) + DISTANCE_FIELD_MIN;

						voxelColor = float4(0, 0, 0, 1);
						if (abs(distance_field_decoded) < 0.01)
							voxelColor = float4(0, 1, 0, 1);
						else if (distance_field_decoded < 0)
							voxelColor = float4(0, 0, -distance_field_decoded / abs(DISTANCE_FIELD_MIN), 1);
						else
							voxelColor = float4(distance_field_decoded / DISTANCE_FIELD_MAX, 0, 0, 1);
						//voxelColor.w=1;
						voxelColor.a = 1.0;

						//voxelColor= float4(encoded_voxel.a,encoded_voxel.a,encoded_voxel.a,1);

					}
					else if (debugLayer == 2) // emission
					{
						voxelColor = float4(encoded_voxel.xyz, 1);
					}

					accum += (1 - accum.a) * voxelColor;
					if (accum.a > 0.99) return accum;
				}
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

	//	return half4(rayDirTex, 1);
		
		if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
		{
			float3 enter, exit;
			int count = 0;
			float clipPlaneY = 0;
			float4 finalColor;

			enter = rayOriginTex + tEnter * rayDirTex;
			exit = rayOriginTex + tLeave * rayDirTex;

			finalColor = float4(1, 0, 0, 1);

		//	finalColor =  raycast(0, enter, exit, rayDirTex, clipPlaneY, finalColor, count);

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