Shader "Procedural Toolkit/Examples/DistanceOperations"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {} // Needed for UI
        [Toggle] _Debug("Debug", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "PreviewType" = "Plane" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ _DEBUG_ON

            #include "Assets/ProceduralToolkit/Shaders/SDF.cginc"
            //#include "Packages/com.syomus.proceduraltoolkit/Shaders/SDF.cginc"

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

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = (Tile(i.uv, 2.0, 2.0) - float2(0.5, 0.5))*0.5;

                float circle = Circle(uv, 0.12);
                float rectangle = Rectangle(RotateCW(uv, _Time.y), float2(0.1, 0.1));

                float distance;
                if (i.uv.x < 0.5)
                {
                    if (i.uv.y < 0.5)
                    {
                        // Bottom Left
                        distance = Union(circle, rectangle);
                    }
                    else
                    {
                        // Top Left
                        distance = Intersection(circle, rectangle);
                    }
                }
                else
                {
                    if (i.uv.y < 0.5)
                    {
                        // Bottom Right
                        distance = Difference(circle, rectangle);
                    }
                    else
                    {
                        // Top Right
                        distance = Xor(circle, rectangle);
                    }
                }
#if _DEBUG_ON
                return DebugValue(distance);
#else
                return InverseSmoothStep0(distance);
#endif
            }
            ENDCG
        }
    }
}
