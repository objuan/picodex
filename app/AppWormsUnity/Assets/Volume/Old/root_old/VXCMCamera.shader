Shader "Vxcm/VXCMCamera"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
		float3 viewDirVS : TEXCOORD1;
	};

	float _u_fYfovRad;
	float _u_fNear;
	float _u_fRatio;
	float4 _u_cameraPos;
	float4x4 _u_worldToTexTrx;
	float4x4 _u_worldViewMatrix;

	sampler2D _MainTex;
	sampler2D_float _CameraDepthTexture;

	/** Read the camera-space position of the point at screen-space pixel ssP */
	float3 GetPosition(float2 ssP) {
		float3 P;

		P.z = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, ssP.xy);

		P = float3(float2(ssP) /*+ float2(0.5, 0.5)*/, P.z);
		return P;
	}

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		//o.position_in_world_space = mul(_Object2World, v.vertex);

		float tanFov_2 = tan(_u_fYfovRad / 2.0);
		float h2 = _u_fNear * tanFov_2;
		float w2 = _u_fRatio * h2;

		float2 v_position = o.uv * 2.0 - 1.0;
		
		// in camera space
		o.viewDirVS = float3(v_position  * float2(w2, h2), _u_fNear);

		return o;
	}

	/*
	* This function implements the "slab test" algorithm for intersecting a ray with a box
	* (http://www.siggraph.org/education/materials/HyperGraph/raytrace/rtinter3.htm)
	*/
	bool intersectRayWithAABB(in float3 ro, in float3 rd,         // Ray-origin and -direction
		in float3 boxMin, in float3 boxMax,
		out float tEnter, out float tLeave)
	{
		float3 tempMin = (boxMin - ro) / rd;
		float3 tempMax = (boxMax - ro) / rd;

		float3 v3Max = max(tempMax, tempMin);
		float3 v3Min = min(tempMax, tempMin);

		tLeave = min(v3Max.x, min(v3Max.y, v3Max.z));
		tEnter = max(max(v3Min.x, 0.0), max(v3Min.y, v3Min.z));

		return tLeave > tEnter;
	}

	half4 frag(v2f i) : COLOR
	{
		float3 rayDirTex = normalize(mul(_u_worldViewMatrix , float4(i.viewDirVS, 1.0)).xyz);

		float3 rayOriginTex = mul(_u_worldToTexTrx , _u_cameraPos).xyz;
		
		// Pixel being shaded 
		//float2 ssC = i.uv.xy;// * _MainTex_TexelSize.zw;
		// View space point being shaded
		//float3 C = GetPosition(ssC);
		float3 min = float3(0,0,0);
		float3 max = float3(1,1,1);
		float tEnter = 0.0;
		float tLeave = 0.0;

	//	return half4(rayDirTex, 1);
		
		if (intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
		{
			return half4(1,0,0, 1);
		}
		else
			return half4(0,0,0,0);
		

	}

	ENDCG

	Subshader {

		// 0: 
		Pass{
			//LOD 200 // ???

			ZTest Always Cull Off ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			ENDCG
		}
	}

	Fallback off
} // shader