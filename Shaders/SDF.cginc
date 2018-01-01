#ifndef PROCEDURAL_TOOLKIT_SDF_INCLUDED
#define PROCEDURAL_TOOLKIT_SDF_INCLUDED

//
// Collection of signed distance functions
//

#include "UnityCG.cginc"
#include "Common.cginc"

//
// Space transformations
//

//
// Tile 1D
//

float Tile(float p, float tiling)
{
    return frac(p*tiling);
}

float TileIO(inout float p, float tiling)
{
    p *= tiling;
    float cell = floor(p);
    p = frac(p);
    return cell;
}

float MirrorTile(float p, float tiling)
{
    p *= tiling;
    float cell = floor(p);
    p = frac(p);
    p = lerp(p, 1.0 - p, abs(fmod(cell, 2.0)));
    return p;
}

float MirrorTileIO(inout float p, float tiling)
{
    p *= tiling;
    float cell = floor(p);
    p = frac(p);
    p = lerp(p, 1.0 - p, abs(fmod(cell, 2.0)));
    return cell;
}

//
// Tile 2D
//

float2 Tile(float2 p, float2 tiling)
{
    return frac(p*tiling);
}

float2 Tile(float2 p, float tilingX, float tilingY)
{
    return Tile(p, float2(tilingX, tilingY));
}

float2 TileIO(inout float2 p, float2 tiling)
{
    p *= tiling;
    float2 cell = floor(p);
    p = frac(p);
    return cell;
}

float2 TileIO(inout float2 p, float tilingX, float tilingY)
{
    return TileIO(p, float2(tilingX, tilingY));
}

float2 MirrorTile(float2 p, float2 tiling)
{
    p *= tiling;
    float2 cell = floor(p);
    p = frac(p);
    p = lerp(p, float2(1.0, 1.0) - p, abs(fmod(cell, float2(2.0, 2.0))));
    return p;
}

float2 MirrorTile(float2 p, float tilingX, float tilingY)
{
    return MirrorTile(p, float2(tilingX, tilingY));
}

float2 MirrorTileIO(inout float2 p, float2 tiling)
{
    p *= tiling;
    float2 cell = floor(p);
    p = frac(p);
    p = lerp(p, float2(1.0, 1.0) - p, abs(fmod(cell, float2(2.0, 2.0))));
    return cell;
}

float2 MirrorTileIO(inout float2 p, float tilingX, float tilingY)
{
    return MirrorTileIO(p, float2(tilingX, tilingY));
}

float2 BrickTile(float2 p, float2 tiling, float xOffset)
{
    p *= tiling;
    p.x -= abs(fmod(floor(p.y), 2.0))*xOffset;
    p = frac(p);
    return p;
}

float2 BrickTile(float2 p, float tilingX, float tilingY, float xOffset)
{
    return BrickTile(p, float2(tilingX, tilingY), xOffset);
}

float2 BrickTileIO(inout float2 p, float2 tiling, float xOffset)
{
    p *= tiling;
    p.x -= abs(fmod(floor(p.y), 2.0))*xOffset;
    float2 cell = floor(p);
    p = frac(p);
    return cell;
}

float2 BrickTileIO(inout float2 p, float tilingX, float tilingY, float xOffset)
{
    return BrickTileIO(p, float2(tilingX, tilingY), xOffset);
}

float2 RadialTile(float2 p, float segments)
{
    float segmentAngle = UNITY_TWO_PI / segments;
    float halfSegmentAngle = segmentAngle*0.5;
    float angleRadians = atan2(-p.x, -p.y) + UNITY_PI + halfSegmentAngle;
    float repeat = fmod(angleRadians, segmentAngle) - halfSegmentAngle;
    p = float2(sin(repeat), cos(repeat))*length(p);
    return p;
}

float RadialTileIO(inout float2 p, float segments)
{
    float segmentAngle = UNITY_TWO_PI/segments;
    float halfSegmentAngle = segmentAngle*0.5;
    float angleRadians = atan2(-p.x, -p.y) + UNITY_PI + halfSegmentAngle;
    float cell = fmod(floor(angleRadians/segmentAngle), segments);
    float repeat = fmod(angleRadians, segmentAngle) - halfSegmentAngle;
    p = float2(sin(repeat), cos(repeat))*length(p);
    return cell;
}

//
// Rotate 2D space
//

float2 RotateCW(float2 p, float angleRadians)
{
    return cos(angleRadians)*p + sin(angleRadians)*float2(-p.y, p.x);
}

float2 RotateCCW(float2 p, float angleRadians)
{
    return cos(angleRadians)*p + sin(angleRadians)*float2(p.y, -p.x);
}

