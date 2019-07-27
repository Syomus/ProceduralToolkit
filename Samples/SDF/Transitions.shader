Shader "Procedural Toolkit/Examples/Transitions"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {} // Needed for UI
        _BackgroundColor("Background Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _Color1("Color 1", Color) = (1.0, 1.0, 1.0, 1.0)
        _Color2("Color 2", Color) = (1.0, 1.0, 1.0, 1.0)
        _Color3("Color 3", Color) = (1.0, 1.0, 1.0, 1.0)
        _Color4("Color 4", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Assets/ProceduralToolkit/Shaders/Common.cginc"
            #include "Assets/ProceduralToolkit/Shaders/Easing.cginc"
            #include "Assets/ProceduralToolkit/Shaders/Transitions.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _BackgroundColor;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            fixed4 _Color4;

            fixed4 frag(v2f i) : SV_Target
            {
                float time01 = frac(_Time.y*0.2);
                fixed4 color = _BackgroundColor;

                float d = HorizontalTransition010(i.uv, EaseInOutQuad(LineStep(0.0, 0.25, time01)));
                color = lerp(color, _Color1, d);

                d = VerticalTransition010(i.uv, EaseInOutQuad(LineStep(0.25, 0.5, time01)));
                color = lerp(color, _Color2, d);

                d = RadialTransition010(i.uv - float2(0.5, 0.5), EaseInOutQuad(LineStep(0.5, 0.75, time01)));
                color = lerp(color, _Color3, d);

                d = CircleTransition010(i.uv - float2(0.5, 0.5), EaseInOutQuad(LineStep(0.75, 1.0, time01)));
                color = lerp(color, _Color4, d);

                return color;
            }
            ENDCG
        }
    }
}
