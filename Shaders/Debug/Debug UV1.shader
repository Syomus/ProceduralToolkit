Shader "Procedural Toolkit/Debug/UV1"
{
    SubShader
    {
        Pass
        {
            Fog { Mode Off }
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.position);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 color = fixed4(frac(i.uv), 0, 0);
                if (any(saturate(i.uv) - i.uv))
                {
                    color.b = 0.5;
                }
                return color;
            }

            ENDCG
        }
    }
}
