#ifndef PROCEDURAL_TOOLKIT_COMMON_INCLUDED
#define PROCEDURAL_TOOLKIT_COMMON_INCLUDED

//
// Collection of shaping and debug functions
//

#include "UnityCG.cginc"

//
// Step functions
//

float CalculateScreenAA()
{
    return length(float2(_ScreenParams.z - 1.0, _ScreenParams.w - 1.0));
}

float SmoothStep0(float x, float aa)
{
    return smoothstep(0.0, aa, x);
}

float SmoothStep0(float x)
{
    return smoothstep(0.0, CalculateScreenAA(), x);
}

float InverseSmoothStep0(float x, float aa)
{
    return smoothstep(aa, 0.0, x);
}

float InverseSmoothStep0(float x)
{
    return smoothstep(CalculateScreenAA(), 0.0, x);
}

float RectangleStep(float from, float x)
{
    return (x > from) ? 1.0 : 0.0;
}

float LineStep(float from, float to, float x)
{
    return saturate((x - from)/(to - from));
}

//
// Wave functions
//

float TriangleWave(float x)
{
    return 2.0*abs(frac(x + 0.5) - 0.5);
}

//
// Pulse functions
//

float RectanglePulse(float x, float from, float to)
{
    return (x > from && x < to) ? 1.0 : 0.0;
}

float TrianglePulse(float x, float from, float to)
{
    return max(1.0 - abs(((x - from)*2.0)/(to - from) - 1.0), 0.0);
}

float SawPulse(float x, float from, float to)
{
    return LineStep(from, to, x) - step(to, x);
}

float LinePulse(float x, float fromA, float fromB, float toA, float toB)
{
    return LineStep(fromA, fromB, x) - LineStep(toA, toB, x);
}

//
// Debug
//

float DebugValuePattern(float value, float offset, float aa)
{
    value = frac(value);
    return smoothstep(offset, offset + aa, value)*smoothstep(1.0 - offset, 1.0 - offset - aa, value);
}

float DebugValuePatternSimple(float value)
{
    float aa = 0.005;
    float pattern1 = DebugValuePattern(value, aa*0.5, aa);
    float pattern2 = DebugValuePattern(value*2.0, aa*0.1, aa*2.0);
    float pattern10 = DebugValuePattern(value*10.0, -aa*10.0, aa*20.0);
    return pattern1*pattern2*pattern10;
}

float DebugValuePattern(float value)
{
    float aa = 0.005;
    float pattern100 = DebugValuePattern(value*100.0, -aa*50.0, aa*100.0);
    return DebugValuePatternSimple(value)*pattern100;
}

float4 DebugValueColor(float value)
{
    float offset = sign(value)*0.3;
    float4 nearColor = float4(0.4 + offset, 0.35, 0.4 - offset, 1.0);
    float4 farColor = float4(0.55 + offset, 0.65, 0.55 - offset, 1.0);
    float4 color = lerp(nearColor, farColor, saturate(abs(value*10.0)));
#ifndef UNITY_COLORSPACE_GAMMA
    color.rgb = GammaToLinearSpace(color.rgb);
#endif
    return color;
}

float4 DebugValueColor(float2 value)
{
    float offsetX = sign(value.x)*0.3;
    float offsetY = step(value.y, 0.0)*0.25;
    float4 nearColor = float4(0.4 + offsetX, 0.35 - offsetY, 0.4 - offsetX, 1.0);
    float4 farColor = float4(0.55 + offsetX, 0.65 - offsetY, 0.55 - offsetX, 1.0);
    float t = 1.0 - length(min((abs(value) - float2(0.1, 0.1))*10.0, float2(0.0, 0.0)));
    float4 color = lerp(nearColor, farColor, t);
#ifndef UNITY_COLORSPACE_GAMMA
    color.rgb = GammaToLinearSpace(color.rgb);
#endif
    return color;
}

float4 DebugValueSimple(float value)
{
    return DebugValuePatternSimple(value)*DebugValueColor(value);
}

float4 DebugValue(float value)
{
    return DebugValuePattern(value)*DebugValueColor(value);
}

float4 DebugValueSimple(float2 value)
{
    return DebugValuePatternSimple(value.x)*DebugValuePatternSimple(value.y)*DebugValueColor(value);
}

float4 DebugValue(float2 value)
{
    return DebugValuePattern(value.x)*DebugValuePattern(value.y)*DebugValueColor(value);
}

#endif
