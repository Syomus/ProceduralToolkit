Shader "Procedural Toolkit/Standard Vertex Color"
{
    Properties
    {
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM

        #pragma surface surf Standard

        struct Input
        {
            float4 color: Color;
        };

        half _Smoothness;
        half _Metallic;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Albedo = IN.color.rgb;
            o.Alpha = IN.color.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