float2 RotateCW45(float2 p)
{
    return (p + float2(-p.y, p.x))*sqrt(0.5);
}

float2 RotateCCW45(float2 p)
{
    return (p + float2(p.y, -p.x))*sqrt(0.5);
}

float2 RotateCW90(float2 p)
{
    return float2(-p.y, p.x);
}

float2 RotateCCW90(float2 p)
{
    return float2(p.y, -p.x);
}

//
// Distance operations
//

float Union(float a, float b)
{
    return min(a, b);
}

float Intersection(float a, float b)
{
    return max(a, b);
}

float Difference(float a, float b)
{
    return max(a, -b);
}

float Xor(float a, float b)
{
    return max(min(a, b), min(-a, -b));
}

//
// Half-space
//

float HalfSpace(float2 p, float2 normal)
{
    return dot(p, normal);
}

float HalfSpaceStep(float2 p, float2 normal)
{
    return step(HalfSpace(p, normal), 0.0);
}

float HalfSpaceSmoothStep(float2 p, float2 normal)
{
    return InverseSmoothStep0(HalfSpace(p, normal));
}

float HalfSpaceSmoothStep(float2 p, float2 normal, float aa)
{
    return InverseSmoothStep0(HalfSpace(p, normal), aa);
}

//
// Space segment
//

float SpaceSegment(float2 p, float angleRadians)
{
    float2 rotatedP = RotateCW(p, angleRadians);
    float cornerStep = step(p.y, 0.0)*step(rotatedP.y, 0.0);
    float segmentStep = 1.0 - cornerStep;

    float h1 = HalfSpace(p, float2(-1.0, 0.0))*segmentStep;
    float h2 = HalfSpace(rotatedP, float2(1.0, 0.0))*segmentStep;
    float segment = angleRadians > UNITY_PI ? Union(h1, h2) : Intersection(h1, h2);
    float corner = -length(p)*cornerStep*sign(angleRadians - UNITY_PI);
    return segment + corner;
}

float SpaceSegmentStep(float2 p, float angleRadians)
{
    return step(SpaceSegment(p, angleRadians), 0.0);
}

float SpaceSegmentSmoothStep(float2 p, float angleRadians)
{
    return InverseSmoothStep0(SpaceSegment(p, angleRadians));
}

float SpaceSegmentSmoothStep(float2 p, float angleRadians, float aa)
{
    return InverseSmoothStep0(SpaceSegment(p, angleRadians), aa);
}

//
// Circle
//

float Circle(float2 p, float radius)
{
    return length(p) - radius;
}

float CircleStep(float2 p, float radius)
{
    return step(Circle(p, radius), 0.0);
}

float CircleSmoothStep(float2 p, float radius)
{
    return InverseSmoothStep0(Circle(p, radius));
}

float CircleSmoothStep(float2 p, float radius, float aa)
{
    return InverseSmoothStep0(Circle(p, radius), aa);
}

//
// Ring
//

float Ring(float2 p, float radius, float width)
{
    return abs(length(p) - radius + width) - width;
}

float RingStep(float2 p, float radius, float width)
{
    return step(Ring(p, radius, width), 0.0);
}

float RingSmoothStep(float2 p, float radius, float width)
{
    return InverseSmoothStep0(Ring(p, radius, width));
}

float RingSmoothStep(float2 p, float radius, float width, float aa)
{
    return InverseSmoothStep0(Ring(p, radius, width), aa);
}

//
// Cheap ellipse with inexact distance to poles
//

float EllipseCheap(float2 p, float2 size)
{
    return (length(p/size) - 1.0)*min(size.x, size.y);
}

float EllipseCheapStep(float2 p, float2 size)
{
    return step(EllipseCheap(p, size), 0.0);
}

float EllipseCheapSmoothStep(float2 p, float2 size)
{
    return InverseSmoothStep0(EllipseCheap(p, size));
}

float EllipseCheapSmoothStep(float2 p, float2 size, float aa)
{
    return InverseSmoothStep0(EllipseCheap(p, size), aa);
}

//
// Capsule
//

float Capsule(float2 p, float2 a, float2 b, float radius)
{
    float2 toP = p - a;
    float2 direction = b - a;
    float h = saturate(dot(toP, direction)/dot(direction, direction));
    return length(toP - direction*h) - radius;
}

float CapsuleStep(float2 p, float2 a, float2 b, float radius)
{
    return step(Capsule(p, a, b, radius), 0.0);
}

