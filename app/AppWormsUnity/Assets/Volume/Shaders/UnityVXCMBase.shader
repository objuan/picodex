Shader "Vxcm/Base"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		//_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
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

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#define VXCM_PROXY_SHADOW
			#include "UnityVxcm.cginc"

			//uniform float4 _MainTex_ST;

			appdata_vcxm_shadow vert(appdata_base v)
			{
				appdata_vcxm_shadow o;
				TRANSFER_SHADOW_CASTER(o) 
			//	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.vertex = v.vertex;
				return o;
			}

		//	uniform sampler2D _MainTex;
			uniform float4 _Color;
		
			out_vcxm_fs frag(appdata_vcxm_shadow i)
			{
				out_vcxm_fs o;
				appdata_vcxm_fs v; // fisso a v altrimenti le funzioni di UNITY non funzionano

				float3 rayOriginTex, rayDirTex;
				getShadowRay(i , rayOriginTex, rayDirTex);
			
				VXCM_RAYCAST(rayOriginTex, rayDirTex, v);

				i.pos = v.worldPos;	// sovrrascrivo il valore non corretto
				TRANSFER_SHADOW_CASTER(i); // chiamato qui invece che nel vertex shader, uso del fake_v2f

				// save to the output
#ifdef SHADOWS_CUBE
				float4 enc = UnityEncodeCubeShadowDepth((length(i.vec) + unity_LightShadowBias.x) * _LightPositionRange.w);;
			//	o.depth = i.pos.z / i.pos.w; 
				o.depth = (1.0 - i.pos.w * _ZBufferParams.w) / (i.pos.w * _ZBufferParams.z);

				o.color = enc;
#else
				o.depth = i.pos.z / i.pos.w;
			//	o.depth = Linear01Depth(i.pos.z);
				//o.depth = (1.0 - i.pos.w * _ZBufferParams.w) / (i.pos.w * _ZBufferParams.z);

				o.color = 1;	
#endif
				clip(v.rayEnc.a*_Color.a - 0.5);

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
	
	//FallBack "Diffuse" //note: for passes: ForwardBase, ShadowCaster, ShadowCollector	
	Fallback "VertexLit"
}