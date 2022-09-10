Shader "Procedural Toolkit/Examples/Star Polygon"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {} // Needed for UI
        _Vertices("Vertices", Range(3, 10)) = 5
        _OffsetAngle("Offset Angle", Range(0, 90)) = 36
        [Toggle] _Cheap("Cheap", Float) = 0.0
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
            #pragma multi_compile __ _CHEAP_ON
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

            float _Vertices;
            float _OffsetAngle;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - float2(0.5, 0.5);
                float radius = 0.4;
#if _CHEAP_ON
                float d = StarPolygonCheap(uv, _Vertices, radius, radians(_OffsetAngle));
#else
                float d = StarPolygon(uv, _Vertices, radius, radians(_OffsetAngle));
#endif

#if _DEBUG_ON
                return DebugValue(d);
#else
                return InverseSmoothStep0(d);
#endif
            }
            ENDCG
        }
    }
}
