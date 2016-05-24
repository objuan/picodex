Shader "Vxcm/Pick/v_01"
{
	Properties{
		_Volume("Volume (Scalar)", 3D) = "white"{}
		_SampleRate("Sample rate",float) = 3
	}

	Subshader {

		Pass{
	
			CGPROGRAM

			#include "UnityCG.cginc"
			#define VXCM_RAYCAST_VS
			#include "UnityVxcm.cginc"

			#pragma vertex vert_img
			#pragma vertex vert_vxcm_raycast
			#pragma fragment frag
			
			float _SampleRate;

			float4  frag(appdata_vxcm_raycast i) : COLOR
			{
				return float4 (1,0,0,1);
			}

			ENDCG
		}
	}

} // shader