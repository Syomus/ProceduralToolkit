#ifndef PROCEDURAL_TOOLKIT_EASING_INCLUDED
#define PROCEDURAL_TOOLKIT_EASING_INCLUDED

//
// Normalized easing functions
//

#include "UnityCG.cginc"

//
// Quadratic
//

float EaseInQuad(float x)
{
    return x*x;
}

float EaseOutQuad(float x)
{
    return x*(2.0 - x);
}

float EaseInOutQuad(float x)
{
    if (x < 0.5)
    {
        return 2.0*x*x;
    }
    return x*(-2.0*x + 4.0) - 1.0;
}

//
// Cubic
//

float EaseInCubic(float x)
{
    return x*x*x;
}

float EaseOutCubic(float x)
{
    x--;
    return x*x*x + 1.0;
}

float EaseInOutCubic(float x)
{
    if (x < 0.5)
    {
        return 4.0*x*x*x;
    }
    x = 2.0*x - 2.0;
    return 0.5*x*x*x + 1.0;
}

//
// Quartic
//

float EaseInQuart(float x)
{
    return x*x*x*x;
}

float EaseOutQuart(float x)
{
    x--;
    return -x*x*x*x + 1.0;
}

float EaseInOutQuart(float x)
{
    if (x < 0.5)
    {
        return 8.0*x*x*x*x;
    }
    x = 2.0*x - 2.0;
    return -0.5*x*x*x*x + 1.0;
}

//
// Quintic
//

float EaseInQuint(float x)
{
    return x*x*x*x*x;
}

float EaseOutQuint(float x)
{
    x--;
    return x*x*x*x*x + 1.0;
}

float EaseInOutQuint(float x)
{
    if (x < 0.5)
    {
        return 16.0*x*x*x*x*x;
    }
    x = 2.0*x - 2.0;
    return 0.5*x*x*x*x*x + 1.0;
}

//
// Sine
//

float EaseInSine(float x)
{
    return -cos(UNITY_HALF_PI*x) + 1.0;
}

float EaseOutSine(float x)
{
    return sin(UNITY_HALF_PI*x);
}

float EaseInOutSine(float x)
{
    return -0.5*cos(UNITY_PI*x) + 0.5;
}

//
// Exponential
//

float EaseInExpo(float x)
{
    return pow(2.0, 10.0*x - 10.0);
}

float EaseOutExpo(float x)
{
    return -pow(2.0, -10.0*x) + 1.0;
}

float EaseInOutExpo(float x)
{
    if (x < 0.5)
    {
        return 0.5*pow(2.0, 20.0*x - 10.0);
    }
    return -0.5*pow(2.0, -20.0*x + 10.0) + 1.0;
}

//
// Circular
//

float EaseInCirc(float x)
{
    return -sqrt(-x*x + 1.0) + 1.0;
}

float EaseOutCirc(float x)
{
    return sqrt(x*(2.0 - x));
}

float EaseInOutCirc(float x)
{
    if (x < 0.5)
    {
        return -0.5*sqrt(-4.0*x*x + 1.0) + 0.5;
    }
    x = 2.0*x - 2.0;
    return 0.5*sqrt(-x*x + 1.0) + 0.5;
}

//
// Back
//

float EaseInBack(float x)
{
    return x*x*x - x*sin(UNITY_PI*x);
}

float EaseOutBack(float x)
{
    x = 1.0 - x;
    return -x*x*x + x*sin(UNITY_PI*x) + 1.0;
}

float EaseInOutBack(float x)
{
    if (x < 0.5)
    {
        return 0.5*EaseInBack(2.0*x);
    }
    return -0.5*EaseInBack(-2.0*x + 2.0) + 1.0;
}

//
// Elastic
//

float EaseInElastic(float x)
{
    return sin(13.0*UNITY_HALF_PI*x)*pow(2.0, 10.0*x - 10.0);
}

float EaseOutElastic(float x)
{
    return sin(-13.0*UNITY_HALF_PI*x - 13.0*UNITY_HALF_PI)*pow(2.0, -10.0*x) + 1.0;
}

float EaseInOutElastic(float x)
{
    if (x < 0.5)
    {
        return 0.5*sin(26.0*UNITY_HALF_PI*x)*pow(2.0, 20.0*x - 10.0);
    }
    return 0.5*sin(-26.0*UNITY_HALF_PI*x)*pow(2.0, -20.0*x + 10.0) + 1.0;
}

//
// Bounce
//

float EaseOutBounce(float x)
{
    const float a = 4356.0/361.0;
    const float b = -35442.0/1805.0;
    const float c = 16061.0/1805.0;

    float x2 = x*x;

    if (x < 4.0/11.0)
    {
        return 7.5625*x2;
    }
    else if (x < 8.0/11.0)
    {
        return 9.075*x2 + (-9.9*x + 3.4);
    }
    else if (x < 0.9)
    {
        return a*x2 + (b*x + c);
    }
    else
    {
        return 10.8*x2 + (-20.52*x + 10.72);
    }
}

float EaseInBounce(float x)
{
    return 1.0 - EaseOutBounce(1.0 - x);
}

float EaseInOutBounce(float x)
{
    if (x < 0.5)
    {
        return -0.5*EaseOutBounce(-2.0*x + 1.0) + 0.5;
    }
    return 0.5*EaseOutBounce(2.0*x - 1.0) + 0.5;
}

#endif
