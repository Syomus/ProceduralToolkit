Shader "Procedural Toolkit/Vertex Color/Diffuse"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM

        #pragma surface surf Lambert

        struct Input
        {
            float4 color: Color;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = IN.color.rgb;
            o.Alpha = IN.color.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
