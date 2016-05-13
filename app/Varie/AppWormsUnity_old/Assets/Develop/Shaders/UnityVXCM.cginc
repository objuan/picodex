#ifndef u_vxcm_CG_VXCM_INCLUDED
#define u_vxcm_CG_VXCM_INCLUDED

#include "UnityCG.cginc"

// =============== VXCM_OBJECT_VS ===================
#if defined (VXCM_OBJECT_VS)

uniform float4x4 u_objectToVolumeTrx;
uniform float4x4 u_vxcm_worldViewMatrix;
// w, h , nearplane
uniform float3 u_cameraInfo;

struct appdata {
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct appdata_vxcm_object {
	float4 pos : SV_POSITION;
	float3 uvw : TEXCOORD0;
	float4 scrPos : TEXCOORD1;
	float4 posW : TEXCOORD2;
//	UNITY_FOG_COORDS(2) // TODO , rimuovere ?? 
};

struct fragOut
{
	half4 color : COLOR;
	float depth : DEPTH;
};

inline appdata_vxcm_object vert_vxcm_object(appdata v)
{
	appdata_vxcm_object o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uvw = mul(u_objectToVolumeTrx, v.vertex);
	o.scrPos = ComputeScreenPos(o.pos);
	o.posW = mul(UNITY_MATRIX_MV, v.vertex);
	//o.uvw = v.vertex.xyz;// TRANSFORM_TEX(v.texcoord, _MainTex);
	//UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

inline void vxcm_getPixelRay(appdata_vxcm_object i, out float3 rayDirTex, out float3 rayOriginTex)
{
	float2 wcoord = (i.scrPos.xy / i.scrPos.w);
	float2 v_position = wcoord * 2.0 - 1.0;
	// in camera space
	float3 viewDirVS = float3(v_position  * u_cameraInfo.xy, u_cameraInfo.z);

	rayDirTex = normalize(mul(u_vxcm_worldViewMatrix, float4(viewDirVS, 1.0)).xyz);

	rayOriginTex = i.uvw;// float3(wcoord, 0);
	//rayDirTex = normalize(mul(u_vxcm_worldViewMatrix, float4(i.viewDirVS, 1.0)).xyz);
	//rayOriginTex = mul(u_vxcm_worldToTexTrx, float4(u_vxcm_cameraPos, 1)).xyz;
}

#endif

// =============== VXCM_RAYCAST_VS ===================
#if defined (VXCM_RAYCAST_VS)

#define ARRAY_LAYERS 4 //TODO, PARAMS

uniform float u_vxcm_fYfovRad;
uniform float u_vxcm_fNear;
uniform float u_vxcm_fRatio;
uniform float3 u_vxcm_cameraPos;
uniform float4x4 u_vxcm_worldToTexTrx;
uniform float4x4 u_vxcm_worldViewMatrix;

//uniform float3 u_clipMapPivotPositionTex[ARRAY_LAYERS];
//uniform float3 u_clipMapOffset[ARRAY_LAYERS];
//uniform float3 u_clipMapScale;

struct appdata_vxcm_raycast {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
	float3 viewDirVS : TEXCOORD1;
};

inline appdata_vxcm_raycast vert_vxcm_raycast(appdata_img v)
{
	appdata_vxcm_raycast o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;
	
	float tanFov_2 = tan(u_vxcm_fYfovRad / 2.0);
	float h2 = u_vxcm_fNear * tanFov_2;
	float w2 = u_vxcm_fRatio * h2;

	float2 v_position = o.uv * 2.0 - 1.0;

	// in camera space
	o.viewDirVS = float3(v_position  * float2(w2, h2), u_vxcm_fNear);

	return o;
}

inline void vxcm_getPixelRay(appdata_vxcm_raycast i, out float3 rayDirTex, out float3 rayOriginTex)
{
	rayDirTex = normalize(mul(u_vxcm_worldViewMatrix, float4(i.viewDirVS, 1.0)).xyz);
	rayOriginTex = mul(u_vxcm_worldToTexTrx, float4(u_vxcm_cameraPos, 1)).xyz;
}

#endif // VXCM_RAYCAST_VS

/*
* This function implements the "slab test" algorithm for intersecting a ray with a box
* (http://www.siggraph.org/education/materials/HyperGraph/raytrace/rtinter3.htm)
*/
inline bool vxcm_intersectRayWithAABB(in float3 ro, in float3 rd,         // Ray-origin and -direction
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

//inline float4  vxcm_getClipMapTexture(sampler3D txt,int level, float3 sampleLevelPos) {
//	float3 pos = frac(sampleLevelPos + u_clipMapPivotPositionTex[level]); // CYCLIC
//	return tex3Dlod(txt, float4(pos * u_clipMapScale + u_clipMapOffset[level],1));
//
//	//return tex3Dlod(_Volume, float4(sampleLevelPos, 0));
//}

#endif // u_vxcm_CG_VXCM_INCLUDED
