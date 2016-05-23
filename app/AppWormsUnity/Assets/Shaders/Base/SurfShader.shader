#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_fwdbase
#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"
struct Input {
	float2 uv_MainTex : TEXCOORD0;
};
sampler2D _MainTex;
sampler2D _BumpMap;
void surf(Input IN, inout SurfaceOutput o)
{
	o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
}
struct v2f_surf {
	V2F_POS_FOG;
	float2 hip_pack0 : TEXCOORD0;
#ifndef LIGHTMAP_OFF
	float2 hip_lmap : TEXCOORD1;
#else
	float3 lightDir : TEXCOORD1;
	float3 vlight : TEXCOORD2;
#endif
	LIGHTING_COORDS(3, 4)
};
#ifndef LIGHTMAP_OFF
float4 unity_LightmapST;
#endif
float4 _MainTex_ST;
v2f_surf vert_surf(appdata_full v) {
	v2f_surf o;
	PositionFog(v.vertex, o.pos, o.fog);
	o.hip_pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
#ifndef LIGHTMAP_OFF
	o.hip_lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
	float3 worldN = mul((float3x3)_Object2World, SCALED_NORMAL);
	TANGENT_SPACE_ROTATION;
#ifdef LIGHTMAP_OFF
	o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
#endif
#ifdef LIGHTMAP_OFF
	float3 shlight = ShadeSH9(float4(worldN, 1.0));
	o.vlight = shlight;
#ifdef VERTEXLIGHT_ON
	float3 worldPos = mul(_Object2World, v.vertex).xyz;
	o.vlight += Shade4PointLights(
		unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
		unity_LightColor0, unity_LightColor1, unity_LightColor2, unity_LightColor3,
		unity_4LightAtten0, worldPos, worldN);
#endif // VERTEXLIGHT_ON
#endif // LIGHTMAP_OFF
	TRANSFER_VERTEX_TO_FRAGMENT(o);
	return o;
}
#ifndef LIGHTMAP_OFF
sampler2D unity_Lightmap;
#endif
half4 frag_surf(v2f_surf IN) : COLOR{
	Input surfIN;
	surfIN.uv_MainTex = IN.hip_pack0.xy;
	SurfaceOutput o;
	o.Albedo = 0.0;
	o.Emission = 0.0;
	o.Specular = 0.0;
	o.Alpha = 0.0;
	o.Gloss = 0.0;
	surf(surfIN, o);
	half atten = LIGHT_ATTENUATION(IN);
	half4 c;
	#ifdef LIGHTMAP_OFF
		c = LightingLambert(o, IN.lightDir, atten);
		c.rgb += o.Albedo * IN.vlight;
	#else // LIGHTMAP_OFF
		half3 lmFull = DecodeLightmap(tex2D(unity_Lightmap, IN.hip_lmap.xy));
	#ifdef SHADOWS_SCREEN
		c.rgb = o.Albedo * min(lmFull, atten * 2);
	#else
		c.rgb = o.Albedo * lmFull;
	#endif
	c.a = o.Alpha;
	#endif // LIGHTMAP_OFF
	return c;
}