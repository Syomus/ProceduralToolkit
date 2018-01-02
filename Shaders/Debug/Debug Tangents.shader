Shader "Procedural Toolkit/Debug/Tangents"
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
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.position);
                o.color = v.tangent * 0.5 + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
