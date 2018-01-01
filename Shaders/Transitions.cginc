#ifndef PROCEDURAL_TOOLKIT_TRANSITIONS_INCLUDED
#define PROCEDURAL_TOOLKIT_TRANSITIONS_INCLUDED

//
// Collection of transition animations
//

#include "SDF.cginc"

float HorizontalTransition01(float2 p, float time01)
{
    float d = p.x - time01;
    return InverseSmoothStep0(d)*RectangleStep(0.0, time01);
}

float HorizontalTransition010(float2 p, float time01)
{
    float t1 = LineStep(0.0, 0.5, time01);
    float t2 = LineStep(0.5, 1.0, time01);
    float d = Difference(p.x - t1, p.x - t2);
    return InverseSmoothStep0(d)*RectanglePulse(time01, 0.0, 1.0);
}

float VerticalTransition01(float2 p, float time01)
{
    float d = p.y - time01;
    return InverseSmoothStep0(d)*RectangleStep(0.0, time01);
}

float VerticalTransition010(float2 p, float time01)
{
    float t1 = LineStep(0.0, 0.5, time01);
    float t2 = LineStep(0.5, 1.0, time01);
    float d = Difference(p.y - t1, p.y - t2);
    return InverseSmoothStep0(d)*RectanglePulse(time01, 0.0, 1.0);
}

float RadialTransition01(float2 p, float time01)
{
    float d = SpaceSegment(p, time01*UNITY_TWO_PI);
    return InverseSmoothStep0(d)*RectangleStep(0.0, time01);
}

float RadialTransition010(float2 p, float time01)
{
    float t1 = LineStep(0.0, 0.5, time01);
    float t2 = LineStep(0.5, 1.0, time01);
    float d = Difference(SpaceSegment(p, t1*UNITY_TWO_PI), SpaceSegment(p, t2*UNITY_TWO_PI));
    return InverseSmoothStep0(d)*RectanglePulse(time01, 0.0, 1.0);
}

float CircleTransition01(float2 p, float time01)
{
    const float radius = 1.5;
    float d = Circle(p, time01*radius);
    return InverseSmoothStep0(d)*RectangleStep(0.0, time01);
}

float CircleTransition010(float2 p, float time01)
{
    float t1 = LineStep(0.0, 0.5, time01);
    float t2 = LineStep(0.5, 1.0, time01);
    const float radius = 1.5;
    float d = Difference(Circle(p, t1*radius), Circle(p, t2*radius));
    return InverseSmoothStep0(d)*RectanglePulse(time01, 0.0, 1.0);
}

#endif
