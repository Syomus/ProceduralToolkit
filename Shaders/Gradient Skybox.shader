Shader "Procedural Toolkit/Gradient Skybox"
{
    Properties
    {
        _SkyColor("Sky Color", Color) = (1, 1, 1, 0)
        _HorizonColor("Horizon Color", Color) = (0.25, 0.25, 0.25, 0)
        _GroundColor("Ground Color", Color) = (0.75, 0.75, 0.75, 0)
        _Exponent1("Sky Exponent", Float) = 3
        _Exponent2("Ground Exponent", Float) = 1
        _Intensity("Intensity", Float) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            half3 _SkyColor;
            half3 _HorizonColor;
            half3 _GroundColor;
            half _Exponent1;
            half _Exponent2;
            half _Intensity;

            struct appdata
            {
                float4 position : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.position);
                o.texcoord = v.texcoord;
                return o;
            }

			half4 frag (v2f i) : SV_Target
            {
                float y = normalize(i.texcoord).y;
                float c1 = 1 - pow(min(1, 1 - y), _Exponent1);
                float c3 = 1 - pow(min(1, 1 + y), _Exponent2);
                float c2 = 1 - c1 - c3;

				half3 color = (_SkyColor * c1 + _HorizonColor * c2 + _GroundColor * c3)*_Intensity;
                return half4(color, 1);
            }
            ENDCG
        }
    }

    Fallback Off
}