float CapsuleSmoothStep(float2 p, float2 a, float2 b, float radius)
{
    return InverseSmoothStep0(Capsule(p, a, b, radius));
}

float CapsuleSmoothStep(float2 p, float2 a, float2 b, float radius, float aa)
{
    return InverseSmoothStep0(Capsule(p, a, b, radius), aa);
}

//
// Cheap rectangle with inexact distance to corners
//

float RectangleCheap(float2 p, float2 size)
{
    float2 d = abs(p) - size;
    return max(d.x, d.y);
}

float RectangleCheapStep(float2 p, float2 size)
{
    return step(RectangleCheap(p, size), 0.0);
}

float RectangleCheapSmoothStep(float2 p, float2 size)
{
    return InverseSmoothStep0(RectangleCheap(p, size));
}

float RectangleCheapSmoothStep(float2 p, float2 size, float aa)
{
    return InverseSmoothStep0(RectangleCheap(p, size), aa);
}

//
// Rectangle
//

float Rectangle(float2 p, float2 size)
{
    float2 d = abs(p) - size;
    float inside = min(max(d.x, d.y), 0.0);
    float outside = length(max(d, float2(0.0, 0.0)));
    return inside + outside;
}

float RectangleStep(float2 p, float2 size)
{
    return step(Rectangle(p, size), 0.0);
}

float RectangleSmoothStep(float2 p, float2 size)
{
    return InverseSmoothStep0(Rectangle(p, size));
}

float RectangleSmoothStep(float2 p, float2 size, float aa)
{
    return InverseSmoothStep0(Rectangle(p, size), aa);
}

//
// Rectangle frame
//

float RectangleFrame(float2 p, float2 size, float width)
{
    float2 d = abs(p) - size;
    float inside = min(max(d.x, d.y), 0.0);
    float outside = length(max(d, float2(0.0, 0.0)));
    return abs(inside + outside + width) - width;
}

float RectangleFrameStep(float2 p, float2 size, float width)
{
    return step(RectangleFrame(p, size, width), 0.0);
}

float RectangleFrameSmoothStep(float2 p, float2 size, float width)
{
    return InverseSmoothStep0(RectangleFrame(p, size, width));
}

float RectangleFrameSmoothStep(float2 p, float2 size, float width, float aa)
{
    return InverseSmoothStep0(RectangleFrame(p, size, width), aa);
}

//
// Round rectangle
//

float RoundRectangle(float2 p, float2 size, float radius)
{
    float2 d = abs(p) - size + float2(radius, radius);
    float inside = min(max(d.x, d.y), 0.0) - radius;
    float outside = length(max(d, float2(0.0, 0.0)));
    return inside + outside;
}

float RoundRectangleStep(float2 p, float2 size, float radius)
{
    return step(RoundRectangle(p, size, radius), 0.0);
}

float RoundRectangleSmoothStep(float2 p, float2 size, float radius)
{
    return InverseSmoothStep0(RoundRectangle(p, size, radius));
}

float RoundRectangleSmoothStep(float2 p, float2 size, float radius, float aa)
{
    return InverseSmoothStep0(RoundRectangle(p, size, radius), aa);
}

//
// Round rectangle frame
//

float RoundRectangleFrame(float2 p, float2 size, float width, float radius)
{
    float2 d = abs(p) - size + float2(radius, radius);
    float inside = min(max(d.x, d.y), 0.0) - radius;
    float outside = length(max(d, float2(0.0, 0.0)));
    return abs(inside + outside + width) - width;
}

float RoundRectangleFrameStep(float2 p, float2 size, float width, float radius)
{
    return step(RoundRectangleFrame(p, size, width, radius), 0.0);
}

float RoundRectangleFrameSmoothStep(float2 p, float2 size, float width, float radius)
{
    return InverseSmoothStep0(RoundRectangleFrame(p, size, width, radius));
}

float RoundRectangleFrameSmoothStep(float2 p, float2 size, float width, float radius, float aa)
{
    return InverseSmoothStep0(RoundRectangleFrame(p, size, width, radius), aa);
}

//
// Cheap polygon with inexact distance to vertices
//

float PolygonCheap(float2 p, float vertices, float radius)
{
    float segmentAngle = UNITY_TWO_PI/vertices;
    float halfSegmentAngle = segmentAngle*0.5;

    float angleRadians = atan2(p.x, p.y);
    float repeat = fmod(abs(angleRadians), segmentAngle) - halfSegmentAngle;
    float inradius = radius*cos(halfSegmentAngle);
    float circle = length(p);
    float y = cos(repeat)*circle - inradius;
    return y;
}

