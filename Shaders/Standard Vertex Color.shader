Shader "Procedural Toolkit/Standard Vertex Color" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard

		struct Input
		{
			float4 color: Color;
		};

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = IN.color.rgb;
			o.Alpha = IN.color.a;
		}

		ENDCG
	}
}
