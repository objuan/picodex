// buona di base
Shader "Vxcm/Object/ray_v07"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,0)
		_Emission("Emissive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.1, 1)) = 0.7
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	//	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_ShadowBias("Shadow Bias", Range(0, 30.0)) = 20.0
	}
	
	// 1 texture stage GPUs
	SubShader{
			//Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
			Tags{  "RenderType" = "Opaque" }
		
	
			Pass{
				Name "MainPass"
				//Tags{ "LightMode" = "Always" }
				Tags{ "LightMode" = "ForwardBase"  }
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

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
				#define VXCM_PROXY_RENDER
				#include "UnityVxcm.cginc"

				float4 _LightColor0;

				sampler2D _MainTex;
				half _Glossiness;
				half _Metallic;
				fixed4 _Color;


				out_vcxm_fs frag(appdata_vxcm i)
				{
					out_vcxm_fs o;
					o.color = half4(0, 0, 0, 0);
					o.depth = 0;

					appdata_vcxm_fs v; // fisso a v altrimenti le funzioni di UNITY non funzionano

					float3 rayOriginTex, rayDirTex;
					getCameraRay(i, rayOriginTex, rayDirTex);

					VXCM_RAYCAST(rayOriginTex, rayDirTex, v);

					if (v.rayEnc.a>0)
					{
						// do a shawon map remap
						remapShadowMap(i, v);

						//o.depth = v.worldPos.z / v.worldPos.w;
						o.depth = (1.0 - v.worldPos.w * _ZBufferParams.w) / (v.worldPos.w * _ZBufferParams.z);

						// get normal in object space
						float3 N = normalize(calcNormal(v));
						float3 lightDir = normalize(ObjSpaceLightDir(v.vertex));

						float attenuation = LIGHT_ATTENUATION(i);  // Macro to get you the combined shadow & attenuation value.

						float4 ambient = 0;// UNITY_LIGHTMODEL_AMBIENT * 2;

						float NdotL = saturate(dot(N, lightDir));

						float4 diffuseTerm = NdotL * _LightColor0 * attenuation * 2;

						float4 diffuseColor = tex2D(_MainTex, i.uv) * _Color;

						float4 finalColor = (ambient + diffuseTerm) * diffuseColor;

					
						o.color = finalColor;// texcol;
					//	o.color.a = 0.5;

											 //	o.color = float4(NdotL,0,0, 1);
											 //	o.color = float4(objCoord.xyz, 1);
						//o.color = float4(N*  attenuation, 1);
						//o.color = float4(attenuation,0,0, 1);
						//o.color = float4(NdotL,0,0, 1);

						// depth, to eye coordinate

					}

					clip(v.rayEnc.a - 1.0);

					return o;

				}

				ENDCG
			}

			Pass{
				Name "SecondPass"
				//Tags{ "LightMode" = "Always" }
				//Tags{ "LightMode" = "ForwardBase" }
			
				Tags{ "LightMode" = "ForwardAdd" }

				AlphaTest  Greater 0.5
				Blend One One
				ZWrite Off
				//Lighting On

				CGPROGRAM

				#pragma vertex vert_vxcm 
				#pragma fragment frag    
				#pragma target 3.0

				//#pragma multi_compile_fwdadd                        // This line tells Unity to compile this pass for forward add, giving attenuation information for the light.
				#pragma multi_compile_fwdadd_fullshadows                

				//#pragma multi_compile_builtin
				//#pragma multi_compile_fwdbase

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
				#define VXCM_PROXY_RENDER
				#include "UnityVxcm.cginc"

				float4 _LightColor0; // Colour of the light used in this pass.

				sampler2D _MainTex;
				half _Glossiness;
				half _Metallic;
				fixed4 _Color;

				out_vcxm_fs frag(appdata_vxcm i)
				{
					out_vcxm_fs o;
					o.color = half4(0, 0, 0, 0);
					o.depth = 0;

					appdata_vcxm_fs v; // fisso a v altrimenti le funzioni di UNITY non funzionano

					float3 rayOriginTex, rayDirTex;
					getCameraRay(i, rayOriginTex, rayDirTex);

					VXCM_RAYCAST(rayOriginTex, rayDirTex, v);

					if (v.rayEnc.a>0)
					{
						// do a shawon map remap
						remapShadowMap(i, v);

					//o.depth = v.worldPos.z / v.worldPos.w;
						o.depth = (1.0 - v.worldPos.w * _ZBufferParams.w) / (v.worldPos.w * _ZBufferParams.z);
					//	o.depth = 1.0f / (_ZBufferParams.x * v.worldPos.z + _ZBufferParams.y);


						// get normal in object space
						float3 N = normalize(calcNormal(v));
						float3 lightDir = normalize(ObjSpaceLightDir(v.vertex));

						float attenuation = LIGHT_ATTENUATION(i);  // Macro to get you the combined shadow & attenuation value.

						float4 ambient = 0;// UNITY_LIGHTMODEL_AMBIENT * 2;

						float NdotL = saturate(dot(N, lightDir));

						float4 diffuseTerm = NdotL * _LightColor0 * attenuation * 2;

						float4 diffuseColor = tex2D(_MainTex, i.uv) * _Color;

						float4 finalColor = (ambient + diffuseTerm) * diffuseColor;

						o.color = finalColor;// texcol;

											 //	o.color = float4(NdotL,0,0, 1);
											 //	o.color = float4(objCoord.xyz, 1);
						//o.color = float4(N*  attenuation, 1);
						//o.color = float4(attenuation,0,0, 1);
						//o.color = float4(NdotL,0,0, 1);

						// depth, to eye coordinate

					}

					clip(v.rayEnc.a - 1.0);

					return o;

				}

			ENDCG
			}

	}
	//FallBack "Diffuse" //note: for passes: ForwardBase, ShadowCaster, ShadowCollector	
	Fallback "Vxcm/Base"
}