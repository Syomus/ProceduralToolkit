Shader "Procedural Toolkit/Examples/Animation"
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
            #include "Assets/ProceduralToolkit/Shaders/SDF.cginc"
            #include "Assets/ProceduralToolkit/Shaders/Easing.cginc"
            #include "Assets/ProceduralToolkit/Shaders/Transitions.cginc"
            //#include "Packages/com.syomus.proceduraltoolkit/Shaders/Common.cginc"
            //#include "Packages/com.syomus.proceduraltoolkit/Shaders/SDF.cginc"
            //#include "Packages/com.syomus.proceduraltoolkit/Shaders/Easing.cginc"
            //#include "Packages/com.syomus.proceduraltoolkit/Shaders/Transitions.cginc"

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

            float AnimatedCircle(float2 p, float time01)
            {
                float time0 = LinePulse(time01, 0.0, 0.25, 0.5, 0.75);
                float time1 = LinePulse(frac(time01 + 0.25), 0.0, 0.25, 0.5, 0.75);
                p -= float2(lerp(0.25, 0.75, EaseInOutExpo(time0)), lerp(0.25, 0.75, EaseInOutExpo(time1)));
                return Circle(p, 0.15);
            }

            float AnimatedCircles(float2 p, float time01)
            {
                p -= float2(1.0, 0.0)*(1.0 - EaseOutExpo(LineStep(0.0, 0.2, time01)));
                float d = AnimatedCircle(p, time01);
                d = Union(d, AnimatedCircle(p, frac(time01 + 0.25)));
                d = Union(d, AnimatedCircle(p, frac(time01 + 0.5)));
                d = Union(d, AnimatedCircle(p, frac(time01 + 0.75)));
                return InverseSmoothStep0(d)*RectanglePulse(time01, 0.0, 1.0);
            }

            float Tiling(float2 p, float time01)
            {
                p = MirrorTile(p, 4.0, 4.0);
                float time = EaseInOutExpo(TriangleWave(time01*2.0));
                float sqrt2 = sqrt(2.0);
                float starSize = 0.5*sqrt2;
                float starAngle = lerp(UNITY_HALF_PI*0.5, UNITY_HALF_PI*0.25, time);
                float squareSize = 0.2*sqrt2;
                float squareVertices = 4.0;
                float squareAngle = lerp(UNITY_HALF_PI*0.25, 0.0, time);

                float d = StarPolygon(p, 8.0, starSize, starAngle);
                d = Union(d, StarPolygon(p - float2(1.0, 1.0), 8.0, starSize, starAngle));

                d = Union(d, StarPolygon(p - float2(0.0, 1.0), squareVertices, squareSize, squareAngle));
                d = Union(d, StarPolygon(p - float2(1.0, 0.0), squareVertices, squareSize, squareAngle));

                d = Difference(d, StarPolygon(p, squareVertices, squareSize, squareAngle));
                d = Difference(d, StarPolygon(p - float2(1.0, 1.0), squareVertices, squareSize, squareAngle));

                float aa = CalculateScreenAA()*4.0;
                return InverseSmoothStep0(d, aa)*RectanglePulse(time01, 0.0, 1.0);
            }

            float Kaleidoscope(float2 p, float time01)
            {
                p = RadialTile(p, 6.0);
                p = MirrorTile(p, 3.0, 3.0);

                float rotation = UNITY_HALF_PI*EaseInOutQuad(frac(time01*2.0));
                float d = Rectangle(RotateCW(p, rotation), float2(0.5, 0.5));
                d = Xor(d, Rectangle(RotateCCW(p - float2(0.2, 0.2), rotation + UNITY_PI*0.1), float2(0.2, 0.2)));
                d = Xor(d, Rectangle(RotateCW(p - float2(0.4, 0.4), rotation + UNITY_PI*0.2), float2(0.4, 0.4)));
                d = Xor(d, Rectangle(RotateCCW(p - float2(0.5, 0.7), rotation + UNITY_PI*0.3), float2(0.4, 0.4)));

                float aa = CalculateScreenAA()*6.0;
                return InverseSmoothStep0(d, aa)*RectanglePulse(time01, 0.0, 1.0);
            }

            float Rings(float2 p, float time01)
            {
                p = RotateCW(p, time01*UNITY_HALF_PI);
                p = Tile(p, 4.0, 4.0);

                float time = EaseInOutSine(TriangleWave(time01*1.0));
                float radius = lerp(0.3, 0.45, time);
                float width = 0.005;
                float d = Ring(p - float2(0.8, 0.5), radius, width);
                d = Union(d, Ring(p - float2(0.2, 0.5), radius, width));
                d = Union(d, Ring(p - float2(0.5, 0.8), radius, width));
                d = Union(d, Ring(p - float2(0.5, 0.2), radius, width));
                d = Union(d, Ring(p - float2(0.5, 0.5), radius, width));
                float aa = CalculateScreenAA()*4.0;
                return InverseSmoothStep0(d, aa)*RectanglePulse(time01, 0.0, 1.0);
            }

            fixed4 _BackgroundColor;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            fixed4 _Color4;

            fixed4 frag(v2f i) : SV_Target
            {
                float time = frac(_Time.y*0.05)*4.0;
                float2 uv = i.uv;
                float2 center = uv - float2(0.5, 0.5);

                fixed4 color = _BackgroundColor;
                color = lerp(color, _Color1, AnimatedCircles(uv, LineStep(0.0, 1.0, time)));
                color = lerp(color, _Color2, Tiling(uv, LineStep(1.0, 2.0, time)));
                color = lerp(color, _Color3, Kaleidoscope(center, LineStep(2.0, 3.0, time)));
                color = lerp(color, _Color4, Rings(center, LineStep(3.0, 4.0, time)));

                color = lerp(color, _BackgroundColor, HorizontalTransition010(uv, LineStep(0.9, 1.1, time)));
                color = lerp(color, _BackgroundColor, CircleTransition010(center, LineStep(1.9, 2.1, time)));
                color = lerp(color, _BackgroundColor, RadialTransition010(center, LineStep(2.9, 3.1, time)));
                color = lerp(color, _BackgroundColor, VerticalTransition01(uv, LineStep(3.9, 4.0, time)));
                return color;
            }
            ENDCG
        }
    }
}
