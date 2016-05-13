Shader "Vxcm/RayCast shader" {
    
    Properties{
		_Volume            ("Volume (Scalar)", 3D) = "white"{}
    }
    SubShader {
		Tags { 
		//	"Queue" = "Transparent" 
		} 
        Pass {
            Cull Back
           // Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM 
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag 
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : POSITION;
				float3 uv : TEXCOORD0;
                float3 world_pos : TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f output;
                output.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                output.world_pos = mul(_Object2World, v.vertex).xyz;
				output.uv = v.vertex.xyz*0.5 + 0.5;
                return output;
            }
			 
            sampler3D _Volume;

            float4 frag( v2f input ) : COLOR 
            {  
				float4 dst = tex3D (_Volume, input.uv);
               // dst = float4(1.0, 0.0, 0.0, 1.0);
                return dst;
            }

            ENDCG
        }
    }
		
	Fallback "VertexLit"
}