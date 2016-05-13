#ifndef u_SHADOW_CAST_CG_INCLUDED
#define u_SHADOW_CAST_CG_INCLUDED

#include "UnityCG.cginc"

float4 _Color;
sampler2D _MainTex;
fixed _Cutoff;

struct v2f {
	V2F_SHADOW_CASTER;
	float2 uv : TEXCOORD1;
};


#endif // u_SHADOW_CAST_CG_INCLUDED
