// con surface
Shader "Vxcm/Object/ray_v08"
{
	Properties{
	//	_SpecColor("Spec Color", Color) = (1,1,1,0)
	/*	_Emission("Emissive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.1, 1)) = 0.7*/
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	
	// 1 texture stage GPUs
	SubShader{
			//Tags{ LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
			Tags{  "RenderType" = "Opaque"  "SHADOWSUPPORT" = "true"  }
		
	
			Pass{
				Name "MainPass"
				Tags{ "LightMode" = "ForwardBase" }
				//Tags{ "LightMode" = "ForwardBase"  }
			//	Tags{ "LightMode" = "ForwardAdd" }

			//	Fog{ Mode Off }
			//	ZWrite On ZTest Less
			//	AlphaTest  Greater 0.5
			//	Lighting On

				CGPROGRAM

				#pragma vertex vert_vxcm 
				#pragma fragment frag    
				#pragma target 3.0

				#pragma fragmentoption ARB_precision_hint_fastest

				//#pragma multi_compile_builtin
				#pragma multi_compile_fwdbase
				//#pragma multi_compile_fwdadd_fullshadows

			// LIGHT MODEL
			//	#define UNITY_SETUP_BRDF_INPUT

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
				#include "UnityStandardCore.cginc"
				//#define UNITY_STANDARD_SIMPLE
				#include "UnityStandardCoreForward.cginc"
				//#include "UnityStandardCoreForwardSimple.cginc"
				#define VXCM_PROXY_RENDER
				#include "UnityVxcm.cginc"

			//	float4 _LightColor0;

			/*	sampler2D _MainTex;
				half _Glossiness;
				half _Metallic;
				fixed4 _Color;*/


				out_vcxm_fs frag(appdata_vxcm app_in)
				{
					out_vcxm_fs o;
					o.color = half4(0, 0, 0, 0);
					o.depth = 0;

					appdata_vcxm_fs v; // fisso a v altrimenti le funzioni di UNITY non funzionano

					float3 rayOriginTex, rayDirTex;
					getCameraRay(app_in, rayOriginTex, rayDirTex);

					VXCM_RAYCAST(rayOriginTex, rayDirTex, v);

					if (v.rayEnc.a>0)
					{
						// do a shawon map remap
						remapShadowMap(app_in, v);
						o.depth = v.worldPos.z / v.worldPos.w;
						float3 normal = normalize(calcNormal(v));

						// -----------
						VertexInput vi;

						vi.vertex = v.vertex;
						vi.normal = normal;
						vi.uv0 = app_in.uv;
//#ifdef _TANGENT_TO_WORLD
//						half4 tangent	: TANGENT;
//#endif

						VertexOutputForwardBase i =  vertForwardBase(vi);

						FragmentCommonData s = FragmentSetupSimple(i);

				//		FRAGMENT_SETUP(s) // prende <i> come ingresso
				/*	#if UNITY_OPTIMIZE_TEXCUBELOD
						s.reflUVW = i.reflUVW;
					#endif*/

						UnityLight mainLight = MainLight(s.normalWorld);
						half atten = SHADOW_ATTENUATION(app_in);

						half occlusion = Occlusion(i.tex.xy);
						UnityGI gi = FragmentGI(s, occlusion, i.ambientOrLightmapUV, atten, mainLight);

						half4 c = UNITY_BRDF_PBS(s.diffColor, s.specColor, s.oneMinusReflectivity, s.oneMinusRoughness, s.normalWorld, -s.eyeVec, gi.light, gi.indirect);
					//	c.rgb += UNITY_BRDF_GI(s.diffColor, s.specColor, s.oneMinusReflectivity, s.oneMinusRoughness, s.normalWorld, -s.eyeVec, occlusion, gi);
						c.rgb += Emission(i.tex.xy);

						UNITY_APPLY_FOG(i.fogCoord, c.rgb);
						half4 finalColor = OutputForward(c, s.alpha);

						o.color = finalColor;// texcol;


					}

					clip(v.rayEnc.a - 1.0);

					return o;

				}

				ENDCG
			}

		//Pass{
		//			Name "SecondPass"
		//			//Tags{ "LightMode" = "Always" }
		//			//Tags{ "LightMode" = "ForwardBase" }

		//			Tags{ "LightMode" = "ForwardAdd" }

		//			AlphaTest  Greater 0.5
		//			Blend One One
		//			ZWrite Off
		//			//Lighting On

		//			CGPROGRAM

		//	#pragma vertex vert_vxcm 
		//	#pragma fragment frag    
		//	#pragma target 3.0

		//			//#pragma multi_compile_fwdadd                        // This line tells Unity to compile this pass for forward add, giving attenuation information for the light.
		//#pragma multi_compile_fwdadd_fullshadows                

		//			//#pragma multi_compile_builtin
		//			//#pragma multi_compile_fwdbase

		//	#include "UnityCG.cginc"
		//	#include "AutoLight.cginc"
		//	#define VXCM_PROXY_RENDER
		//	#include "UnityVxcm.cginc"

		//			float4 _LightColor0; // Colour of the light used in this pass.

		//		sampler2D _MainTex;
		//		half _Glossiness;
		//		half _Metallic;
		//		fixed4 _Color;

		//		out_vcxm_fs frag(appdata_vxcm i)
		//		{
		//			out_vcxm_fs o;
		//			o.color = half4(0, 0, 0, 0);
		//			o.depth = 0;

		//			appdata_vcxm_fs v; // fisso a v altrimenti le funzioni di UNITY non funzionano

		//			float3 rayOriginTex, rayDirTex;
		//			getCameraRay(i, rayOriginTex, rayDirTex);

		//			VXCM_RAYCAST(rayOriginTex, rayDirTex, v);

		//			if (v.rayEnc.a>0)
		//			{
		//				// do a shawon map remap
		//				remapShadowMap(i, v);

		//				o.depth = v.worldPos.z / v.worldPos.w;

		//				// get normal in object space
		//				float3 N = normalize(calcNormal(v));
		//				float3 lightDir = normalize(ObjSpaceLightDir(v.vertex));

		//				float attenuation = LIGHT_ATTENUATION(i);  // Macro to get you the combined shadow & attenuation value.

		//				float4 ambient = 0;// UNITY_LIGHTMODEL_AMBIENT * 2;

		//				float NdotL = saturate(dot(N, lightDir));

		//				float4 diffuseTerm = NdotL * _LightColor0 * attenuation * 2;

		//				float4 diffuseColor = tex2D(_MainTex, i.uv) * _Color;

		//				float4 finalColor = (ambient + diffuseTerm) * diffuseColor;

		//				o.color = finalColor;// texcol;
		//				o.color.a = 1;
		//									 //	o.color = float4(NdotL,0,0, 1);
		//									 //	o.color = float4(objCoord.xyz, 1);
		//									 //o.color = float4(N*  attenuation, 1);
		//									 //o.color = float4(attenuation,0,0, 1);
		//									 //o.color = float4(NdotL,0,0, 1);

		//									 // depth, to eye coordinate

		//			}

		//			clip(v.rayEnc.a - 1.0);

		//			return o;

		//		}

		//		ENDCG
		//	}
	}
	//FallBack "Diffuse" //note: for passes: ForwardBase, ShadowCaster, ShadowCollector	
	Fallback "Vxcm/Base"
}