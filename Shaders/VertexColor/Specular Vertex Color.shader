Shader "Procedural Toolkit/Vertex Color/Specular"
{
    Properties
    {
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM

        #pragma surface surf BlinnPhong

        half _Shininess;

        struct Input
        {
            float4 color: Color;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = IN.color.rgb;
            o.Alpha = IN.color.a;
            o.Gloss = IN.color.a;
            o.Specular = _Shininess;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
