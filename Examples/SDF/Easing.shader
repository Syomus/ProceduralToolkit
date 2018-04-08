Shader "Procedural Toolkit/Examples/Easing"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {} // Needed for UI
        _BackgroundColor("Background Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _EasingColor("Easing Color", Color) = (1.0, 0.0, 0.0, 1.0)
        _PlotColor("Plot Color", Color) = (0.0, 1.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Assets/ProceduralToolkit/Shaders/SDF.cginc"
            #include "Assets/ProceduralToolkit/Shaders/Easing.cginc"

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

            float Plot(float p, float y)
            {
                float width = 0.02;
                return smoothstep(p - width, p, y) - smoothstep(p, p + width, y);
            }

            fixed4 _BackgroundColor;
            fixed4 _EasingColor;
            fixed4 _PlotColor;

            fixed4 frag(v2f i) : SV_Target
            {
                float time = TriangleWave(_Time.y*0.3);

                float2 uv = i.uv;
                float2 tiling = float2(3.0, 7.0);
                float2 cell2 = TileIO(uv, tiling);
                float cell = cell2.y*tiling.x + cell2.x;

                float f = time;
                float p = uv.x;
                // Quadratic
                if (cell < 1.0)
                {
                    f = EaseInQuad(time);
                    p = EaseInQuad(uv.x);
                }
                else if (cell < 2.0)
                {
                    f = EaseOutQuad(time);
                    p = EaseOutQuad(uv.x);
                }
                else if (cell < 3.0)
                {
                    f = EaseInOutQuad(time);
                    p = EaseInOutQuad(uv.x);
                }
                // Cubic
                else if (cell < 4.0)
                {
                    f = EaseInCubic(time);
                    p = EaseInCubic(uv.x);
                }
                else if (cell < 5.0)
                {
                    f = EaseOutCubic(time);
                    p = EaseOutCubic(uv.x);
                }
                else if (cell < 6.0)
                {
                    f = EaseInOutCubic(time);
                    p = EaseInOutCubic(uv.x);
                }
                // Quartic
                else if (cell < 7.0)
                {
                    f = EaseInQuart(time);
                    p = EaseInQuart(uv.x);
                }
                else if (cell < 8.0)
                {
                    f = EaseOutQuart(time);
                    p = EaseOutQuart(uv.x);
                }
                else if (cell < 9.0)
                {
                    f = EaseInOutQuart(time);
                    p = EaseInOutQuart(uv.x);
                }
                // Quintic
                else if (cell < 10.0)
                {
                    f = EaseInQuint(time);
                    p = EaseInQuint(uv.x);
                }
                else if (cell < 11.0)
                {
                    f = EaseOutQuint(time);
                    p = EaseOutQuint(uv.x);
                }
                else if (cell < 12.0)
                {
                    f = EaseInOutQuint(time);
                    p = EaseInOutQuint(uv.x);
                }
                // Sine
                else if (cell < 13.0)
                {
                    f = EaseInSine(time);
                    p = EaseInSine(uv.x);
                }
                else if (cell < 14.0)
                {
                    f = EaseOutSine(time);
                    p = EaseOutSine(uv.x);
                }
                else if (cell < 15.0)
                {
                    f = EaseInOutSine(time);
                    p = EaseInOutSine(uv.x);
                }
                // Expo
                else if (cell < 16.0)
                {
                    f = EaseInExpo(time);
                    p = EaseInExpo(uv.x);
                }
                else if (cell < 17.0)
                {
                    f = EaseOutExpo(time);
                    p = EaseOutExpo(uv.x);
                }
                else if (cell < 18.0)
                {
                    f = EaseInOutExpo(time);
                    p = EaseInOutExpo(uv.x);
                }
                // Circ
                else if (cell < 19.0)
                {
                    f = EaseInCirc(time);
                    p = EaseInCirc(uv.x);
                }
                else if (cell < 20.0)
                {
                    f = EaseOutCirc(time);
                    p = EaseOutCirc(uv.x);
                }
                else if (cell < 21.0)
                {
                    f = EaseInOutCirc(time);
                    p = EaseInOutCirc(uv.x);
                }

                float4 color = _BackgroundColor;
                color = lerp(color, _EasingColor, step(uv.x, f));
                color = lerp(color, _PlotColor, Plot(p, uv.y));
                return color;
            }
            ENDCG
        }
    }
}
