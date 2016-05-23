Shader "Vxcm/Object/ray_v05"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,0)
		_Emission("Emissive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.1, 1)) = 0.7
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

	// 2/3 texture stage GPUs
	SubShader{
		Tags{ "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		// Pass to render object as a shadow caster
		Pass{
			Name "Caster"
			Tags{ "LightMode" = "ShadowCaster" }
			Offset 1, 1

			Fog{ Mode Off }
			ZWrite On ZTest Less Cull Off

			CGPROGRAM
			//#pragma vertex vert
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			#include "UnityCG.cginc"
			#define VXCM_PROXY_VS
			#include "UnityVxcm.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				float3 localPos: TEXCOORD2;// for volume
			};

			uniform float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.localPos = v.vertex.xyz;
				return o;
			}

			struct fragOut
			{
				float4 color : COLOR;
				float depth : DEPTH;
			};

			uniform sampler2D _MainTex;
			uniform float _Cutoff;
			uniform float4 _Color;
			uniform float3 u_cameraInfoLight;

			//float4 frag(v2f i) : COLOR
			fragOut frag(v2f i)
			{
				fragOut o;
				float3 rayOriginTex = mul(u_objectToVolumeTrx, float4(i.localPos,1)).xyz;

				float4 localCameraPos;
				if (i.pos.w == 1) // TODO ????
					localCameraPos =   mul(transpose(UNITY_MATRIX_IT_MV), float4(0, 0, -10000, 1)); // from directional light
				else
					localCameraPos = mul(transpose(UNITY_MATRIX_IT_MV), float4(0, 0,0, 1)); // from camera
					
				float3 rayDirTex = normalize(i.localPos.xyz - localCameraPos.xyz);

				float4 texcol = raycast(rayOriginTex, rayDirTex);

				float4 vertex = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * texcol.x, 1));
				float4 opos = mul(UNITY_MATRIX_MVP, vertex);

				//float depth = vpos.z;// / vpos.w;

				//TRANSFER_SHADOW_CASTER_NOPOS_LEGACY(o, vpos)

				//	opos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz, 1)); \
				//	opos = UnityApplyLinearShadowBias(opos);

				//depth = depth - 0.01;

			/*	if (texcol.a = 0)
					depth = 0;
*/
				//half4 texcol = tex2D(_MainTex, i.uv);
				
				
			//	i.pos = vpos;
			//	/i.pos.z /= i.pos.w;

				//i.vec = float(0,0,vpos.z);
				//i.hpos.zw = vpos.zw;

				//SHADOW_CASTER_FRAGMENT(i)
		

#ifdef SHADOWS_CUBE
				i.vec = mul(_Object2World, vertex).xyz - _LightPositionRange.xyz;

				float4 enc = UnityEncodeCubeShadowDepth((length(i.vec) + unity_LightShadowBias.x) * _LightPositionRange.w);;

				o.depth = opos.z / opos.w;
				o.color = enc;// o.depth;
#else
			
				opos = UnityApplyLinearShadowBias(opos);
				//opos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);

				o.depth = opos.z / opos.w;// i.pos.z / i.pos.w;
				o.color = 1;	
#endif
				clip(texcol.a*_Color.a - _Cutoff);

				return o;
			}
		ENDCG

		}
	

	// Pass to render object as a shadow collector
