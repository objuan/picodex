#ifndef u_vxcm_CG_VXCM_INCLUDED
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata_vcxm members vertex,worldPos,rayEnc)
#pragma exclude_renderers d3d11 xbox360
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

// =================================================
// ================= VXCM PROXY ====================
// =================================================

#if defined(VXCM_PROXY_SHADOW) || defined(VXCM_PROXY_RENDER)
#define VXCM_PROXY_VS
#endif

#if defined (VXCM_PROXY_VS)

uniform float4x4 u_objectToVolumeTrx;
uniform float4x4 u_vxcm_worldViewMatrix;
uniform float4x4 u_camToLocalTrx;
uniform float3 u_camPosTrx;
uniform float4x4 u_prjInvTrx;
// w, h , nearplane
uniform float3 u_cameraInfo;


struct appdata {
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct appdata_vxcm_object {
	float4 pos : SV_POSITION;
	float3 localPos : TEXCOORD0;
	float3 volumePos : TEXCOORD1;
};


inline appdata_vxcm_object vert_vxcm_object(appdata v)
{
	appdata_vxcm_object o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.volumePos = mul(u_objectToVolumeTrx, v.vertex);
	o.localPos =  v.vertex;
	return o;
}

#endif
// =============================================================
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


// =============================================


sampler3D _Volume;
float DF_MAX_MINUS_MIN;
float DF_MIN;

uniform float u_cut_plane_xz;
uniform float4 u_textureRes;
uniform float4x4 u_objectToVolumeInvTrx;
//uniform float4x4 u_worldToVolumeTrx;

float DF(float3 sampleLevelPos) {
	float4 val = tex3Dlod(_Volume, float4(sampleLevelPos, 0));
	return (1.0 - val.r) * (DF_MAX_MINUS_MIN)+DF_MIN;
}

// return distance or 0 if not hit
float raycast(
	in float3 enter,
	in float3 leave,
	in float3 dir,
	float clipPlaneY,
	out int count)
{
	count = 0;
	float distance = 0;
	float stepLength = length(leave - enter);

	float3 sample_size = u_textureRes.xyz; // TODO
	float iso_level = sample_size * 0.1;
	float t = 0.0;
	float d = 1.0;
	float2 prec = 0.0; // previous values
	//[unroll(10)]
	for (count = 0; count < 100; ++count) {
		float3 samplePos = enter + dir * t;

		prec.x = d;
		d = DF(samplePos);
		if (d > iso_level)
		{
			//  vado avanti
			if (t < stepLength)
			{
				prec.y = t;
				t += sample_size * d;
			}
			else
				break;
		}
		else
		{
			// colpito, mi fermo
			// calcolo l'interpolazione
			float factor =  (iso_level - prec.x) / (d - prec.x);
			distance = prec.y + factor  * (t - prec.y);
			//distance = t;// +u_textureRes.x;// lerp(prec.y, t, factor);
			//distance = lerp(prec.y, t,factor);
			//distance = t;
			//distance = factor;

			//float factor = (d - iso_level ) / (d - prec.x);
			//distance = t - factor * (t - prec.y);

			break;
		}
	}
	return distance;
}

// return 
// x =  hit dist , ==0 no hit
// a = 1-> hit, 0 = no hit
float4 raycast(float3 rayOriginTex,float3 rayDirTex)
{
	float4 retColor = float4(0, 0, 0,0);

	float3 min = float3(0, 0, 0);
	float3 max = float3(1, 1, 1);
	float tEnter = 0.0;
	float tLeave = 0.0;

	if (vxcm_intersectRayWithAABB(rayOriginTex, rayDirTex, min, max, tEnter, tLeave))
	{
		//float3 finalColor = float3(0, 0, 0);

		float3 enter, exit;
		int count = 0;
		float clipPlaneY = 0;

		enter = rayOriginTex.xyz + tEnter * rayDirTex;
		exit = rayOriginTex.xyz + tLeave * rayDirTex;

		float dist = raycast(enter, exit, rayDirTex, clipPlaneY, count);

		//dist = float(count) / 100;

		// decode color
		if (dist >0.01)
		{
			//finalColor = float3(dist, dist, dist);

			// to eye coordinate
		//	float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex.xyz + rayDirTex * dist, 1));
			//float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
			//	o.depth = vpos.z / vpos.w;
	
			retColor = float4(dist, dist, dist,1); //TODO BETTER
		}

	}
	return retColor;
}

// moltiplica p per l'inverza della matrice orthogonal 
// upper, left 3x3 matrix is orthogonal; 
// 4th row is (0,0,0,1) 
inline float4 multInverse(float4x4 m, float p) {
	float4 last_column = float4(m[0][3], m[1][3], m[2][3], m[3][3]);
	return float4(mul(p - last_column, m).xyz, 1.0);
}


// ===============================
// COMMON
// ===============================

// struct per 
struct appdata_vcxm_fs
{
	float4 vertex;
	float4 worldPos;
	float3 volumePos;
	float4 rayEnc;
};

struct out_vcxm_fs
{
	//float4 color	: COLOR;
	//float depth : DEPTH;
	float4 color : SV_Target;
	float depth : SV_Depth;
};


#define VXCM_RAYCAST(ORIG,DIR,o) o.rayEnc = raycast(ORIG, DIR);o.volumePos = ORIG + DIR * o.rayEnc.x;o.vertex = mul(u_objectToVolumeInvTrx, float4(o.volumePos, 1));o.worldPos = mul(UNITY_MATRIX_MVP, o.vertex);


inline float3 calcNormal(in appdata_vcxm_fs i)
{
	float3 pos = i.volumePos;
	//vec2 delta = vec2(0, SAMPLE_SIZE);
	float3 grad;
	grad.x = DF(pos + u_textureRes.xww) - DF(pos - u_textureRes.xww);
	grad.y = DF(pos + u_textureRes.wyw) - DF(pos - u_textureRes.wyw);
	grad.z = DF(pos + u_textureRes.wwz) - DF(pos - u_textureRes.wwz);
	return normalize(grad);

}

// ===============================
// SHADOW
// ===============================

// UNITY_PASS_SHADOWCASTER
#if defined (VXCM_PROXY_SHADOW)

// V2F_SHADOW_CASTER
// float4 pos : SV_POSITION
// (float4 hpos  OR  float3 vec ) : TEXCOORD0
struct appdata_vcxm_shadow {
	V2F_SHADOW_CASTER;
	//float2  uv : TEXCOORD1;
	float4 vertex: TEXCOORD1;// for volume
};

inline void getShadowRay(appdata_vcxm_shadow i, out float3 rayOriginTex, out float3 rayDirTex)
{
	rayOriginTex = mul(u_objectToVolumeTrx, float4(i.vertex.xyz, 1)).xyz;

	float4 localCameraPos;
	if (i.pos.w == 1) // TODO ????
		localCameraPos = mul(transpose(UNITY_MATRIX_IT_MV), float4(0, 0, 10000, 1)); // from directional light
	else
		localCameraPos = mul(transpose(UNITY_MATRIX_IT_MV), float4(0, 0, 0, 1)); // from camera

	rayDirTex = normalize(i.vertex.xyz - localCameraPos.xyz);
}

#endif


// ===============================
// RENDER
// ===============================

#if defined (VXCM_PROXY_RENDER)

struct appdata_vxcm
{
	float4 pos : SV_POSITION;
	//	float3 lightDir : TEXCOORD0;
	float2 uv : TEXCOORD0;
	//	float3 volumePos: TEXCOORD3; // for volume
	float4 vertex: TEXCOORD1;// for volume
	LIGHTING_COORDS(2,3)
};

inline appdata_vxcm vert_vxcm(in appdata v)
{
	appdata_vxcm o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	//o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
	//	o.volumePos = mul(u_objectToVolumeTrx, v.vertex).xyz;
	o.vertex = v.vertex;

	TRANSFER_VERTEX_TO_FRAGMENT(o);
	return o;
}

inline void getCameraRay(in appdata_vxcm i, out float3 rayOriginTex, out float3 rayDirTex)
{
	rayOriginTex = mul(u_objectToVolumeTrx, float4(i.vertex.xyz, 1)).xyz;
	float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
	rayDirTex = normalize(i.vertex - localCameraPos.xyz);

}

inline void remapShadowMap(inout appdata_vxcm i, in appdata_vcxm_fs v)
{
	i.pos = v.worldPos;
	i.vertex = v.vertex;
	TRANSFER_VERTEX_TO_FRAGMENT(i); // REDO MAP, inutile sulel direzionali ??
}

#endif


#endif // u_vxcm_CG_VXCM_INCLUDED
