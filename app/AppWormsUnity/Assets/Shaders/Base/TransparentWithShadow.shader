Shader "Vxcm/Transparent" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff vertex:vert

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
		//	o.customColor = abs(v.normal);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			//o.Normal = float3(1, 1, 0);

			//float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz)) * v.tangent.w; \
			//float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal)

			//TANGENT_SPACE_ROTATION; // creates the rotation matrix
			//o.Normal = mul(rotation, normalize(IN.worldPos)).xyz;
			
		}
		ENDCG
	}

	Fallback "Transparent/Cutout/VertexLit"
}