//	Pass{
//			Name "ShadowCollector"
//			Tags{ "LightMode" = "ShadowCollector" }
//
//			Fog{ Mode Off }
//			ZWrite On ZTest Less
//
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//			#pragma fragmentoption ARB_precision_hint_fastest
//			#pragma multi_compile_shadowcollector
//
//			#define SHADOW_COLLECTOR_PASS
//			#include "UnityCG.cginc"
//			#define VXCM_PROXY_VS
//			#include "UnityVxcm.cginc"
//
//			struct v2f {
//				V2F_SHADOW_COLLECTOR;
//			//	float2  uv : TEXCOORD5;
//				float3 localPos: TEXCOORD5;// for volume
//			};
//
//			uniform float4 _MainTex_ST;
//
//			v2f vert(appdata_base v)
//			{
//				v2f o;
//				TRANSFER_SHADOW_COLLECTOR(o)
//			//	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
//				o.localPos = v.vertex.xyz;
//				return o;
//			}
//
//			struct fragOut
//			{
//				float4 color : COLOR;
//				float depth : DEPTH;
//			};
//
//			uniform sampler2D _MainTex;
//			uniform float _Cutoff;
//			uniform float4 _Color;
//
//			half4 frag(v2f i) : COLOR
//			{
//				fragOut o;
//			//	half4 texcol = tex2D(_MainTex, i.uv);
//
//				float3 rayOriginTex = mul(u_objectToVolumeTrx, float4(i.localPos, 1)).xyz;
//				float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
//				float3 rayDirTex = normalize(i.localPos - localCameraPos.xyz);
//			//	float4 texcol = raycast(rayOriginTex, rayDirTex);
//
//				float4 texcol = float4(0,0,0,0);
///*
//				float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * texcol.x, 1));
//				float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
//				float depth = vpos.z / vpos.w;*/
//
//				//clip(texcol.a*_Color.a - _Cutoff);
//				clip(texcol.a*_Color.a - _Cutoff);
//
//				// float4 wpos = mul(_Object2World, v.vertex);
//				// o._WorldPosViewZ.xyz = wpos;
//				// o._WorldPosViewZ.w = -mul( UNITY_MATRIX_MV, v.vertex ).z;
//
//				SHADOW_COLLECTOR_FRAGMENT(i)
//
//				//o.color = depth;
//				//o.depth = depth;
//				//return o;
//			}
//			ENDCG
//
//		}
	
	} // END SUBSHADER
	
	// 1 texture stage GPUs
	SubShader{
			//Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
			Tags{  "RenderType" = "Opaque" }
		
	
			//Pass{
			//	Name "MainPass"
			//	//Tags{ "LightMode" = "Always" }
			//	Tags{ "LightMode" = "ForwardBase"  }
			////	Tags{ "LightMode" = "ForwardAdd" }

			//	Fog{ Mode Off }
			////	ZWrite On ZTest Less
			//	AlphaTest  Greater 0.1
			////	Lighting On

			//	CGPROGRAM

			//	#pragma vertex vert_vxcm_object_new 
			//	#pragma fragment frag    
			//	#pragma target 3.0

			//	#pragma fragmentoption ARB_precision_hint_fastest

			//	//#pragma multi_compile_builtin
			//	#pragma multi_compile_fwdbase
			//	//#pragma multi_compile_fwdadd_fullshadows

			//	#include "UnityCG.cginc"
			//	#include "AutoLight.cginc"
			//	#define VXCM_PROXY_VS
			//	#include "UnityVxcm.cginc"

			//	
			//	float4 _LightColor0;

			//	sampler2D _MainTex;
			//	half _Glossiness;
			//	half _Metallic;
			//	fixed4 _Color;
			//	uniform float _Cutoff;

			//	struct appdata_vxcm_object_new
			//	{
			//		float4 pos : SV_POSITION;
			//		float3 lightDir : TEXCOORD0;
			//		float2 uv : TEXCOORD1;
			//	//	float3 volumePos: TEXCOORD3; // for volume
			//		float4 localPos: TEXCOORD2;// for volume
			//		LIGHTING_COORDS(3,4)
			//	};

			//	inline appdata_vxcm_object_new vert_vxcm_object_new(appdata v)
			//	{
			//		appdata_vxcm_object_new o;

			//		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			//		o.uv = v.texcoord;
			//		o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
			//	//	o.volumePos = mul(u_objectToVolumeTrx, v.vertex).xyz;
			//		o.localPos = v.vertex;

			//		TRANSFER_VERTEX_TO_FRAGMENT(o);
			//	
			//		return o;
			//	}

			//	struct fragOut
			//	{
			//		float4 color : COLOR;
			//		float depth : DEPTH;
			//	};


			//	fragOut frag(appdata_vxcm_object_new i)
			//	{
			//		//	return float4(1,0,0, 1);
			//		fragOut o;
			//		o.color = half4(0, 0, 0, 0);
			//		o.depth = 0;

			//		// ------------

			//	//	float3 rayOriginTex = i.volumePos;
			//		float3 rayOriginTex = mul(u_objectToVolumeTrx, i.localPos).xyz;
			//		float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
			//		float3 rayDirTex = normalize(i.localPos  - localCameraPos.xyz);

			//		//o.color = half4(rayOriginTex,1);
			//		//return o;

			//		// ------------

			//		float4 texcol = raycast(rayOriginTex, rayDirTex);

			//		if (texcol.a>0)
			//		{
			//		
			//			// get normal in object space
			//			float4 samplePos = float4(rayOriginTex + rayDirTex * texcol.x, 1);

			//			float4 objCoord = mul(u_objectToVolumeInvTrx, samplePos);
			//			float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
			//			o.depth = vpos.z / vpos.w;

			//			float3 lightDir = normalize(ObjSpaceLightDir(objCoord));

			//			float3 N = normalize(calcNormal(samplePos.xyz));

			//			// render

			//			float3 L = normalize(lightDir);


			//			float attenuation = LIGHT_ATTENUATION(i) * 2;
			//			float4 ambient = UNITY_LIGHTMODEL_AMBIENT * 2;

			//			float NdotL = saturate(dot(N, L));
			//			float4 diffuseTerm = NdotL * _LightColor0 * attenuation;

			//			float4 diffuseColor = tex2D(_MainTex, i.uv) * _Color;

			//			float4 finalColor = (ambient + diffuseTerm) * diffuseColor;

			//			o.color = finalColor;// texcol;

			//		//	o.color = float4(NdotL,0,0, 1);
			//		//	o.color = float4(objCoord.xyz, 1);
			//			o.color = float4(N * attenuation, 1);
			//			//o.color = float4(attenuation,0,0, 1);
			//			//o.color = float4(diffuseTerm.xyz, 1);

			//			// depth, to eye coordinate
			//			
			//		}

			//		clip(o.color.a-1.0);

			//		return o;

			//	}

			//	ENDCG
			//}

			Pass{
				Name "SecodnPass"
				//Tags{ "LightMode" = "Always" }
				//Tags{ "LightMode" = "ForwardBase" }
			
				Tags{ "LightMode" = "ForwardAdd" }

				AlphaTest  Greater 0.5
				//Blend One One
				//ZWrite Off
				Lighting On

				CGPROGRAM

#pragma vertex vert_vxcm_object_new 
#pragma fragment frag    
#pragma target 3.0

//#pragma multi_compile_fwdadd                        // This line tells Unity to compile this pass for forward add, giving attenuation information for the light.
#pragma multi_compile_fwdadd_fullshadows                

				//#pragma multi_compile_builtin
				//#pragma multi_compile_fwdbase


#include "UnityCG.cginc"
#include "AutoLight.cginc"
#define VXCM_PROXY_VS
#include "UnityVxcm.cginc"


				float4 _LightColor0; // Colour of the light used in this pass.

			sampler2D _MainTex;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			uniform float _Cutoff;

			struct appdata_vxcm_object_new
			{
				float4 pos : SV_POSITION;
				float3 lightDir : TEXCOORD0;
				float2 uv : TEXCOORD1;
				//	float3 volumePos: TEXCOORD3; // for volume
				float4 localPos: TEXCOORD2;// for volume
				LIGHTING_COORDS(3,4)
			};

			struct dummy_light
			{
				unityShadowCoord3 _LightCoord;
				unityShadowCoord3 _ShadowCoord;
			};

			inline appdata_vxcm_object_new vert_vxcm_object_new(appdata v)
			{
				appdata_vxcm_object_new o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
				//	o.volumePos = mul(u_objectToVolumeTrx, v.vertex).xyz;
				o.localPos = v.vertex;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			struct fragOut
			{
				float4 color : COLOR;
				float depth : DEPTH;
			};

		//	#define TRANSFER_SHADOW(a) a._ShadowCoord = mul( unity_World2Shadow[0], mul( _Object2World, objCoord ) );

			fragOut frag( appdata_vxcm_object_new i)
			{
				//	return float4(1,0,0, 1);
				fragOut o;
				o.color = half4(0, 0, 0, 0);
				o.depth = 0;

				// ------------

				//	float3 rayOriginTex = i.volumePos;
				float3 rayOriginTex = mul(u_objectToVolumeTrx, i.localPos).xyz;
				float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
				float3 rayDirTex = normalize(i.localPos - localCameraPos.xyz);

				//o.color = half4(rayOriginTex,1);
				//return o;

				// ------------

				float4 texcol = raycast(rayOriginTex, rayDirTex);

				if (texcol.a>0)
				{

					// get normal in object space
					float4 samplePos = float4(rayOriginTex + rayDirTex * texcol.x, 1);

					float4 objCoord = mul(u_objectToVolumeInvTrx, samplePos);
					float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
					o.depth = vpos.z / vpos.w;

					float3 lightDir = normalize(ObjSpaceLightDir(objCoord));

					float3 N = normalize(calcNormal(samplePos.xyz));

					// render

					float3 L = normalize(lightDir);

					//i.pos = vpos;
					//i.worldPos = objCoord;
					// preparo la struttura modificata
					dummy_light dummy;
					dummy._LightCoord = float4(mul(_LightMatrix0, mul(_Object2World, objCoord)).xyz,1);
				//	dummy._ShadowCoord = mul(unity_World2Shadow[0], mul(_Object2World, objCoord));
					dummy._ShadowCoord = mul(_Object2World, objCoord).xyz - _LightPositionRange.xyz;
					//TRANSFER_SHADOW(i)
					float attenuation = LIGHT_ATTENUATION(dummy) ;  // Macro to get you the combined shadow & attenuation value.
					float4 ambient = UNITY_LIGHTMODEL_AMBIENT * 2;

					float NdotL = saturate(dot(N, L));
					float4 diffuseTerm = NdotL * _LightColor0 * attenuation;

					float4 diffuseColor = tex2D(_MainTex, i.uv) * _Color;

					float4 finalColor = (ambient + diffuseTerm) * diffuseColor;

					o.color = finalColor;// texcol;

										 //	o.color = float4(NdotL,0,0, 1);
										 //	o.color = float4(objCoord.xyz, 1);
					o.color = float4(N * attenuation, 1);
					//o.color = float4(attenuation,0,0, 1);
					//o.color = float4(diffuseTerm.xyz, 1);

					// depth, to eye coordinate

				}

				clip(o.color.a - 1.0);

				return o;

			}

			ENDCG
			}

	}
	//FallBack "Diffuse" //note: for passes: ForwardBase, ShadowCaster, ShadowCollector	
	Fallback "VertexLit"
}