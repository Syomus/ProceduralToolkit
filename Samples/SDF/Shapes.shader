Shader "Procedural Toolkit/Examples/Shapes"
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
                float2 uv = i.uv;
                float cell = 0.125;
                float cell2 = cell*3;
                float cell3 = cell*5;
                float cell4 = cell*7;
                float size = 0.1;
                float2 size2 = float2(size, size*3/4);

                float distance = Circle(uv - float2(cell, cell), size);
                distance = Union(distance, Ring(uv - float2(cell2, cell), size, size/3));
                distance = Union(distance, Capsule(uv - float2(cell3, cell), -float2(size/2, size/2), float2(size/2, size/2), size/3));

                distance = Union(distance, Rectangle(uv - float2(cell, cell2), size2));
                distance = Union(distance, RoundRectangle(uv - float2(cell2, cell2), size2, size/3));
                distance = Union(distance, RectangleFrame(uv - float2(cell3, cell2), size2, size/4));
                distance = Union(distance, RoundRectangleFrame(uv - float2(cell4, cell2), size2, size/4, size/4));

                distance = Union(distance, Polygon(uv - float2(cell, cell3), 5.0, size));
                distance = Union(distance, StarPolygon(uv - float2(cell2, cell3), 5.0, size, UNITY_PI/5.0));

                distance = Union(distance, RectangleCheap(uv - float2(cell, cell4), size2));
                distance = Union(distance, EllipseCheap(uv - float2(cell2, cell4), size2));

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
