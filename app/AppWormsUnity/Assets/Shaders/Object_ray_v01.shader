// DF
Shader "Vxcm/Object/ray_v01"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	#define VXCM_OBJECT_VS
	#include "UnityVxcm.cginc"

	//sampler3D _Volume;

	//float DF_MAX_MINUS_MIN;
	//float DF_MIN;

	//uniform float u_cut_plane_xz;
	//uniform float u_textureRes;
	//uniform float4x4 u_objectToVolumeInvTrx;

	//float DF( float3 sampleLevelPos) {
	//	float4 val = tex3Dlod(_Volume, float4(sampleLevelPos, 0));
	//	return (1.0 - val.r) * (DF_MAX_MINUS_MIN) +DF_MIN;
	//}

	//// return distance or 0 if not hit
	//float raycast(
	//	in float3 enter,
	//	in float3 leave,
	//	in float3 dir,
	//	float clipPlaneY,
	//	out int count)
	//{
	//	count = 0;
	//	float distance = 0;
	//	float stepLength = length(leave - enter);

	//	float sample_size = u_textureRes; // TODO
	//	float iso_level = sample_size * 0.1;
	//	float t = 0.0;
	//	float d = 1.0;

	//	[loop] for (count = 0; count < 99 ; ++count) {
	//		float3 samplePos = enter + dir * t;

	//		d = DF( samplePos);
	//		if (d > iso_level)
	//		{
	//			//  vado avanti
	//			if (t < stepLength)
	//				t += sample_size * d;
	//			else
	//				break;
	//		}
	//		else
	//		{
	//			// colpito, mi fermo
	//			distance = t;
	//			break;
	//		}
	//	}
	//	return distance;
	//}

	struct fragOut
	{
		float4 color : COLOR;
		float depth : DEPTH;
	};

	fragOut frag(appdata_vxcm_object i)
	{
		//	return float4(1,0,0, 1);
		fragOut o;
		o.color = half4(0, 0, 0, 0);
		o.depth = 0;

		float3 rayDirTex, rayOriginTex;
		vxcm_getPixelRay(i, rayDirTex, rayOriginTex);

		o.color = half4(rayDirTex, 1);
		return o;

		float3 min = float3(0,0,0);
		float3 max = float3(1,1,1);
		float tEnter = 0.0;
		float tLeave = 0.0;

		if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
		{
			float3 enter, exit;
			int count = 0;
			float clipPlaneY = 0;
			
			enter = rayOriginTex + tEnter * rayDirTex;
			exit = rayOriginTex + tLeave * rayDirTex;

			half4 finalColor = half4(1, 0, 0, 1);

			float dist  = raycast( enter, exit, rayDirTex, clipPlaneY,  count);
		
			//dist = float(count) / 100;

			// decode color
			if (dist >0.01)
			{


				finalColor = half4(dist, dist, dist, 1);

				// to eye coordinate
				float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * dist,1));
				float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
				o.depth =  vpos.z / vpos.w;
				o.color = finalColor;
			}
			else
				discard;
		//	o.color = finalColor;
		}
		else
		{
			discard;
			//o.col = float(0, 0, 0, 0);
		}

		
		return o;

	}

		ENDCG

		Subshader {
			//Tags{ "Queue" = "Opaque" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
			//Tags{ "LightMode" = "ShadowCaster" }

			//Tags{ "RenderType" = "Opaque" }
			Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }

			Pass{

				ZWrite On	ZTest Less
				Offset 1, 1
				//Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

				CGPROGRAM

				#pragma vertex vert_vxcm_object 
				#pragma fragment frag    addshadow  
				#pragma target 3.0

				#pragma multi_compile_shadowcaster
				#pragma fragmentoption ARB_precision_hint_fastest

				ENDCG
			}
	}

	Fallback "Transparent/Cutout/VertexLit"
} // shader