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
			#pragma vertex vert_vxcm_object
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"
			#define VXCM_PROXY_VS
			#include "UnityVxcm.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
			};

			uniform float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			uniform sampler2D _MainTex;
			uniform float _Cutoff;
			uniform float4 _Color;
			uniform float3 u_cameraInfoLight;

			//float4 frag(v2f i) : COLOR
			float4 frag(appdata_vxcm_object i) : COLOR
			{
				float3 rayOriginTex = i.volumePos;
				float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
				float3 rayDirTex = normalize(i.localPos - localCameraPos.xyz);

				//

				float4 texcol = raycast(rayOriginTex, rayDirTex);

				float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * texcol.x, 1));
				float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
				float depth  = vpos.z / vpos.w;

				//half4 texcol = tex2D(_MainTex, i.uv);
				clip(texcol.a*_Color.a - _Cutoff);

				//SHADOW_CASTER_FRAGMENT(i)
				//UNITY_OUTPUT_DEPTH(i.hpos.zw);
				//UNITY_OUTPUT_DEPTH(0);
				return float4(0, 0, 0, 0);
			}
		ENDCG

		}
	

	// Pass to render object as a shadow collector
	//Pass{
	//		Name "ShadowCollector"
	//		Tags{ "LightMode" = "ShadowCollector" }

	//		Fog{ Mode Off }
	//		ZWrite On ZTest Less

	//		CGPROGRAM
	//		#pragma vertex vert_vxcm_object
	//		#pragma fragment frag
	//		#pragma fragmentoption ARB_precision_hint_fastest
	//		#pragma multi_compile_shadowcollector

	//		#define SHADOW_COLLECTOR_PASS
	//		#include "UnityCG.cginc"
	//		#define VXCM_PROXY_VS
	//		#include "UnityVxcm.cginc"

	//		struct v2f {
	//			V2F_SHADOW_COLLECTOR;
	//			float2  uv : TEXCOORD5;
	//		};

	//		uniform float4 _MainTex_ST;

	//		v2f vert(appdata_base v)
	//		{
	//			v2f o;
	//			TRANSFER_SHADOW_COLLECTOR(o)
	//			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	//			return o;
	//		}

	//		uniform sampler2D _MainTex;
	//		uniform float _Cutoff;
	//		uniform float4 _Color;

	//		half4 fra2g(appdata_vxcm_object i) : COLOR
	//		{
	//			//half4 texcol = tex2D(_MainTex, i.uv);
	//			//clip(texcol.a*_Color.a - _Cutoff);
	//			float3 rayDirTex, rayOriginTex;
	//			vxcm_getPixelRay(i, rayDirTex, rayOriginTex);

	//			float4 texcol = raycast(rayOriginTex, rayDirTex);

	//			float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * texcol.x, 1));
	//			float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
	//			float depth = vpos.z / vpos.w;

	//			//half4 texcol = tex2D(_MainTex, i.uv);
	//			clip(texcol.a*_Color.a - _Cutoff);

	//			//SHADOW_COLLECTOR_FRAGMENT(i)
	//			return half14(0, 0, 0, 0);
	//		}
	//		ENDCG

	//	}
	
	} // END SUBSHADER
	
	// 1 texture stage GPUs
	SubShader{
			Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		
	
			Pass{
				Name "MainPass"
				Tags{ "LightMode" = "Always" }

				Fog{ Mode Off }
				ZWrite On ZTest Less
				AlphaTest  Greater 0.5

				CGPROGRAM

				#include "UnityCG.cginc"
				#define VXCM_PROXY_VS
				#include "UnityVxcm.cginc"

				#pragma vertex vert_vxcm_object 
				#pragma fragment frag    
				#pragma target 3.0

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

					// ------------

					float3 rayOriginTex = i.volumePos;
					float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
					float3 rayDirTex = normalize(i.localPos  - localCameraPos.xyz);

					//o.color = half4(rayOriginTex,1);
					//return o;

					// ------------

					float4 texcol = raycast(rayOriginTex, rayDirTex);

					if (texcol.a>0)
					{
						
						// to eye coordinate
						float4 objCoord = mul(u_objectToVolumeInvTrx, float4(rayOriginTex + rayDirTex * texcol.x,1));
						float4 vpos = mul(UNITY_MATRIX_MVP, objCoord);
						o.depth = vpos.z / vpos.w;
						o.color = texcol;
					}

					clip(o.color.a-1.0);

					return o;

				}

				ENDCG
			}
			
	}
}