float PolygonCheapStep(float2 p, float2 vertices, float radius)
{
    return step(PolygonCheap(p, vertices, radius), 0.0);
}

float PolygonCheapSmoothStep(float2 p, float2 vertices, float radius)
{
    return InverseSmoothStep0(PolygonCheap(p, vertices, radius));
}

float PolygonCheapSmoothStep(float2 p, float2 vertices, float radius, float aa)
{
    return InverseSmoothStep0(PolygonCheap(p, vertices, radius), aa);
}

//
// Polygon
//

float Polygon(float2 p, float vertices, float radius)
{
    float segmentAngle = UNITY_TWO_PI/vertices;
    float halfSegmentAngle = segmentAngle*0.5;

    float angleRadians = atan2(p.x, p.y);
    float repeat = fmod(abs(angleRadians), segmentAngle) - halfSegmentAngle;
    float inradius = radius*cos(halfSegmentAngle);
    float circle = length(p);
    float x = sin(repeat)*circle;
    float y = cos(repeat)*circle - inradius;

    float inside = min(y, 0.0);
    float corner = radius*sin(halfSegmentAngle);
    float outside = length(float2(max(abs(x) - corner, 0.0), y))*step(0.0, y);
    return inside + outside;
}

float PolygonStep(float2 p, float2 vertices, float radius)
{
    return step(Polygon(p, vertices, radius), 0.0);
}

float PolygonSmoothStep(float2 p, float2 vertices, float radius)
{
    return InverseSmoothStep0(Polygon(p, vertices, radius));
}

float PolygonSmoothStep(float2 p, float2 vertices, float radius, float aa)
{
    return InverseSmoothStep0(Polygon(p, vertices, radius), aa);
}

//
// Cheap star polygon with inexact distance to vertices
//

float StarPolygonCheap(float2 p, float vertices, float radius, float starAngle)
{
    float segmentAngle = UNITY_TWO_PI/vertices;
    float halfSegmentAngle = segmentAngle*0.5;

    float angleRadians = atan2(p.x, p.y);
    float repeat = abs(frac(angleRadians/segmentAngle - 0.5) - 0.5)*segmentAngle;
    float circle = length(p);
    float x = sin(repeat)*circle;
    float y = cos(repeat)*circle - radius;
    float uvRotation = halfSegmentAngle + starAngle;
    y = cos(uvRotation)*y + sin(uvRotation)*x;
    return y;
}

float StarPolygonCheapStep(float2 p, float2 vertices, float radius, float starAngle)
{
    return step(StarPolygonCheap(p, vertices, radius, starAngle), 0.0);
}

float StarPolygonCheapSmoothStep(float2 p, float2 vertices, float radius, float starAngle)
{
    return InverseSmoothStep0(StarPolygonCheap(p, vertices, radius, starAngle));
}

float StarPolygonCheapSmoothStep(float2 p, float2 vertices, float radius, float starAngle, float aa)
{
    return InverseSmoothStep0(StarPolygonCheap(p, vertices, radius, starAngle), aa);
}

//
// Star polygon
//

float StarPolygon(float2 p, float vertices, float radius, float starAngle)
{
    float segmentAngle = UNITY_TWO_PI/vertices;
    float halfSegmentAngle = segmentAngle*0.5;

    float angleRadians = atan2(p.x, p.y);
    float repeat = abs(frac(angleRadians/segmentAngle - 0.5) - 0.5)*segmentAngle;
    float circle = length(p);
    float x = sin(repeat)*circle;
    float y = cos(repeat)*circle - radius;
    float uvRotation = halfSegmentAngle + starAngle;
    float2 uv = cos(uvRotation)*float2(x, y) + sin(uvRotation)*float2(-y, x);

    float corner = radius*sin(halfSegmentAngle)/cos(starAngle);
    float inside = -length(float2(max(uv.x - corner, 0.0), uv.y))*step(0.0, -uv.y);
    float outside = length(float2(min(uv.x, 0.0), uv.y))*step(0.0, uv.y);
    return inside + outside;
}

float StarPolygonStep(float2 p, float2 vertices, float radius, float starAngle)
{
    return step(StarPolygon(p, vertices, radius, starAngle), 0.0);
}

float StarPolygonSmoothStep(float2 p, float2 vertices, float radius, float starAngle)
{
    return InverseSmoothStep0(StarPolygon(p, vertices, radius, starAngle));
}

float StarPolygonSmoothStep(float2 p, float2 vertices, float radius, float starAngle, float aa)
{
    return InverseSmoothStep0(StarPolygon(p, vertices, radius, starAngle), aa);
}

#endif
