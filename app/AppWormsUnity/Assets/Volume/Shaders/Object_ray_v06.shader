Shader "Vxcm/Object/ray_v06"
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
	Pass{
			Name "ShadowCollector"
			Tags{ "LightMode" = "ShadowCollector" }

			Fog{ Mode Off }
			ZWrite On ZTest Less

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector

			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_COLLECTOR;
				float2  uv : TEXCOORD5;
			};

			uniform float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_COLLECTOR(o)
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			uniform sampler2D _MainTex;
			uniform float _Cutoff;
			uniform float4 _Color;

			half4 frag(v2f i) : COLOR
			{
				half4 texcol = tex2D(_MainTex, i.uv);
				clip(texcol.a*_Color.a - _Cutoff);

				SHADOW_COLLECTOR_FRAGMENT(i)
			}
			
				ENDCG

		}
	
	} // END SUBSHADER
	
	// 1 texture stage GPUs
	SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200

		CGPROGRAM
		#include "UnityCG.cginc"
		#define VXCM_OBJECT_VS
		#include "UnityVxcm.cginc"

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert //fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform float _Cutoff;

		struct Input {
			float2 uv_MainTex;
			//float3 volumePos;
			float4 localPos;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//o.volumePos = mul(u_objectToVolumeTrx, v.vertex);
			o.localPos = v.vertex;
		}


		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			float4 volumePos = mul(u_objectToVolumeTrx, IN.localPos);

			float3 rayOriginTex = volumePos;
			float4 localCameraPos = multInverse(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
			float3 rayDirTex = normalize(IN.localPos - localCameraPos.xyz);

			float4 texcol = raycast(rayOriginTex, rayDirTex);
			
			// ---------------

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo = texcol.rgb;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			clip(texcol.a*_Color.a - _Cutoff);

		//	o.Albedo = float3(1, 1, 0);
			//o.Metallic = 0.1;
			//o.Alpha = 1;
			//o.Smoothness = 0.5;
		}
		ENDCG
	}
}