//
// Kvant/SprayMV - Particle system with motion vectors support
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#include "UnityCG.cginc"

// Particle position buffer
// .xyz = particle position
// .w   = life (+0.5 -> -0.5)
sampler2D _PositionBuffer;
float4 _PositionBuffer_TexelSize;

sampler2D _PreviousPositionBuffer;
float4 _PreviousPositionBuffer_TexelSize;

// Particle velocity buffer
// .xyz = particle velocity
sampler2D _VelocityBuffer;
float4 _VelocityBuffer_TexelSize;

// Particle rotation buffer
// .xyzw = particle rotation
sampler2D _RotationBuffer;
float4 _RotationBuffer_TexelSize;

sampler2D _PreviousRotationBuffer;
float4 _PreviousRotationBuffer_TexelSize;

// Particle color options
fixed _ColorMode; // 0 = constant, 1 = animate
half4 _Color;
half4 _Color2;

// Scale factor
float2 _Scale; // (min, max)

// Seed for PRNG
float _RandomSeed;

// PRNG function
float UVRandom(float2 uv, float salt)
{
    uv += float2(salt, _RandomSeed);
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 QMult(float4 q1, float4 q2)
{
    float3 ijk = q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz);
    return float4(ijk, q1.w * q2.w - dot(q1.xyz, q2.xyz));
}

// Vector rotation with a quaternion
// http://mathworld.wolfram.com/Quaternion.html
float3 RotateVector(float3 v, float4 r)
{
    float4 r_c = r * float4(-1, -1, -1, 1);
    return QMult(r, QMult(float4(v, 0), r_c)).xyz;
}

// Scaling animation function
float ScaleAnimation(float2 uv, float time01)
{
    float s = lerp(_Scale.x, _Scale.y, UVRandom(uv, 14));
    // Linear scaling animation with life time
    // (0, 0) -> (0.1, 1) -> (0.9, 1) -> (1, 0)
    return s * min(1, 5 - abs(5 - time01 * 10));
}

// Color animation function
float4 ColorAnimation(float2 uv, float time01)
{
#if _COLORMODE_RANDOM
    return lerp(_Color, _Color2, UVRandom(uv, 15));
#else
    return lerp(_Color, _Color2, (1 - time01) * _ColorMode);
#endif
}
