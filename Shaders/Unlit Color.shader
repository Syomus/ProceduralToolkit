Shader "Procedural Toolkit/Unlit Color" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
	
		Pass {  
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata
			{
				float4 position : POSITION;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
			};

			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.position);
                return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				return _Color;
			}

			ENDCG
		}
	}
}