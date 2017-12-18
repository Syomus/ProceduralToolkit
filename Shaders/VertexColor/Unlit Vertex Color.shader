Shader "Procedural Toolkit/Vertex Color/Unlit"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
    
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 position : POSITION;
                fixed4 color : COLOR;
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
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            { 
                return i.color;
            }

            ENDCG
        }
    }
}
